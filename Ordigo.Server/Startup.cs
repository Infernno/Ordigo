using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ordigo.Server.Core.Contracts;
using Ordigo.Server.Core.Data.Contexts;
using Ordigo.Server.Core.Data.Repositories;
using Ordigo.Server.Core.Factories;
using Ordigo.Server.Core.Services;
using Ordigo.Server.Core.Validation;
using System.Text;

namespace Ordigo.Server
{
    /// <summary>
    /// Класс, предназначенный для изменения настроек сервера
    /// </summary>
    public class Startup
    {
        #region Constructor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Данные конфигурации из appsettings.json
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Выполняет настройку конфигурации сервисов
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

#if USE_SQLITE
            var connectionString = Configuration["Database:ConnectionString"];
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
#else
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("MemoryDb"));
#endif

            ConfigureDependencies(services);
            ConfigureTokenAuth(services);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }

        /// <summary>
        /// Выполняет регистрацию зависимостей в DI контейнере
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        private static void ConfigureDependencies(IServiceCollection services)
        {
            services.AddTransient<IAuthTokenFactory, AuthTokenFactory>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITextNoteRepository, TextNoteRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INoteService, NoteService>();

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = ctx => new ValidationFailedResult(ctx.ModelState);
            });
        }

        /// <summary>
        /// Выполняет настройку проверки токена
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        private void ConfigureTokenAuth(IServiceCollection services)
        {
            var issuer = Configuration["Jwt:Issuer"];
            var encryptionKey = Configuration["Jwt:Key"];

            var bytes = Encoding.ASCII.GetBytes(encryptionKey);
            var key = new SymmetricSecurityKey(bytes);

            var parameters = new TokenValidationParameters
            {
                // Указывает, будет ли проверяться издатель при валидации токена
                ValidateIssuer = true,
                // Будет ли проверяться потребитель токена
                ValidateAudience = false,
                // Проверка времени жизни токена
                ValidateLifetime = false,
                // Установка ключа безопасности
                IssuerSigningKey = key,
                // Валидация издателя токена
                ValidIssuer = issuer,
                // Валидация ключа безопасности
                ValidateIssuerSigningKey = true
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = parameters;
            });
        }

        #endregion
    }
}
