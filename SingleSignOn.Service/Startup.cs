using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SingleSignOn.Data.Repositories;
using SingleSignOn.DataAccessLayer.Interfaces;
using SingleSignOn.Service.Interfaces;
using SingleSignOn.Service.Services;

namespace SingleSignOn.Service
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
            services.AddMvc();
            services.AddIdentityServer()
                       .AddDeveloperSigningCredential()
                       .AddInMemoryApiResources(Config.GetApiResources()) //setup api client list that calling the server
                       .AddInMemoryIdentityResources(Config.GetIdentityResources()) // setup idenitity for user claim authority
                       .AddInMemoryClients(Config.GetClients()) //setup avaiable api client with details
                       .AddProfileService<UserProfileService>();

            //service injection
            services.AddScoped<IAccountService, AccountService>();

            //repository injection
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
