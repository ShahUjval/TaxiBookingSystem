using System;
using System.IO;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TaxiBookingAPI.Interfaces;
using TaxiBookingAPI.Model;
using TaxiBookingAPI.TaxiBookingModule;
using TaxiBookingAPI.Validators;

namespace TaxiBookingAPI
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
            
            services.AddMvc().AddFluentValidation();

            services.AddTransient<IValidator<Location>, LocationValidator>();
            // Inject the Taxi Booking Service in Default IOC Container Service Collection
            services.AddSingleton<ITaxiBookingService,TaxiBookingService>();
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TaxiBooking API",
                    Description = "a simple taxi booking system in a 2D grid || using ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Ujval Shah",
                        Email = "shah.ujval@gmail.com",
                        Url = new Uri("https://github.com/ShahUjval"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT"
                    }
                });
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaxiBooking API V1");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}