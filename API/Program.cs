
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Configuration;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserDatabase")));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            Console.WriteLine("AAAAA");
            Console.WriteLine(builder.Configuration.GetConnectionString("UserDatabase"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (true /*app.Environment.IsDevelopment()*/)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var urlAceptadas = builder.Configuration.GetSection("AllowedHosts").Value?.Split(",");
            app.UseCors(builder => builder.WithOrigins(urlAceptadas ?? ["*"])
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  );

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UserContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}
