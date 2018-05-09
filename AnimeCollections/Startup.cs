using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AnimeCollections.Data;
using Microsoft.AspNetCore.Identity;
using AnimeCollections.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AnimeCollections.Authorization;

namespace AnimeCollections
{
    public class Startup
    {



        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;

            var builder = new ConfigurationBuilder();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {



            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext")));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                    //  .AddEntityFrameworkStores<ApplicationDbContext>()
                     .AddEntityFrameworkStores<ApplicationDbContext>()
                      .AddDefaultTokenProviders();

            var skipHTTPS = Configuration.GetValue<bool>("LocalTest:skipHTTPS");
            // requires using Microsoft.AspNetCore.Mvc;
            services.Configure<MvcOptions>(options =>
            {
                // Set LocalTest:skipHTTPS to true to skip SSL requrement in 
                // debug mode. This is useful when not using Visual Studio.
                if (Environment.IsDevelopment() && !skipHTTPS)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }
            });

            services.AddMvc();//
        //    .AddRazorPagesOptions(options =>
        //    {
        //        options.Conventions.AuthorizeFolder("/Account/Manage");
           //     options.Conventions.AuthorizePage("/Account/Logout");
       //     });
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });


            services.AddScoped<IAuthorizationHandler,
                                  OwnerAuthorizationHandler>();

            services.AddSingleton<IAuthorizationHandler,
                                  AdministratorsAuthorizationHandler>();

            services.AddSingleton<IAuthorizationHandler,
                                  ManagerAuthorizationHandler>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

           // app.UseMvc();
        }
    }
}
