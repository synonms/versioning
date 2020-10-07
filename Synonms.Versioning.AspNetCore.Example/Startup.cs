using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Synonms.Versioning.Core.Serialisation;
using Synonms.Versioning.Swashbuckle;
using Synonms.Versioning.Web;

namespace Synonms.Versioning.AspNetCore.Example
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
            services.AddControllers();

            // You can add IApiVersionReaders in here using the factory thingy
            services.AddScoped<ApiVersionMiddleware>();

            // Inject the custom JSON serialiser into your controller
            services.AddScoped<IVersionableSerialiser, VersionableJsonSerialiser>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My Awesome API",
                    Description = "Do all the things",
                    Version = "1.0"
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "My Even More Awesome API",
                    Description = "Do all the things plus more",
                    Version = "2.0"
                });

                c.EnableAnnotations();

                // Enable pruning of paths and schemas in Swagger docs
                c.DocumentFilter<VersionableDocumentFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            // This must be before UseEndpoints()
            app.UseMiddleware<ApiVersionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            });
        }
    }
}
