using System.Net.Http;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScorpPlus.Contexts;
using ScorpPlus.Services;
using ScorpPlus.Services.Notifications;

namespace ScorpPlusAdminDashboard
{
    public class Startup
    {
        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration">Dashboard configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Dashboard configurations
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Configuring Dashboard services
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
            services.AddDbContext<DeviceContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<RoomContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<NotificationContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
            services.AddDbContext<AccessContext>(opt =>
                opt.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            // Configuring notification service
            var telegramBotConfiguration = Configuration.GetSection("TelegramBot");
            services.AddSingleton(new TelegramNotificator(telegramBotConfiguration["Token"]));

            var mailingConfiguration = Configuration.GetSection("MailingServer");
            services.AddSingleton(new EmailNotificator(
                mailingConfiguration["Username"],
                mailingConfiguration["Password"],
                mailingConfiguration["MailName"],
                mailingConfiguration["MailAddress"],
                mailingConfiguration["Host"],
                int.Parse(mailingConfiguration["Port"])));

            services.AddScoped<NotificationService>();
            
            services.AddAuthentication(
                    CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddAuthorization();
            services.AddBlazoredSessionStorage();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddSingleton(new IndividualService(Configuration["AdataToken"]));
            services.AddHttpClient();
            services.AddScoped<HttpClient>();
        }

        /// <summary>
        /// Configuring application environment
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Web Host Environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}