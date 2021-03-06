using LunarDeckFoxyApi.Authentication;
using LunarDeckFoxyApi.Models;
using LunarDeckFoxyApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LunarDeckFoxyApi
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
            // authentication with JWT tokens
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = Configuration.GetSection("Auth:JwtIssuer").Value,
                        ValidAudience = Configuration.GetSection("Auth:JwtIssuer").Value,
                        IssuerSigningKey = new JwtSecurityKey(Configuration).Create()
                    };
                });

            // TODO: add authorization policies
            services.AddAuthorization();

            // added authorization filter for all API endpoints
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });

            // database settings mapping
            services.Configure<LunarDeckDatabaseSettingsModel>(Configuration.GetSection("LunarDeckDB"));
            services.AddScoped<HangoutsServices>();
            services.AddScoped<AuthenticationServices>();

            // swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "lunar_deck_api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "lunar_deck_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
