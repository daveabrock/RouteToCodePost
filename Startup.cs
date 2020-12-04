using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RouteToCodePost.Data;

namespace RouteToCodePost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SampleContext>(options =>
               options.UseInMemoryDatabase("SampleData"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                var log = endpoints.ServiceProvider.GetService<ILogger<Startup>>();
                endpoints.MapGet("/bands", async context =>
                {
                    var repository = context.RequestServices.GetService<SampleContext>();
                    var bands = repository.Bands.ToListAsync();
                    await context.Response.WriteAsJsonAsync(bands);
                });
                endpoints.MapGet("/bands/{id}", async context =>
                {
                    var repository = context.RequestServices.GetService<SampleContext>();
                    var id = context.Request.RouteValues["id"];
                    var band = repository.Bands.FindAsync(Convert.ToInt32(id));
                    await context.Response.WriteAsJsonAsync(band);
                });
                endpoints.MapPost("/bands", async context =>
                {
                    var repository = context.RequestServices.GetService<SampleContext>();

                    if (!context.Request.HasJsonContentType())
                    {
                        context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                        return;
                    }

                    var band = await context.Request.ReadFromJsonAsync<Band>();
                    await repository.SaveChangesAsync();
                    context.Response.StatusCode = StatusCodes.Status201Created;
                    return;
                });

                endpoints.MapDelete("/bands/{id}", async context =>
                {
                    var repository = context.RequestServices.GetService<SampleContext>();

                    var id = context.Request.RouteValues["id"];
                    var band = await repository.Bands.FindAsync(Convert.ToInt32(id));

                    if (band is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        return;
                    }

                    repository.Bands.Remove(band);
                    await repository.SaveChangesAsync();
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    return;
                });
            });
    
}
