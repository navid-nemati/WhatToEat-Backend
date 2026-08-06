namespace Authentication_Practice.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> SaveImageAsync(
            IFormFile file,
            string folderName,
            CancellationToken cancellation =  default);

        Task DeleteAsync(
            string? reletivePath,
            CancellationToken cancellationToken = default);
    }
}
