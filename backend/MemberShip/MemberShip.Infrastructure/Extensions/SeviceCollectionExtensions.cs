using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MemberShip.Application.IRepositories;
using MemberShip.Infrastructure.Repositories;
using MemberShip.Domain;
using MemberShip.Application.IUOF;
using MemberShip.Infrastructure.UOF;
using Hangfire;
using MemberShip.Application.IEmailsender;

namespace MemberShip.Infrastructure.Extensions 
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMembershipInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddDbContext<AppContext>(options => options.UseSqlServer(connectionString: "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"));
            services.AddScoped<IGenericrepository<User>, Genericrepository<User>>();
            services.AddScoped<IUnitofwork,Unitofwork>();
            services.AddScoped<IEmailsender,Emailsender>();

            services.AddHangfire(x => x.UseSqlServerStorage("Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"));
            services.AddHangfireServer();
  
            return services;

        }
    }
}
