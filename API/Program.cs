
using API.Repositories;
using Microsoft.EntityFrameworkCore;

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var acceptedUrls = builder.Configuration.GetSection("AllowedHosts").Value?.Split(",");
            app.UseCors(builder => builder.WithOrigins(acceptedUrls ?? ["*"])
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
