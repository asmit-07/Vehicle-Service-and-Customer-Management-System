using Microsoft.EntityFrameworkCore;
using VehicleServiceApp.Data;
using VehicleServiceApp.Models;
using VehicleServiceApp.Services;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VehicleServiceDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;
var context = new AppDbContext(options);

var customerService = new CustomerService(context);
var vehicleService = new VehicleService(context);
var serviceRecordService = new ServiceRecordService(context);

while (true)
{
    Console.WriteLine("\n===== Vehicle Service Management =====");
    Console.WriteLine("1. Manage Customers");
    Console.WriteLine("2. Manage Vehicles");
    Console.WriteLine("3. Manage Services");
    Console.WriteLine("4. View Pending Services");
    Console.WriteLine("5. Exit");

    Console.Write("Select Option: ");
    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            // ===================== CUSTOMERS =====================
            case "1":

                Console.WriteLine("\n--- Customer Management ---");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Update Customer");
                Console.WriteLine("3. Delete Customer");
                Console.WriteLine("4. View All Customers");
                Console.WriteLine("5. Deactivate Customer");

                Console.Write("Select Option: ");
                var customerChoice = Console.ReadLine();

                switch (customerChoice)
                {
                    case "1":
                        Console.Write("Enter Name: ");
                        var name = Console.ReadLine();

                        Console.Write("Enter Email: ");
                        var email = Console.ReadLine();

                        Console.Write("Enter Phone: ");
                        var phone = Console.ReadLine();

                        customerService.AddCustomer(name, email, phone);
                        Console.WriteLine("Customer Added Successfully!");
                        break;

                    case "2":
                        Console.Write("Enter Customer Id: ");
                        if (!int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            Console.WriteLine("Invalid Id!");
                            break;
                        }

                        var customer = context.Customers.Find(updateId);

                        if (customer == null)
                        {
                            Console.WriteLine("Customer not found!");
                            break;
                        }

                        // Only after existence check ↓

                        Console.Write("Enter New Name: ");
                        var newName = Console.ReadLine() ?? "";

                        Console.Write("Enter New Email: ");
                        var newEmail = Console.ReadLine() ?? "";

                        Console.Write("Enter New Phone: ");
                        var newPhone = Console.ReadLine() ?? "";

                        customer.Name = newName;
                        customer.Email = newEmail;
                        customer.Phone = newPhone;

                        context.SaveChanges();

                        Console.WriteLine("Customer Updated Successfully!");
                        break;

                    case "3":

                        Console.Write("Enter Customer Id to Delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            Console.WriteLine("Invalid Id!");
                            break;
                        }

                        try
                        {
                            customerService.DeleteCustomer(deleteId);
                            Console.WriteLine("Customer Deleted!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    case "4":
                        var customers = customerService.GetAllCustomers();
                        foreach (var c in customers)
                        {
                            Console.WriteLine($"{c.CustomerId} - {c.Name} - Active: {c.IsActive}");
                        }
                        break;

                    case "5":
                        Console.Write("Enter Customer Id to Deactivate: ");
                        int deactivateId = int.Parse(Console.ReadLine());
                        customerService.DeactivateCustomer(deactivateId);
                        Console.WriteLine("Customer Deactivated!");
                        break;

                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }

                break;

            // ===================== VEHICLES =====================
            case "2":

                Console.WriteLine("\n--- Vehicle Management ---");
                Console.WriteLine("1. Add Vehicle");
                Console.WriteLine("2. Update Vehicle");
                Console.WriteLine("3. Delete Vehicle");
                Console.WriteLine("4. View All Vehicles");
                Console.WriteLine("5. Search by Registration");
                Console.WriteLine("6. View Vehicles by Customer");

                Console.Write("Select Option: ");
                var vehicleChoice = Console.ReadLine();

                switch (vehicleChoice)
                {
                    case "1":
                        Console.Write("Enter Registration Number: ");
                        var reg = Console.ReadLine();

                        Console.Write("Enter Brand: ");
                        var brand = Console.ReadLine();

                        Console.Write("Enter Model: ");
                        var model = Console.ReadLine();

                        Console.Write("Enter Year: ");
                        if (!int.TryParse(Console.ReadLine(), out int year))
                        {
                            Console.WriteLine("Invalid Year!");
                            break;
                        }

                        Console.Write("Enter Customer Id: ");
                        if (!int.TryParse(Console.ReadLine(), out int custId))
                        {
                            Console.WriteLine("Invalid Customer Id!");
                            break;
                        }

                        vehicleService.AddVehicle(reg, brand, model, year, custId);
                        Console.WriteLine("Vehicle Added!");
                        break;
                    // ================= UPDATE =================
                    case "2":
                        Console.Write("Enter Vehicle Id to Update: ");

                        if (!int.TryParse(Console.ReadLine(), out int updateVehicleId))
                        {
                            Console.WriteLine("Invalid Vehicle Id!");
                            break;
                        }
                        
                        var vehicle = context.Vehicles.Find(updateVehicleId);

                        if (vehicle == null)
                        {
                            Console.WriteLine("Vehicle not found.");
                            break;
                        }
                        
                        Console.Write("Enter New Registration Number: ");
                        var newReg = Console.ReadLine() ?? "";

                        Console.Write("Enter New Brand: ");
                        var newBrand = Console.ReadLine() ?? "";

                        Console.Write("Enter New Model: ");
                        var newModel = Console.ReadLine() ?? "";

                        Console.Write("Enter New Year: ");
                        if (!int.TryParse(Console.ReadLine(), out int newYear))
                        {
                            Console.WriteLine("Invalid Year!");
                            break;
                        }
                     
                        vehicle.RegistrationNumber = newReg;
                        vehicle.Brand = newBrand;
                        vehicle.Model = newModel;
                        vehicle.Year = newYear;

                        context.SaveChanges();

                        Console.WriteLine("Vehicle updated successfully.");
                        break;


                    // ================= DELETE =================
                    case "3":

                        Console.Write("Enter Vehicle Id to Delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int deleteVehicleId))
                        {
                            Console.WriteLine("Invalid Vehicle Id!");
                            break;
                        }

                        vehicleService.DeleteVehicle(deleteVehicleId);
                        Console.WriteLine("Vehicle Deleted!");
                        break;

                    case "4":
                        var vehicles = vehicleService.GetAllVehicles();
                        foreach (var v in vehicles)
                        {
                            Console.WriteLine($"{v.VehicleId} - {v.RegistrationNumber} - {v.Brand}");
                        }
                        break;

                    case "5":
                        {
                            Console.Write("Enter Registration Number: ");
                            var searchReg = Console.ReadLine();

                            var foundvehicle = vehicleService.SearchByRegistration(searchReg ?? "");

                            if (foundvehicle != null)
                                Console.WriteLine($"{foundvehicle.VehicleId} - {foundvehicle.Brand}");
                            else
                                Console.WriteLine("Vehicle not found");

                            break;
                        }

                    case "6":
                        Console.Write("Enter Customer Id: ");
                        int cid = int.Parse(Console.ReadLine());
                        var custVehicles = vehicleService.GetVehiclesByCustomer(cid);

                        foreach (var v in custVehicles)
                            Console.WriteLine($"{v.VehicleId} - {v.RegistrationNumber}");
                        break;

                    default:
                        Console.WriteLine("Option not fully implemented");
                        break;
                }

                break;

            // ===================== SERVICES =====================
            case "3":

                Console.WriteLine("\n--- Service Management ---");
                Console.WriteLine("1. Add Service");
                Console.WriteLine("2. Mark Service Completed");
                Console.WriteLine("3. View All Services");

                Console.Write("Select Option: ");
                var serviceChoice = Console.ReadLine();

                switch (serviceChoice)
                {
                    case "1":
                        Console.Write("Enter Vehicle Id: ");
                        int vehicleId = int.Parse(Console.ReadLine());

                        Console.Write("Enter Service Type: ");
                        var type = Console.ReadLine();

                        Console.Write("Enter Cost: ");
                        decimal cost = decimal.Parse(Console.ReadLine());

                        serviceRecordService.AddService(vehicleId, type, cost);
                        Console.WriteLine("Service Added!");
                        break;

                    case "2":
                        Console.Write("Enter Service Id: ");
                        int serviceId = int.Parse(Console.ReadLine());
                        serviceRecordService.MarkServiceCompleted(serviceId);
                        Console.WriteLine("Service Marked Completed!");
                        break;

                    case "3":
                        var services = serviceRecordService.GetAllServices();
                        foreach (var s in services)
                            Console.WriteLine($"ServiceId: {s.ServiceId} - Cost: {s.Cost}");
                        break;

                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }

                break;

            // ===================== VIEW PENDING =====================
            case "4":
                var pending = serviceRecordService.GetPendingServices();
                foreach (var s in pending)
                    Console.WriteLine($"ServiceId: {s.ServiceId} - Cost: {s.Cost}");
                break;

            case "5":
                return;

            default:
                Console.WriteLine("Invalid Option");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}