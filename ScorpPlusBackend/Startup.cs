using ScorpPlusBackend.Contexts;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ScorpPlusBackend
{
    /// <summary>
    /// Startup class of an API
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration">API configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// API configurations
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Configuring API services
        /// </summary>
        /// <param name="services">List of runtime services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding DB contexts
            var dbConnectionString = Configuration.GetConnectionString("ScorpPlusDb");

            services.AddDbContext<UserContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<EmployeeContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<RoomContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<AccessContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<DeviceContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<ClimateContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);

            // Configuring JWT service
            var jwtConfiguration = Configuration.GetSection("Jwt");
            Options.JwtOptions = new Options.Jwt
            {
                Audience = jwtConfiguration["Audience"],
                Issuer = jwtConfiguration["Issuer"],
                Key = jwtConfiguration["Key"],
                Lifetime = int.Parse(jwtConfiguration["Lifetime"])
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Options.JwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = Options.JwtOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = Options.JwtOptions.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddControllers();
        }

        /// <summary>
        /// Configuring application environment
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Web Host Environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}