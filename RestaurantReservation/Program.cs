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

#region MenuItem Create

try
{
  await repo.CreateMenuItemAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var menuItem = new MenuItem
{
  RestaurantId = 1,
  Name = "LFJHD",
  Description = "adfl;hkadfjh",
  Price = 3134
};

await repo.CreateMenuItemAsync(menuItem);

#endregion

#region MenuItem Update

try
{
  await repo.UpdateMenuItemAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await repo.UpdateMenuItemAsync(new MenuItem { ItemId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

menuItem.Name = "adfpiadhfiadfadf";

await repo.UpdateMenuItemAsync(menuItem);

#endregion

#region MenuItem Delete

try
{
  await repo.DeleteMenuItemAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteMenuItemAsync(menuItem.ItemId);

#endregion

#region Order Create

try
{
  await repo.CreateOrderAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var order = new Order
{
  ReservationId = 1,
  EmployeeId = 1,
  OrderDate = DateTime.Now,
  TotalAmount = 1
};

await repo.CreateOrderAsync(order);

#endregion

#region Order Update

try
{
  await repo.UpdateOrderAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await repo.UpdateOrderAsync(new Order { OrderId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

order.TotalAmount = 2;

await repo.UpdateOrderAsync(order);

#endregion

#region Order Delete

try
{
  await repo.DeleteOrderAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteOrderAsync(order.OrderId);

#endregion