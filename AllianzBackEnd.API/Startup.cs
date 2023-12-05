

using AllianzBackEnd.Infrastructure;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using AllianzBackEnd.Domain.Config;
using AllianzBackEnd.Messaging;
using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Infrastructure.Repositories.PurchaseHistories;
using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Infrastructure.Repositories.Users;
using AllianzBackEnd.Core.Managers;
using System.Text.Json.Serialization;

namespace AllianzBackEnd.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public IWebHostEnvironment HostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
            Console.WriteLine($"Environment is {env.EnvironmentName}");
            BuildConfigDev();
            //BuildConfig();
        }

        //private void BuildConfig()
        //{
        //    var basePath = "/app/Secrets";

        //    Configuration = new ConfigurationBuilder()
        //        .SetBasePath(basePath)
        //        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
        //        .AddEnvironmentVariables()
        //        .Build();
        //}

        private static void BuildConfigDev()
        {
            _ = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            //var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();


            services.AddHttpClient<ApiClient>();

            services.AddScoped<IPurchaseHistoryRepository, PurchaseHistoryRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<PurchaseHistoryManager>();

            services.AddScoped<UserManager>();

            //services.AddSingleton(provider =>
            //{
            //    var settings = appSettings;
            //    return settings;
            //});


            ////JWT TOKEN VALIDATION
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(appSettings.JwtSecretKey)),
            //        RequireExpirationTime = appSettings.IsExpirationSet,
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //    };
            //});
            services.AddLogging();
            services.AddSingleton(p => Configuration.GetSection(nameof(FlutterWaveConfig)).Get<FlutterWaveConfig>());
            //services.AddSingleton(p => Configuration.GetSection(nameof(GeneratePdfConfig)).Get<GeneratePdfConfig>());
            //services.AddSingleton(p => Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>());

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(GetDatabaseConnection());
            });

            services.AddControllers().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                jsonOptions.JsonSerializerOptions.MaxDepth = 64;
            });


            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Allianz API", Version = "v1" });

                c.DocumentFilter<SwaggerFilter>(Configuration.GetValue<string>("SwaggerFilterUrl"));

                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});

            });

            services.AddCors();
        }

        private string GetDatabaseConnection() => Configuration?.GetConnectionString("TestDatabase")!;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            bool isDev = string.Equals(Environment.GetEnvironmentVariable("ENVIRONMENT"), "dev", StringComparison.OrdinalIgnoreCase);

            if (env.IsDevelopment() || isDev)
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure the HTTP request pipeline.

            app.UseHsts();

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                var swaggerBase = !string.IsNullOrEmpty(c.RoutePrefix) ? ".." : ".";
                c.SwaggerEndpoint($"{swaggerBase}/swagger/v1/swagger.json", "Allianz");
            });


            app.UseRouting();

            app.UseCors(config =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
                config.WithHeaders("Authorization", "Content-Type");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
