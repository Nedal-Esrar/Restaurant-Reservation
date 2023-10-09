using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Enums;
using RestaurantReservation.Db.Repositories;

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
        }).AddScoped<CustomerRepository>()
        .AddScoped<EmployeeRepository>()
        .AddScoped<MenuItemRepository>()
        .AddScoped<OrderItemRepository>()
        .AddScoped<OrderRepository>()
        .AddScoped<ReservationRepository>()
        .AddScoped<RestaurantRepository>()
        .AddScoped<TableRepository>();
    });
}

var serviceProvider = CreateHostBuilder(args).Build().Services;

var customerRepository = serviceProvider.GetRequiredService<CustomerRepository>();

#region Customer Create

try
{
  await customerRepository.CreateAsync(null);
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

await customerRepository.CreateAsync(customer);

#endregion

#region Customer Update

try
{
  await customerRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await customerRepository.UpdateAsync(new Customer { CustomerId = 11111111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

customer.FirstName = "GGGGGGG";

await customerRepository.UpdateAsync(customer);

#endregion

#region Customer Delete

try
{
  await customerRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await customerRepository.DeleteAsync(customer.CustomerId);

#endregion

var employeeRepository = serviceProvider.GetRequiredService<EmployeeRepository>();

#region Employee Create

try
{
  await employeeRepository.CreateAsync(null);
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

await employeeRepository.CreateAsync(employee);

#endregion

#region Employee Update

try
{
  await employeeRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await employeeRepository.UpdateAsync(new Employee { EmployeeId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

employee.FirstName = "BG";

await employeeRepository.UpdateAsync(employee);

#endregion

#region Employee Delete

try
{
  await employeeRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await employeeRepository.DeleteAsync(employee.EmployeeId);

#endregion

var menuItemRepository = serviceProvider.GetRequiredService<MenuItemRepository>();

#region MenuItem Create

try
{
  await menuItemRepository.CreateAsync(null);
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

await menuItemRepository.CreateAsync(menuItem);

#endregion

#region MenuItem Update

try
{
  await menuItemRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await menuItemRepository.UpdateAsync(new MenuItem { ItemId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

menuItem.Name = "adfpiadhfiadfadf";

await menuItemRepository.UpdateAsync(menuItem);

#endregion

#region MenuItem Delete

try
{
  await menuItemRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await menuItemRepository.DeleteAsync(menuItem.ItemId);

#endregion

var orderRepository = serviceProvider.GetRequiredService<OrderRepository>();

#region Order Create

try
{
  await orderRepository.CreateAsync(null);
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

await orderRepository.CreateAsync(order);

#endregion

#region Order Update

try
{
  await orderRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await orderRepository.UpdateAsync(new Order { OrderId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

order.TotalAmount = 2;

await orderRepository.UpdateAsync(order);

#endregion

#region Order Delete

try
{
  await orderRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await orderRepository.DeleteAsync(order.OrderId);

#endregion

var orderItemRepository = serviceProvider.GetRequiredService<OrderItemRepository>();

#region OrderItem Create

try
{
  await orderItemRepository.CreateAsync(null);
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

await orderItemRepository.CreateAsync(orderItem);

#endregion

#region OrderItem Update

try
{
  await orderItemRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await orderItemRepository.UpdateAsync(new OrderItem { OrderItemId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

orderItem.Quantity = 314;

await orderItemRepository.UpdateAsync(orderItem);

#endregion

#region OrderItem Delete

try
{
  await orderItemRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await orderItemRepository.DeleteAsync(orderItem.OrderItemId);

#endregion

var reservationRepository = serviceProvider.GetRequiredService<ReservationRepository>();

#region Reservation Create

try
{
  await reservationRepository.CreateAsync(null);
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

await reservationRepository.CreateAsync(reservation);

#endregion

#region Reservation Update

try
{
  await reservationRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await reservationRepository.UpdateAsync(new Reservation { ReservationId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

reservation.PartySize = 6;

await reservationRepository.UpdateAsync(reservation);

#endregion

#region Reservation Delete

try
{
  await reservationRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await reservationRepository.DeleteAsync(reservation.ReservationId);

#endregion

var restaurantRepository = serviceProvider.GetRequiredService<RestaurantRepository>();

#region Restaurant Create

try
{
  await restaurantRepository.CreateAsync(null);
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

await restaurantRepository.CreateAsync(restaurant);

#endregion

#region Restaurant Update

try
{
  await restaurantRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await restaurantRepository.UpdateAsync(new Restaurant { RestaurantId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

restaurant.Name = "YOU WILL NOT EAT HERE";

await restaurantRepository.UpdateAsync(restaurant);

#endregion

#region Restaurant Delete

try
{
  await restaurantRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await restaurantRepository.DeleteAsync(restaurant.RestaurantId);

#endregion

var tableRepository = serviceProvider.GetRequiredService<TableRepository>();

#region Table Create

try
{
  await tableRepository.CreateAsync(null);
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

await tableRepository.CreateAsync(table);

#endregion

#region Table Update

try
{
  await tableRepository.UpdateAsync(null);
}
catch (ArgumentNullException e)
{
  Console.WriteLine(e.Message);
}

try
{
  await tableRepository.UpdateAsync(new Table { TableId = 1111 });
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

table.Capacity = 4;

await tableRepository.UpdateAsync(table);

#endregion

#region Table Delete

try
{
  await tableRepository.DeleteAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

await tableRepository.DeleteAsync(table.TableId);

#endregion

#region List All Managers

var managers = await employeeRepository.ListManagersAsync();

foreach (var e in managers) Console.WriteLine($"{e.EmployeeId},{e.FirstName},{e.LastName}");

#endregion

#region Get Reservations By Customer

try
{
  await reservationRepository.GetReservationsByCustomerAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var reservations = await reservationRepository.GetReservationsByCustomerAsync(1);

foreach (var r in reservations)
  Console.WriteLine(
    $"{r.ReservationId},{r.CustomerId},{r.RestaurantId},{r.TableId},{r.ReservationDate},{r.PartySize}");

#endregion

#region List Orders And MenuItems

try
{
  await orderRepository.ListOrdersAndMenuItemsAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var orders = await orderRepository.ListOrdersAndMenuItemsAsync(1);

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
  await menuItemRepository.ListOrderedMenuItemsAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var orderedItems = await menuItemRepository.ListOrderedMenuItemsAsync(1);

foreach (var orderedItem in orderedItems)
  Console.WriteLine($"{orderedItem.ItemId},{orderedItem.Name},{orderedItem.RestaurantId},{orderedItem.Price}");

#endregion

#region Calculate Average Order Amount

try
{
  await orderRepository.CalculateAverageOrderAmountAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

var averageOrderAmount = await orderRepository.CalculateAverageOrderAmountAsync(1);

Console.WriteLine($"Average Order Amount for Customer with id 1: {averageOrderAmount}");

#endregion

#region Get All Reservations With Details

foreach (var r in await reservationRepository.GetReservationsWithDetailsAsync())
  Console.WriteLine($"{r.ReservationId},{r.CustomerId},{r.ReservationId}");

#endregion

#region Get All Employees With Details

foreach (var e in await employeeRepository.GetEmployeesWithDetailsAsync())
  Console.WriteLine($"{e.EmployeeId},{e.RestaurantId},{e.EmployeePosition}");

#endregion

#region CalculateRestaurantRevenue

try
{
  await restaurantRepository.CalculateRestaurantRevenueAsync(1111);
}
catch (NotFoundException e)
{
  Console.WriteLine(e.Message);
}

Console.WriteLine(await restaurantRepository.CalculateRestaurantRevenueAsync(1));

#endregion

#region FindCustomersWithPartySizeLargerThan

foreach (var c in await customerRepository.FindCustomersWithPartySizeLargerThanAsync(3))
  Console.WriteLine(c.CustomerId);

#endregion