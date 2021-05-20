using System;
using System.IO;
using System.Reflection;
using System.Text;
using ScorpPlus.Contexts;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScorpPlus.Services.Notifications;
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
            Program.Configuration = configuration;
        }

        /// <summary>
        /// Configuring API services
        /// </summary>
        /// <param name="services">List of runtime services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding DB contexts
            var dbConnectionString = Program.Configuration.GetConnectionString("ScorpPlusDb");

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
            services.AddDbContext<NotificationContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);

            // Starts background analyzer
            // new BackgroundAnalyzer().Start();

            // Configuring notification service
            var telegramBotConfiguration = Program.Configuration.GetSection("TelegramBot");
            services.AddSingleton(new TelegramNotificator(telegramBotConfiguration["Token"]));

            var mailingConfiguration = Program.Configuration.GetSection("MailingServer");
            services.AddSingleton(new EmailNotificator(
                mailingConfiguration["Username"],
                mailingConfiguration["Password"],
                mailingConfiguration["MailName"],
                mailingConfiguration["MailAddress"],
                mailingConfiguration["Host"],
                int.Parse(mailingConfiguration["Port"])));

            services.AddScoped<NotificationService>();
            
            // Configuring JWT service
            var jwtConfiguration = Program.Configuration.GetSection("Jwt");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfiguration["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtConfiguration["Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration["Key"])),
                        ValidateIssuerSigningKey = true,
                    };
                });


            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SCorpAPI",
                    Description = "An API for scorp CRM system",
                    Contact = new OpenApiContact
                    {
                        Name = "SCorpAPI"
                    },
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
                    } 
                });
            });
        }

        /// <summary>
        /// Configuring application environment
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Web Host Environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Adata Scraping API V1");
                c.RoutePrefix = "swagger";
            });
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseCors(builder =>
                builder.WithOrigins()
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}