using ExchangeService.Data;
using ExchangeService.Domain;
using ExchangeService.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Config;
using NLog.Web;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Builder;

try
{


    var builder = WebApplication.CreateBuilder(args);


    var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    //var logger = LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "\\nlog.config")).GetCurrentClassLogger();


    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Logging.ClearProviders();
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestHeaders.Add("sec-ch-ua");
        logging.ResponseHeaders.Add("MyResponseHeader");
        logging.MediaTypeOptions.AddText("application/javascript");
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;

    });
    builder.Host.UseNLog();



    builder.Services.AddDbContext<ExchangeRatesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("ExchangeService.Data")));

    builder.Services.AddTransient<ICurrencyExchangeService, CurrencyExchangeService>();
    builder.Services.AddTransient<ExchangeRatesDbContext, ExchangeRatesDbContext>();

    string cobb = builder.Configuration.GetConnectionString("Redis");

    builder.Services.AddStackExchangeRedisCache(redis =>
    {
        string con = builder.Configuration.GetConnectionString("Redis");
        redis.Configuration = con;
    });

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    var app = builder.Build();

    app.UseHttpLogging();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    // THIS FIELD NORMALLY CREATES DB/TABLES BUT BECAUSE OF THE LOGGER ERROR I COULDN'T APPLY THIS SUCCESSFULLY 
    //using (var scope = app.Services.CreateScope())
    //{
    //    var db = scope.ServiceProvider.GetRequiredService<ExchangeRatesDbContext>();
    //    db.Database.Migrate();
    //}

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    //logger.Error(ex);
    throw (ex);
}
finally
{
    NLog.LogManager.Shutdown();
}