
using Serilog;

namespace MagicVilla_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Custom logging 
            //Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            //                 .WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

            //builder.Host.UseSerilog();

            builder.Services.AddControllers(options =>
            {
                //options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson();
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
