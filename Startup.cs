using System;
using System.Threading.Tasks;
using hki.web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using hki.web.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace hki.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Data Source=SQL5033.myASP.NET;Initial Catalog=DB_A27A42_coep;User Id=DB_A27A42_coep_admin;Password=Agusvaldes1!;"));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<RoleManager<IdentityRole>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
           CreateRoles(serviceProvider).Wait();
        }
 
        //Creacion de Roles
        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            
            //adding custom roles
            
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            string[] roleNames = { "Administrador", "Produccion", "Almacen", "Programacion", "Calidad" };
            
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //creating a super user who could maintain the web app
            var poweruser = new ApplicationUser
            {
                UserName = "aruiz",
                Email = "aruiz@optm.mx"
            };
            var UserPassword = "Cxy4N55N!";
            var _user = await UserManager.FindByEmailAsync(poweruser.Email);
            if(_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, "Administrador");
                }
               
            }
        }
        
    }
}