namespace RoleBase_Api.Models
{
    public class UserOTP
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string OTP { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; }

        public User User { get; set; }
    }
}
