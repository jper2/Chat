namespace Chat.Core.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; } // Optional: only for login
        public string Message { get; set; } // Error or success message
    }

}
