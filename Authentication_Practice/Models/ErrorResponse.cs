namespace Authentication_Practice.Models
{
    public class ErrorResponse
    {
        public bool Success => false;
        public int Status { get; set; }
        public string Message { get; set; } = "";
        public object? Errors { get; set; }
    }
}
