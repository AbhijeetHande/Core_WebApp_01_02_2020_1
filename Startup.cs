using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core_WebApp.Services;
using Core_WebApp.CustomFilters;

namespace Core_WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //Register objec in dependency
        //1 database context
        //-------EF core DBContext
        //2 mvc options
        //--------Filters
        //--------Formatters

        //3 security
        //--------Authentication For user
        //--------Authorization
        //-------------Based on Roles
        //----------------Role Based Policies
        //--------Based on Json Web Token

        //4 CORS policies
        //5 custom srvices
        //--------Domain Based service aka Bussiness logic
        //6 Sessions
        //7 Cookies

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();//MVC request and web api request processing 

            //register the dbcontext in DI controller
            services.AddControllersWithViews(
                options => options.Filters.Add(typeof(MyExceptionFilter))
                );
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppDbConnection"));
            });

            services.AddScoped<IRepository<Category, int>, CategoryRepository>();
            services.AddScoped<IRepository<Product, int>, ProductRepository>();
        }
        
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //Iapplication Builder -->used to manage HTTP request using middleware
        //Iwebhostenviorment -->detect the hostin env for excution
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //detect the enviorment 
            if (env.IsDevelopment())
            {
                //standard framework error page
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //standard excepion handler
                app.UseExceptionHandler("/Home/Error");
            }
            //by default uses WWWROOT to read static file  ex js,css,ing or any other custom static file
            //to render http response
            app.UseStaticFiles();
            //Routing for api and MVC based on end point
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
