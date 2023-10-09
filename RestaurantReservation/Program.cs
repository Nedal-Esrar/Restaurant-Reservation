using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantReservation.Db;

static IHostBuilder CreateHostBuilder(string[] args)
{
  return Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
      services.AddDbContext<RestaurantReservationDbContext>(options =>
      {
        var configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", false, true)
          .Build();

        options
          .UseSqlServer(configuration.GetConnectionString("SqlConnection"));
      });
    });
}

var serviceProvider = CreateHostBuilder(args).Build().Services;