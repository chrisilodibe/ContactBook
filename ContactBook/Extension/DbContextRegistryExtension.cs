using CloudinaryDotNet;
using ContactBook.Core.Interfaces;
using ContactBook.Core.implementations;
using ContactBook.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Extension
{
    public static class DbContextRegistryExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
           services.AddDbContext<ContactBookDbContext>(options =>
           {
               options.UseSqlServer(configuration.GetConnectionString("Connection"));
           });

            var cloudinarySettings = configuration.GetSection("Cloudinary");
            var account = new Account(
                cloudinarySettings["CloudName"],
                cloudinarySettings["ApiKey"],
                cloudinarySettings["ApiSecret"]
                );
            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);


            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICrudRepository, CrudRepository>();
        }

        
    }
}
