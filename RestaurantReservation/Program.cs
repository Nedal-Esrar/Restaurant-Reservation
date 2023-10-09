using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantReservation;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Enums;

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
      }).AddScoped<RestaurantReservationRepository>();
    });
}

var serviceProvider = CreateHostBuilder(args).Build().Services;

var repo = serviceProvider.GetRequiredService<RestaurantReservationRepository>();

#region Customer Create

try
{
  await repo.CreateCustomerAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var customer = new Customer
{
  FirstName = "Nedal",
  LastName = "Ahmad",
  Email = "GG@GG.com",
  PhoneNumber = "1111111111"
};

await repo.CreateCustomerAsync(customer);

#endregion

#region Customer Update

try
{
  await repo.UpdateCustomerAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await repo.UpdateCustomerAsync(new Customer { CustomerId = 11111111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

customer.FirstName = "GGGGGGG";

await repo.UpdateCustomerAsync(customer);

#endregion

#region Customer Delete

try
{
  await repo.DeleteCustomerAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteCustomerAsync(customer.CustomerId);

#endregion

#region Employee Create

try
{
  await repo.CreateEmployeeAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var employee = new Employee
{
  RestaurantId = 1,
  FirstName = "GG",
  LastName = "GG",
  Position = EmployeePosition.Bartender
};

await repo.CreateEmployeeAsync(employee);

#endregion

#region Employee Update

try
{
  await repo.UpdateEmployeeAsync(new Employee { EmployeeId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

employee.FirstName = "BG";

await repo.UpdateEmployeeAsync(employee);

#endregion

#region Employee Delete

try
{
  await repo.DeleteEmployeeAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteEmployeeAsync(employee.EmployeeId);

#endregion