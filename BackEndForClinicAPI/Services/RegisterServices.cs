using System;
using System.Collections.Concurrent;
using System.Text;
using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegisterServices
    {
        public static void AddServicesForInterfaceAndControllers(this IServiceCollection services)
        {
            services.AddScoped<IDoctorInterface, DoctorService>();
            services.AddScoped<IPatientInterface, PatientService>();
            services.AddScoped<IAppointmentInterface, AppointmentService>();
            services.AddScoped<IAdminInterface, AdminService>();
            services.AddSingleton<AuthenticationAndAuthorizationHelpers>();
            services.AddSingleton<ConcurrentDictionary<string, string>>();
            
        }
    }
}

