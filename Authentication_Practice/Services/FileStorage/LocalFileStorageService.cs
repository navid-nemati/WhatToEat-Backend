namespace Authentication_Practice.Services.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        private const long MaxImageSize = 5 * 1024 * 1024; // 5MB

        private static readonly Dictionary<string, string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png",
            [".webp"] = "image/webp"
        };

        public LocalFileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(
            IFormFile file,
            string folderName,
            CancellationToken cancellationToken = default)
        {
            if (file is null || file.Length == 0)
                throw new InvalidOperationException("فایل تصویر انتخاب نشده است");

            if (file.Length > MaxImageSize)
                throw new InvalidOperationException(
                "حجم تصویر نباید بیشتر از 5 مگابایت باشد");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension) ||
            !AllowedExtensions.ContainsKey(extension))
            {
                throw new InvalidOperationException(
                    "فرمت تصویر باید JPG، JPEG، PNG یا WEBP باشد");
            }

            if (!AllowedExtensions.TryGetValue(extension, out var expectedContentType) ||
            !string.Equals(
                file.ContentType,
                expectedContentType,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("نوع فایل تصویر معتبر نیست");
            }

            if (!await HasValidImageSignatureAsync(
                file,
                extension,
                cancellationToken))
            {
                throw new InvalidOperationException(
                    "محتوای فایل با فرمت تصویر مطابقت ندارد");
            }

            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot");
            }

            var normalizedFolder = folderName
            .Replace('\\', '/')
            .Trim('/');

            var physicalFolderPath = Path.Combine(
                webRootPath,
                normalizedFolder.Replace('/', Path.DirectorySeparatorChar));

            Directory.CreateDirectory(physicalFolderPath);

            var generatedFileName = $"{Guid.NewGuid():N}{extension}";

            var physicalFilePath = Path.Combine(
                physicalFolderPath,
                generatedFileName);

            await using var stream = new FileStream(
                physicalFilePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None);

            await file.CopyToAsync(stream, cancellationToken);

            return $"/{normalizedFolder}/{generatedFileName}";
        }

        public Task DeleteAsync(
            string? relativePath,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return Task.CompletedTask;

            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot");
            }

            var cleanedPath = relativePath
            .TrimStart('/')
            .Replace('/', Path.DirectorySeparatorChar);

            var fullPath = Path.GetFullPath(
                Path.Combine(webRootPath, cleanedPath));

            var safeRootPath = Path.GetFullPath(webRootPath);

            // جلوگیری از Path Traversal
            if (!fullPath.StartsWith(
                    safeRootPath,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("مسیر فایل نامعتبر است");
            }

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        private static async Task<bool> HasValidImageSignatureAsync(
       IFormFile file,
       string extension,
       CancellationToken cancellationToken)
        {
            var header = new byte[12];

            await using var stream = file.OpenReadStream();

            var bytesRead = await stream.ReadAsync(
                header.AsMemory(0, header.Length),
                cancellationToken);

            if (bytesRead < 4)
                return false;

            return extension switch
            {
                ".jpg" or ".jpeg" =>
                    header[0] == 0xFF &&
                    header[1] == 0xD8 &&
                    header[2] == 0xFF,

                ".png" =>
                    bytesRead >= 8 &&
                    header[0] == 0x89 &&
                    header[1] == 0x50 &&
                    header[2] == 0x4E &&
                    header[3] == 0x47 &&
                    header[4] == 0x0D &&
                    header[5] == 0x0A &&
                    header[6] == 0x1A &&
                    header[7] == 0x0A,

                ".webp" =>
                    bytesRead >= 12 &&
                    header[0] == 0x52 && // R
                    header[1] == 0x49 && // I
                    header[2] == 0x46 && // F
                    header[3] == 0x46 && // F
                    header[8] == 0x57 && // W
                    header[9] == 0x45 && // E
                    header[10] == 0x42 && // B
                    header[11] == 0x50,   // P

                _ => false
            };
        }
    }
}
