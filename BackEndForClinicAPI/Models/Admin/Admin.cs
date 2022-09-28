using System;
namespace BackEndForClinicAPI.Models
{
    public class Admin: UserModel
    {
        public string AdminIdInLab { get; set; } = string.Empty;
    }
}