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

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            // Add services to the container.
            builder.Services.AddRazorPages();
            

            builder.Services.AddDbContext<EccomerceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericoRepository<>));
            builder.Services.AddScoped<IVentaRepository, VentaRepository>();


            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IVentaService,VentaService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IRoleService, RoleService>();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("nuevaPolitica", app =>
                {
                    app.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); 
            }

            app.MapControllers();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseCors("nuevaPolitica");


            app.UseAuthorization();

            app.MapRazorPages();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ejemplo"));
            app.Run();
        }
    }
}
