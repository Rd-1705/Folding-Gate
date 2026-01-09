namespace foldingGate.Models.DB
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Admin atau Pemilik
        public string NamaLengkap { get; set; }
    }
}
