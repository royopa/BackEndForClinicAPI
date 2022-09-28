using System;
using BackEndForClinicAPI.Models;

namespace BackEndForClinicAPI.DTOs
{
    public class ListOfDoctorsWithMaxSlotsDTO
    {
        public Doctor? Doctor { get; set; }
        public int? TotalTime { get; set; }
    }
}

