using System;
namespace BackEndForClinicAPI.Models
{
    public class Token
    {
        public string TokenGenerated { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}

