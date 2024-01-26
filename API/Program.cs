using Commands.User;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Queries.User;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            services.AddDbContext<UserContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserDatabase")));

            // Add services to the container.
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(typeof(GetAllUsersQuery).Assembly, typeof(CreateUserCommand).Assembly)
            );

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

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
