using Microsoft.EntityFrameworkCore;
using Eccomerce.Repositorio;
using Eccomerce.Repositorio.Contratos;
using Eccomerce.Repositorio.Implementacion;
using Microsoft.AspNetCore.Builder; // Necesario para configuraciones avanzadas de Swagger
using Eccomerce.Utilidades;
using Eccomerce.Servicio;
using Eccomerce.Servicio.BrandService;
using Eccomerce.Servicio.CategoryService;
using Eccomerce.Servicio.DashboardService;
using Eccomerce.Servicio.ProductService;
using Eccomerce.Servicio.UserService;
using Eccomerce.Servicio.VentaService;
using Eccomerce.Servicio.RolesService;
using Eccomerce.Servicio.CartService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var builder = WebApplication.CreateBuilder(args);

            // Configuración JWT
            var jwtSettings = builder.Configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });



            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
            

            builder.Services.AddDbContext<EccomerceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddAutoMapper(config =>
            {
                config.AddProfile<AutoMapperProfile>();
                config.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                config.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            });


            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericoRepository<>));
            builder.Services.AddScoped<IVentaRepository, VentaRepository>();
            builder.Services.AddScoped<IPasswordHasher,BcryptPasswordHasher>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IVentaService,VentaService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("nuevaPolitica", app =>
                {
                    app.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

        
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("nuevaPolitica");

            app.UseAuthentication(); 
            app.UseAuthorization();

            // Configuración Swagger
           
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            
            


            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}
