using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Portfolio.Core.AutoMapper;
using Portfolio.Core.Configuration;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Interfaces.Common;
using Portfolio.Core.Services;
using Portfolio.Core.Services.Common;
using Portfolio.Database;
using Portfolio.Helpers;
using System.IO;

namespace Portfolio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var conf = Configuration.GetSection("Hosting");
            services.AddHttpContextAccessor();

            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);
            services.AddSingleton(appSettings);

            services.AddSingleton(new HostingConfig());
            services.AddScoped<IWebHelper, WebHelper>();

            var config = Configuration["ConnectionString:DefaultConnection"];
            services.AddDbContext<PortfolioContext>(options =>
            {
                options.UseSqlServer(config);
            });

            services.AddAutoMapper(typeof(PortfolioMappings));

            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ISkillGroupService, SkillGroupService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IAboutMeService, AboutMeService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped(typeof(IBaseRepository<,,>), typeof(BaseRepository<,,>));
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddSingleton<IUploadImageHelper, UploadImageHelper>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bart's Portfolio API", Version = "v1" });
            });

            services.AddCors((options =>
            { options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin()); }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                await context.Response.WriteAsJsonAsync(new { error = exception.Message });
            }));
            
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bart's Portfolio API");
                } );

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            var path = Path.Combine(env.ContentRootPath, "wwwroot\\uploads");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/uploads"
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
