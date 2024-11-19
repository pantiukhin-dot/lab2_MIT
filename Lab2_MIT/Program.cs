using Lab2_MIT.DAL.Services;
using Lab2_MIT.DAL.Settings;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Lab2_MIT.DAL;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<LabDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.Configure<MongoDBSettings>(
            builder.Configuration.GetSection("MongoDBSettings")
        );

        builder.Services.AddSingleton<IMongoClient>(s =>
        {
            var settings = s.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        builder.Services.AddSingleton<StudentService>();
        builder.Services.AddHostedService<UpdateService>();

        var app = builder.Build();
        DatabaseSeeder.Seed(app.Services);
        app.Run();
    }
}