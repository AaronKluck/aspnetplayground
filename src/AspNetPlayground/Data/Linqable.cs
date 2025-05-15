using System.Linq.Expressions;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public decimal Price { get; set; }
    public required string Manufacturer { get; set; }
    public bool IsDiscontinued { get; set; }
    public int StockQuantity { get; set; }
    public DateTime LaunchDate { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public CustomerType Type { get; set; }
    public DateTime JoinDate { get; set; }
    public required string Country { get; set; }
    public decimal TotalPurchases { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();
}

public enum CustomerType
{
    Regular,
    Premium,
    VIP
}

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public required string ShippingAddress { get; set; }
    public int? DiscountApplied { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
}

public class Employee
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Department { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public int? ManagerId { get; set; }
    public List<Skill> Skills { get; set; } = new List<Skill>();
}

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int ProficiencyLevel { get; set; } // 1-5
}

public class LinqSampler
{
    public class Sample
    {
        public List<Product> Products { get; }
        public List<Customer> Customers { get; }
        public List<Order> Orders { get; }
        public List<OrderItem> OrderItems { get; }
        public List<Employee> Employees { get; }
        public List<Skill> Skills { get; }

        public Sample(List<Product> products, List<Customer> customers, List<Order> orders, List<OrderItem> orderItems, List<Employee> employees, List<Skill> skills)
        {
            Products = products;
            Customers = customers;
            Orders = orders;
            OrderItems = orderItems;
            Employees = employees;
            Skills = skills;
        }
    }

    public static Sample Create()
    {
        // Create sample products
        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop Pro", Category = "Electronics", Price = 1299.99m, Manufacturer = "TechCorp", IsDiscontinued = false, StockQuantity = 45, LaunchDate = new DateTime(2022, 5, 15) },
            new Product { Id = 2, Name = "Smartphone X", Category = "Electronics", Price = 799.99m, Manufacturer = "TechCorp", IsDiscontinued = false, StockQuantity = 120, LaunchDate = new DateTime(2023, 1, 10) },
            new Product { Id = 3, Name = "Bluetooth Headphones", Category = "Electronics", Price = 149.99m, Manufacturer = "AudioTech", IsDiscontinued = false, StockQuantity = 200, LaunchDate = new DateTime(2022, 10, 5) },
            new Product { Id = 4, Name = "Coffee Maker", Category = "Appliances", Price = 89.99m, Manufacturer = "HomeGoods", IsDiscontinued = false, StockQuantity = 30, LaunchDate = new DateTime(2021, 6, 20) },
            new Product { Id = 5, Name = "Fitness Tracker", Category = "Electronics", Price = 129.99m, Manufacturer = "FitGear", IsDiscontinued = false, StockQuantity = 75, LaunchDate = new DateTime(2023, 3, 15) },
            new Product { Id = 6, Name = "Blender", Category = "Appliances", Price = 69.99m, Manufacturer = "HomeGoods", IsDiscontinued = true, StockQuantity = 5, LaunchDate = new DateTime(2020, 2, 10) },
            new Product { Id = 7, Name = "Desk Chair", Category = "Furniture", Price = 199.99m, Manufacturer = "ComfortLiving", IsDiscontinued = false, StockQuantity = 15, LaunchDate = new DateTime(2022, 8, 12) },
            new Product { Id = 8, Name = "Gaming Console", Category = "Electronics", Price = 499.99m, Manufacturer = "GameTech", IsDiscontinued = false, StockQuantity = 60, LaunchDate = new DateTime(2021, 11, 15) },
            new Product { Id = 9, Name = "Washing Machine", Category = "Appliances", Price = 599.99m, Manufacturer = "HomeGoods", IsDiscontinued = false, StockQuantity = 10, LaunchDate = new DateTime(2022, 4, 30) },
            new Product { Id = 10, Name = "Vintage Phone", Category = "Electronics", Price = 299.99m, Manufacturer = "RetroTech", IsDiscontinued = true, StockQuantity = 0, LaunchDate = new DateTime(2019, 7, 8) }
        };

