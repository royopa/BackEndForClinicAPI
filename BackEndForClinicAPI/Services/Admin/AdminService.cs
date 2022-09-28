using System;
using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndForClinicAPI.Services
{
    public class AdminService : IAdminInterface
    {
        private readonly BackEndForClinicAPIDBContext _dbContext;


        public AdminService(BackEndForClinicAPIDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public async Task<ActionResult<Admin>> AddAdmin(AdminRequestModel addAdminRequest)
        {
            var admin = new Admin()
            {
                Id = Guid.NewGuid(),
                Address = addAdminRequest.Address,
                FullName = addAdminRequest.FullName,
                EmailAddress = addAdminRequest.EmailAddress,
                Phone = addAdminRequest.Phone,
                GivenName = addAdminRequest.GivenName,
                AdminIdInLab = addAdminRequest.AdminIdInLab,
                Password = PasswordEncrypterDecrypter.EncryptPassword(addAdminRequest.Password.ToString().ToLower()),
                Role = Roles.ADMIN.ToString(),
                Surname = addAdminRequest.Surname,
                UserName = addAdminRequest.UserName
            };

            await _dbContext.Admins.AddAsync(admin);
            await _dbContext.SaveChangesAsync();

            return admin;
        }



        public async Task<ActionResult<Admin>?> GetAdmin(Guid id)
        {
            var admin = await _dbContext.Admins.FindAsync(id);

            if (admin == null)
            {
                return null;
            }
            return admin;
        }



        public async Task<ActionResult<List<Admin>>> GetAdmins()
        {
            var listOfAdmins = await _dbContext.Admins.ToListAsync();
            return listOfAdmins;
        }
    }
}

