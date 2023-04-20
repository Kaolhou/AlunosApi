using AlunosApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AlunosApi.Services;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AlunosApi {
    class Program {
        public static void Main(String[] args) {

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAlunoService, AlunosService>();

            builder.Services.AddDbContext<AppDbContext>(options => {
                options.UseMySql(
                    "Server=localhost;database=test;user=root;Password=Kaolhou",
                    new MySqlServerVersion(new Version(8, 0))
                );
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            


            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AlunosApi", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints((endpoint) => {
                endpoint.MapControllers();
            });

            app.Run();
        }
    }
}