        // Create sample customers
        List<Customer> customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John Smith", Email = "john.smith@example.com", Type = CustomerType.Regular, JoinDate = new DateTime(2021, 3, 15), Country = "USA", TotalPurchases = 750.50m },
            new Customer { Id = 2, Name = "Emma Johnson", Email = "emma.j@example.com", Type = CustomerType.Premium, JoinDate = new DateTime(2020, 11, 8), Country = "Canada", TotalPurchases = 200.75m },
            new Customer { Id = 3, Name = "Liam Wilson", Email = "liam@example.com", Type = CustomerType.Regular, JoinDate = new DateTime(2022, 1, 20), Country = "USA", TotalPurchases = 450.25m },
            new Customer { Id = 4, Name = "Olivia Brown", Email = "olivia.brown@example.com", Type = CustomerType.VIP, JoinDate = new DateTime(2019, 6, 5), Country = "UK", TotalPurchases = 8750.00m },
            new Customer { Id = 5, Name = "Noah Davis", Email = "noah.d@example.com", Type = CustomerType.Regular, JoinDate = new DateTime(2022, 5, 12), Country = "Australia", TotalPurchases = 325.50m },
            new Customer { Id = 6, Name = "Sophia Martinez", Email = "sophia@example.com", Type = CustomerType.Premium, JoinDate = new DateTime(2022, 9, 30), Country = "USA", TotalPurchases = 2150.25m },
            new Customer { Id = 7, Name = "James Taylor", Email = "james.t@example.com", Type = CustomerType.VIP, JoinDate = new DateTime(2018, 12, 15), Country = "Canada", TotalPurchases = 12500.75m },
            new Customer { Id = 8, Name = "Isabella Anderson", Email = "isabella@example.com", Type = CustomerType.Regular, JoinDate = new DateTime(2022, 2, 28), Country = "USA", TotalPurchases = 175.25m }
        };

        // Create sample orders
        List<Order> orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 1, OrderDate = new DateTime(2023, 1, 5), TotalAmount = 249.98m, Status = OrderStatus.Delivered, ShippingAddress = "123 Main St, Anytown", DiscountApplied = 0 },
            new Order { Id = 2, CustomerId = 2, OrderDate = new DateTime(2023, 1, 8), TotalAmount = 1299.99m, Status = OrderStatus.Delivered, ShippingAddress = "456 Oak Ave, Somewhere", DiscountApplied = 10 },
            new Order { Id = 3, CustomerId = 3, OrderDate = new DateTime(2023, 1, 12), TotalAmount = 149.99m, Status = OrderStatus.Shipped, ShippingAddress = "789 Pine Rd, Elsewhere", DiscountApplied = 0 },
            new Order { Id = 4, CustomerId = 4, OrderDate = new DateTime(2023, 1, 15), TotalAmount = 699.98m, Status = OrderStatus.Delivered, ShippingAddress = "101 Queen St, London", DiscountApplied = 15 },
            new Order { Id = 6, CustomerId = 5, OrderDate = new DateTime(2023, 2, 10), TotalAmount = 89.99m, Status = OrderStatus.Processing, ShippingAddress = "202 Beach Rd, Sydney", DiscountApplied = 0 },
            new Order { Id = 5, CustomerId = 1, OrderDate = new DateTime(2023, 2, 3), TotalAmount = 499.99m, Status = OrderStatus.Delivered, ShippingAddress = "123 Main St, Anytown", DiscountApplied = 0 },
            new Order { Id = 8, CustomerId = 6, OrderDate = new DateTime(2023, 2, 20), TotalAmount = 269.98m, Status = OrderStatus.Processing, ShippingAddress = "303 Maple Dr, Anystate", DiscountApplied = 5 },
            new Order { Id = 7, CustomerId = 2, OrderDate = new DateTime(2023, 2, 15), TotalAmount = 129.99m, Status = OrderStatus.Shipped, ShippingAddress = "456 Oak Ave, Somewhere", DiscountApplied = 0 },
            new Order { Id = 9, CustomerId = 4, OrderDate = new DateTime(2023, 3, 2), TotalAmount = 1099.98m, Status = OrderStatus.Pending, ShippingAddress = "101 Queen St, London", DiscountApplied = 20 },
            new Order { Id = 10, CustomerId = 7, OrderDate = new DateTime(2023, 3, 5), TotalAmount = 599.99m, Status = OrderStatus.Processing, ShippingAddress = "505 River Rd, Toronto", DiscountApplied = 10 },
            new Order { Id = 11, CustomerId = 1, OrderDate = new DateTime(2023, 3, 10), TotalAmount = 235.98m, Status = OrderStatus.Pending, ShippingAddress = "123 Main St, Anytown", DiscountApplied = 0 },
            new Order { Id = 12, CustomerId = 8, OrderDate = new DateTime(2023, 3, 12), TotalAmount = 149.99m, Status = OrderStatus.Pending, ShippingAddress = "606 Lake Ave, Statesville", DiscountApplied = 0 },
            new Order { Id = 13, CustomerId = 3, OrderDate = new DateTime(2023, 3, 15), TotalAmount = 299.99m, Status = OrderStatus.Cancelled, ShippingAddress = "789 Pine Rd, Elsewhere", DiscountApplied = 0 }
        };

        // Create sample order items
        List<OrderItem> orderItems = new List<OrderItem>
        {
            new OrderItem { Id = 1, OrderId = 1, ProductId = 3, Quantity = 3, UnitPrice = 149.99m, Discount = 0 },
            new OrderItem { Id = 2, OrderId = 1, ProductId = 4, Quantity = 2, UnitPrice = 89.99m, Discount = 0 },
            new OrderItem { Id = 3, OrderId = 2, ProductId = 1, Quantity = 1, UnitPrice = 1299.99m, Discount = 0 },
            new OrderItem { Id = 4, OrderId = 3, ProductId = 3, Quantity = 2, UnitPrice = 149.99m, Discount = 0 },
            new OrderItem { Id = 5, OrderId = 4, ProductId = 2, Quantity = 3, UnitPrice = 799.99m, Discount = 100.01m },
            new OrderItem { Id = 6, OrderId = 5, ProductId = 8, Quantity = 4, UnitPrice = 499.99m, Discount = 0 },
            new OrderItem { Id = 7, OrderId = 6, ProductId = 4, Quantity = 5, UnitPrice = 89.99m, Discount = 0 },
            new OrderItem { Id = 8, OrderId = 7, ProductId = 5, Quantity = 6, UnitPrice = 129.99m, Discount = 0 },
            new OrderItem { Id = 9, OrderId = 8, ProductId = 3, Quantity = 7, UnitPrice = 149.99m, Discount = 0 },
            new OrderItem { Id = 10, OrderId = 8, ProductId = 4, Quantity = 1, UnitPrice = 89.99m, Discount = 0 },
            new OrderItem { Id = 11, OrderId = 8, ProductId = 6, Quantity = 2, UnitPrice = 69.99m, Discount = 39.99m },
            new OrderItem { Id = 12, OrderId = 9, ProductId = 9, Quantity = 3, UnitPrice = 599.99m, Discount = 0 },
            new OrderItem { Id = 13, OrderId = 9, ProductId = 1, Quantity = 4, UnitPrice = 1299.99m, Discount = 800m },
            new OrderItem { Id = 14, OrderId = 10, ProductId = 9, Quantity = 5, UnitPrice = 599.99m, Discount = 0 },
            new OrderItem { Id = 15, OrderId = 11, ProductId = 5, Quantity = 6, UnitPrice = 129.99m, Discount = 0 },
            new OrderItem { Id = 16, OrderId = 11, ProductId = 7, Quantity = 7, UnitPrice = 199.99m, Discount = 94m },
            new OrderItem { Id = 17, OrderId = 12, ProductId = 3, Quantity = 8, UnitPrice = 149.99m, Discount = 0 },
            new OrderItem { Id = 18, OrderId = 13, ProductId = 10, Quantity = 9, UnitPrice = 299.99m, Discount = 0 }
        };

        // Link orders to customers
        foreach (var order in orders)
        {
            var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);
            if (customer != null)
            {
                customer.Orders.Add(order);
            }
        }

        // Link order items to orders
        foreach (var item in orderItems)
        {
            var order = orders.FirstOrDefault(o => o.Id == item.OrderId);
            if (order != null)
            {
                order.Items.Add(item);
            }
        }

        // Create sample employees
        List<Employee> employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Michael Scott", Department = "Management", Salary = 75000m, HireDate = new DateTime(2010, 5, 15), ManagerId = null },
            new Employee { Id = 2, Name = "Jim Halpert", Department = "Sales", Salary = 55000m, HireDate = new DateTime(2013, 9, 20), ManagerId = 1 },
            new Employee { Id = 3, Name = "Pam Beesly", Department = "Reception", Salary = 45000m, HireDate = new DateTime(2014, 3, 10), ManagerId = 1 },
            new Employee { Id = 4, Name = "Dwight Schrute", Department = "Sales", Salary = 52000m, HireDate = new DateTime(2012, 7, 8), ManagerId = 1 },
            new Employee { Id = 5, Name = "Angela Martin", Department = "Accounting", Salary = 51000m, HireDate = new DateTime(2014, 11, 16), ManagerId = 1 },
            new Employee { Id = 6, Name = "Kevin Malone", Department = "Accounting", Salary = 48000m, HireDate = new DateTime(2015, 2, 25), ManagerId = 5 },
            new Employee { Id = 7, Name = "Oscar Martinez", Department = "Accounting", Salary = 52500m, HireDate = new DateTime(2013, 6, 14), ManagerId = 5 },
            new Employee { Id = 8, Name = "Stanley Hudson", Department = "Sales", Salary = 53000m, HireDate = new DateTime(2012, 12, 3), ManagerId = 1 }
        };

        // Create sample skills
        List<Skill> skills = new List<Skill>
        {
            new Skill { Id = 1, Name = "Leadership", ProficiencyLevel = 4 },
            new Skill { Id = 2, Name = "Communication", ProficiencyLevel = 3 },
            new Skill { Id = 3, Name = "Excel", ProficiencyLevel = 5 },
            new Skill { Id = 4, Name = "Presentation", ProficiencyLevel = 2 },
            new Skill { Id = 5, Name = "Sales", ProficiencyLevel = 4 },
            new Skill { Id = 6, Name = "Customer Service", ProficiencyLevel = 5 },
            new Skill { Id = 7, Name = "Accounting", ProficiencyLevel = 4 },
            new Skill { Id = 8, Name = "Office Management", ProficiencyLevel = 3 }
        };

        // Assign skills to employees
        employees[0].Skills.AddRange(new List<Skill> { skills[0], skills[1], skills[3] }); // Michael
        employees[1].Skills.AddRange(new List<Skill> { skills[1], skills[4], skills[5] }); // Jim
        employees[2].Skills.AddRange(new List<Skill> { skills[1], skills[5], skills[7] }); // Pam
        employees[3].Skills.AddRange(new List<Skill> { skills[3], skills[4], skills[7] }); // Dwight
        employees[4].Skills.AddRange(new List<Skill> { skills[2], skills[6] });           // Angela
        employees[5].Skills.AddRange(new List<Skill> { skills[2], skills[6] });           // Kevin
        employees[6].Skills.AddRange(new List<Skill> { skills[2], skills[6], skills[1] }); // Oscar
        employees[7].Skills.AddRange(new List<Skill> { skills[4], skills[1] });           // Stanley

        return new Sample(
            products,
            customers,
            orders,
            orderItems,
            employees,
            skills
        );
    }

    public static List<Product> GetProductPage<T>(
        List<Product> products,
        int pageNum,
        int pageSize,
        Expression<Func<Product, T>> sortExpression,
        bool descending = false
    )
    {
        if (pageNum <= 0) throw new ArgumentException("pages are 1-based");
        var queryable = products.AsQueryable();
        var sorted = descending ?
            queryable.OrderByDescending(sortExpression) :
            queryable.OrderBy(sortExpression);
        return sorted.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();
    }
}
