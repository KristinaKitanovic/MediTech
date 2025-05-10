using BookListRazor.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;



namespace BookListRazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

<<<<<<< HEAD

=======
        
>>>>>>> f5790289442c8ab3174febe471ec55d8e84c2709
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Sesija traje 30 minuta
                options.Cookie.HttpOnly = true; // Sigurnosna postavka (da cookie ne može biti dostupan putem JavaScript-a)
            });
<<<<<<< HEAD
        }



        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
=======

            
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
        }


        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDatabaseInitializer databaseInitializer)
>>>>>>> f5790289442c8ab3174febe471ec55d8e84c2709
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

<<<<<<< HEAD
=======
            databaseInitializer.InitializeAsync().Wait();  // Koristimo Wait da sa?ekamo izvršenje asinkronog metoda

>>>>>>> f5790289442c8ab3174febe471ec55d8e84c2709
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }


    }
}