using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using CashFlow.DataProvider.EFCore;
using CashFlow.DataProvider.EFCore.Helpers;
using CashFlow.DataProvider.EFCore.Model;
using CashFlow.DataProvider.EFCore.Providers;
using CashFlow.DataProvider.Interfaces;
using CashFlow.Server.Lib.Services;
using CashFlow.Server.Services;
using CashFlow.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CashFlow.Server
{
    public class Startup
    {
        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<DataContext>(x =>
            {
                //Heroku
                if (Env.IsProduction())
                    x.UseNpgsql(new ConnectionStringHerokuHelper(Environment.GetEnvironmentVariable("DATABASE_URL")).ConnectionString, builder => builder.MigrationsAssembly("CashFlow.DataProvider.EFCore"));
                else if (Env.IsDevelopment())
                    x.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), builder => builder.MigrationsAssembly("CashFlow.DataProvider.EFCore"));
                //else if (env.IsDevelopment())
                //    x.UseInMemoryDatabase("TestDb");
            });

            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Pagination"));
            });

            services.AddControllers();

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>();

            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IIncomeProvider, IncomeProvider>();
            services.AddScoped<IExpenseProvider, ExpenseProvider>();
            services.AddScoped<IAssetProvider, AssetProvider>();
            services.AddScoped<IPassiveProvider, PassiveProvider>();

            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IPassiveService, PassiveService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
