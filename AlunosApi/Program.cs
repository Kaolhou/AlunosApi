using AlunosApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AlunosApi.Services;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AlunosApi.Util;

namespace AlunosApi {
    class Program {
        public static void Main(String[] args) {
            DotEnv.Load();
            System.Diagnostics.Debug.WriteLine(Environment.GetEnvironmentVariable("teste"));

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


            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddScoped<IAuthenticate,AuthenticateService>();
            builder.Services.AddScoped<AlunosService>();


            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://kaolhou.dev",
                        ValidAudience = "https://kaolhou.dev",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("No Jwt"))),

                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AlunosApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(option => {
                option.WithOrigins("http://localhost:3000", "http://localhost:5173");
                option.AllowAnyHeader();
                option.AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints((endpoint) => {
                endpoint.MapControllers();
            });

            app.Run();
        }
    }
}