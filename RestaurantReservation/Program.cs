using System.Text;
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

#region OrderItem Create

try
{
  await repo.CreateOrderItemAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var orderItem = new OrderItem
{
  OrderId = 1,
  ItemId = 1,
  Quantity = 3
};

await repo.CreateOrderItemAsync(orderItem);

#endregion

#region OrderItem Update

try
{
  await repo.UpdateOrderItemAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await repo.UpdateOrderItemAsync(new OrderItem { OrderItemId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

orderItem.Quantity = 314;

await repo.UpdateOrderItemAsync(orderItem);

#endregion

#region OrderItem Delete

try
{
  await repo.DeleteOrderItemAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteOrderItemAsync(orderItem.OrderItemId);

#endregion

#region Reservation Create

try
{
  await repo.CreateReservationAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var reservation = new Reservation
{
  CustomerId = 1,
  RestaurantId = 1,
  TableId = 1,
  ReservationDate = DateTime.Now,
  PartySize = 5
};

await repo.CreateReservationAsync(reservation);

#endregion

#region Reservation Update

try
{
  await repo.UpdateReservationAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await repo.UpdateReservationAsync(new Reservation { ReservationId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

reservation.PartySize = 6;

await repo.UpdateReservationAsync(reservation);

#endregion

#region Reservation Delete

try
{
  await repo.DeleteReservationAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteReservationAsync(reservation.ReservationId);

#endregion

#region Restaurant Create

try
{
  await repo.CreateRestaurantAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var restaurant = new Restaurant
{
  Name = "THIS IS WHERE YOU WILL EAT",
  Address = "GG/GG/GG/GG",
  PhoneNumber = "3148679",
  OpeningHours = "02:00-14:00"
};

await repo.CreateRestaurantAsync(restaurant);

#endregion

#region Restaurant Update

try
{
  await repo.UpdateRestaurantAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await repo.UpdateRestaurantAsync(new Restaurant { RestaurantId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

restaurant.Name = "YOU WILL NOT EAT HERE";

await repo.UpdateRestaurantAsync(restaurant);

#endregion

#region Restaurant Delete

try
{
  await repo.DeleteRestaurantAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteRestaurantAsync(restaurant.RestaurantId);

#endregion

#region Table Create

try
{
  await repo.CreateTableAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

var table = new Table
{
  RestaurantId = 1,
  Capacity = 134134
};

await repo.CreateTableAsync(table);

#endregion

#region Table Update

try
{
  await repo.UpdateTableAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await repo.UpdateTableAsync(new Table { TableId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

table.Capacity = 4;

await repo.UpdateTableAsync(table);

#endregion

#region Table Delete

try
{
  await repo.DeleteTableAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await repo.DeleteTableAsync(table.TableId);

#endregion

#region List All Managers

var managers = await repo.ListManagersAsync();

foreach (var e in managers) Console.WriteLine($"{e.EmployeeId},{e.FirstName},{e.LastName}");

#endregion

#region Get Reservations By Customer

try
{
  await repo.GetReservationsByCustomerAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var reservations = await repo.GetReservationsByCustomerAsync(1);

foreach (var r in reservations)
  Console.WriteLine(
    $"{r.ReservationId},{r.CustomerId},{r.RestaurantId},{r.TableId},{r.ReservationDate},{r.PartySize}");

#endregion

#region List Orders And MenuItems

try
{
  await repo.ListOrdersAndMenuItemsAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var orders = await repo.ListOrdersAndMenuItemsAsync(1);

foreach (var o in orders)
  Console.WriteLine(
    $@"
        OrderId: {o.OrderId},
        MenuItems: {o.OrderItems
          .Aggregate(new StringBuilder(), (acc, x) => acc.Append($"({x.Item.ItemId},{x.Item.Name},{x.Item.RestaurantId},Quantity: {x.Quantity})"))}");

#endregion

#region List Ordered MenuItems

try
{
  await repo.ListOrderedMenuItemsAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var orderedItems = await repo.ListOrderedMenuItemsAsync(1);

foreach (var orderedItem in orderedItems)
  Console.WriteLine($"{orderedItem.ItemId},{orderedItem.Name},{orderedItem.RestaurantId},{orderedItem.Price}");

#endregion

#region Calculate Average Order Amount

try
{
  await repo.CalculateAverageOrderAmountAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var averageOrderAmount = await repo.CalculateAverageOrderAmountAsync(1);

Console.WriteLine($"Average Order Amount for Customer with id 1: {averageOrderAmount}");

#endregion

#region Get All Reservations With Details

foreach (var r in await repo.GetReservationsWithDetailsAsync())
{
  Console.WriteLine($"{r.ReservationId},{r.CustomerId},{r.ReservationId}");
}

#endregion

#region Get All Employees With Details

foreach (var e in await repo.GetEmployeesWithDetailsAsync())
{
  Console.WriteLine($"{e.EmployeeId},{e.RestaurantId},{e.EmployeePosition}");
}

#endregion

#region CalculateRestaurantRevenue

try
{
  await repo.CalculateRestaurantRevenueAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

Console.WriteLine(await repo.CalculateRestaurantRevenueAsync(1));

#endregion

#region FindCustomersWithPartySizeLargerThan

foreach (var c in await repo.FindCustomersWithPartySizeLargerThanAsync(3))
{
  Console.WriteLine(c.CustomerId);
}

#endregion