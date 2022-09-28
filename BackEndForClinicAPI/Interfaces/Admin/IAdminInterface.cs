using System;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForClinicAPI.Interfaces
{
    public interface IAdminInterface
    {
        public Task<ActionResult<List<Admin>>> GetAdmins();
        public Task<ActionResult<Admin>?> GetAdmin(Guid id);
        public Task<ActionResult<Admin>> AddAdmin(AdminRequestModel addAdminRequest);
    }
}

