namespace AspNetPlayground.Tests;

public class LinqableTests
{
    private static readonly LinqSampler.Sample Sample = LinqSampler.Create();

    /**
     * Find all electronics products that are not discontinued, cost less than
     * $500, and return a list of product names along with their prices sorted
     * from least to most expensive.
     */
    [Fact]
    public void Basic_Filtering_and_Projection()
    {
        var result = Sample.Products.
            Where(x => x.Category == "Electronics" && x.Price < 500m).
            Select(x => new { x.Name, x.Price }).
            OrderBy(x => x.Price).
            ToList();

        var expected = new[]
        {
            new {Name = "Fitness Tracker", Price = 129.99m},
            new {Name = "Bluetooth Headphones", Price = 149.99m},
            new {Name = "Vintage Phone", Price = 299.99m},
            new {Name = "Gaming Console", Price = 499.99m},
        };
        Assert.Equal(
            expected.Select(e => (e.Name, e.Price)),
            result.Select(r => (r.Name, r.Price))
        );
    }

    /**
     * Find all customers who are either VIP customers OR Premium customers who
     * joined before 2021 AND have made purchases totaling more than $2000.
     * Return their names and total purchases.
     */
    [Fact]
    public void Complex_Filtering_with_Multiple_Conditions()
    {
        var result = Sample.Customers.
            Where(
                x => (x.Type == CustomerType.Premium || x.Type == CustomerType.VIP) &&
                x.JoinDate < new DateTime(2021, 1, 1) &&
                x.TotalPurchases > 2000m
            ).
            Select(x => new { x.Name, x.TotalPurchases }).
            ToList();

        var expected = new[]
        {
            new {Name = "Olivia Brown", TotalPurchases = 8750m},
            new {Name = "James Taylor", TotalPurchases = 12500.75m},
        };
        Assert.Equal(
            expected.Select(e => (e.Name, e.TotalPurchases)),
            result.Select(r => (r.Name, r.TotalPurchases))
        );
    }

    /**
     * Group all products by manufacturer and for each manufacturer, calculate:
     * - The number of products they make
     * - The average price of their products
     * - The total inventory value (price × stockQuantity)
     * - Sort the results by total inventory value (highest first).
     */
    [Fact]
    public void Grouping_and_Aggregation()
    {
        var result = Sample.Products.
            GroupBy(x => x.Manufacturer).
            Select(g => new
            {
                Manufacturer = g.Key,
                TotalProducts = g.Count(),
                AveragePrice = Math.Round(g.Average(p => p.Price), 2),
                InventoryValue = g.Sum(p => p.Price * p.StockQuantity),
            }
            ).
            OrderByDescending(x => x.InventoryValue).
            ToList();



        var expected = new[]
        {
            new {Manufacturer = "TechCorp", TotalProducts = 2, AveragePrice = 1049.99m, InventoryValue = 154498.35m},
            new {Manufacturer = "GameTech", TotalProducts = 1, AveragePrice = 499.99m, InventoryValue = 29999.40m},
            new {Manufacturer = "AudioTech", TotalProducts = 1, AveragePrice = 149.99m, InventoryValue = 29998.00m},
            new {Manufacturer = "FitGear", TotalProducts = 1, AveragePrice = 129.99m, InventoryValue = 9749.25m},
            new {Manufacturer = "HomeGoods", TotalProducts = 3, AveragePrice = 253.32m, InventoryValue = 9049.55m},
            new {Manufacturer = "ComfortLiving", TotalProducts = 1, AveragePrice = 199.99m, InventoryValue = 2999.85m},
            new {Manufacturer = "RetroTech", TotalProducts = 1, AveragePrice = 299.99m, InventoryValue = 0.00m},
        };
        Assert.Equal(
            expected.Select(e => (e.Manufacturer, e.TotalProducts, e.AveragePrice, e.InventoryValue)),
            result.Select(r => (r.Manufacturer, r.TotalProducts, r.AveragePrice, r.InventoryValue))
        );
    }

    /**
     * Join the products and orderItems collections to find the 3 most
     * frequently ordered products (breaking ties in order count by total
     * quantity). Return the product name, total quantity ordered, and total
     * revenue generated.
     */
    [Fact]
    public void Joining_Collections()
    {
        var result = Sample.Products.
            GroupJoin(Sample.OrderItems, o => o.Id, i => i.ProductId, (p, oi) => new
            {
                p.Name,
                TotalOrders = oi.Count(),
                TotalQuantity = oi.Sum(x => x.Quantity),
                TotalRevenue = oi.Sum(x => (x.UnitPrice - x.Discount) * x.Quantity)
            }
            ).
            OrderByDescending(x => x.TotalOrders).
            ThenByDescending(x => x.TotalQuantity).
            ToList();


        var expected = new[]
        {
            new {Name = "Bluetooth Headphones", TotalOrders = 4, TotalQuantity = 20, TotalRevenue = 2999.8m},
            new {Name = "Coffee Maker", TotalOrders = 3, TotalQuantity = 8, TotalRevenue = 719.92m},
            new {Name = "Fitness Tracker", TotalOrders = 2, TotalQuantity = 12, TotalRevenue = 1559.88m},
        };
        Assert.Equal(
            expected.Select(e => (e.Name, e.TotalOrders, e.TotalQuantity, e.TotalRevenue)),
            result.Select(r => (r.Name, r.TotalOrders, r.TotalQuantity, r.TotalRevenue))
        );
    }

    /**
     * For each customer who has placed at least 2 orders, find their most
     * expensive order (by TotalAmount). Return the customer name, the order ID,
     * the order amount, and the order date, sorted by order amount (highest
     * first).
     */
    [Fact]
    public void Nested_Collection_Processing()
    {
        var result = Sample.Customers.
            Join(Sample.Orders, c => c.Id, o => o.CustomerId, (c, o) => new
            {
                c.Name,
                o.CustomerId,
                OrderId = o.Id,
                OrderAmount = o.TotalAmount,
                o.OrderDate

            }
            ).
            GroupBy(x => x.CustomerId).
            Where(g => g.Count() > 1).
            Select(g => new
            {
                OrderCount = g.Count(),
                MaxOrder = g.MaxBy(x => x.OrderAmount)
            }
            ).
            Select(m => new
            {
                m.MaxOrder!.Name,
                m.MaxOrder!.CustomerId,
                m.OrderCount,
                m.MaxOrder!.OrderId,
                m.MaxOrder!.OrderAmount,
                m.MaxOrder!.OrderDate,
            }).
            ToList();

        var expected = new[]
        {
            new {Name = "John Smith", CustomerId = 1, OrderId = 5, OrderAmount = 499.99m, OrderDate = new DateTime(2023, 2, 3)},
            new {Name = "Emma Johnson", CustomerId = 2, OrderId = 2, OrderAmount = 1299.99m, OrderDate = new DateTime(2023, 1, 8)},
            new {Name = "Liam Wilson", CustomerId = 3, OrderId = 13, OrderAmount = 299.99m, OrderDate = new DateTime(2023, 3, 15)},
            new {Name = "Olivia Brown", CustomerId = 4, OrderId = 9, OrderAmount = 1099.98m, OrderDate = new DateTime(2023, 3, 2)},
        };
        Assert.Equal(
            expected.Select(e => (e.Name, e.CustomerId, e.OrderId, e.OrderAmount, e.OrderDate)),
            result.Select(r => (r.Name, r.CustomerId, r.OrderId, r.OrderAmount, r.OrderDate))
        );
    }

    /**
     * Find all product categories that have at least one product under $100 AND
     * at least one product over $500. Use set operations in your approach.
     */
    [Fact]
    public void Set_Operations()
    {
        var lessResult = Sample.Products.
            Where(x => x.Price < 100m).
            GroupBy(x => x.Category).
            Select(g => g.Key);
        var moreResult = Sample.Products.
            Where(x => x.Price > 500m).
            GroupBy(x => x.Category).
            Select(g => g.Key);
        var result = lessResult.Intersect(moreResult).ToList();

        var expected = new[] { "Appliances" };
        Assert.Equal(expected, result);
    }

    /**
     * Create a sales report for the month of February 2023 showing for each order:
     * - The order ID
     * - Customer name
     * - Order status
     * - A list of product names included in the order
     * - Total order amount after discounts
     * - Sort by order date.
     */
    [Fact]
    public void Advanced_Projection_with_Anonymous_Types()
    {
        var orderResult = Sample.Orders.
            Where(x => x.OrderDate >= new DateTime(2023, 2, 1) && x.OrderDate < new DateTime(2023, 3, 1)).
            Join(Sample.Customers, o => o.CustomerId, c => c.Id, (o, c) => new
            { CustomerName = c.Name, Order = o }
            );

        var itemResult = Sample.OrderItems.
            Join(Sample.Products, i => i.ProductId, p => p.Id, (i, p) => new
            {
                ProductName = p.Name,
                i.OrderId,
                NetAmount = (i.UnitPrice - i.Discount) * i.Quantity
            }
            );

        var result = orderResult.
            GroupJoin(itemResult, o => o.Order.Id, i => i.OrderId, (o, i) => new
            {
                OrderId = o.Order.Id,
                o.CustomerName,
                o.Order.Status,
                ProductNames = i.Select(x => x.ProductName).ToList(),
                TotalAmount = i.Sum(x => x.NetAmount),
                o.Order.OrderDate,
            }
        ).
        OrderBy(x => x.OrderDate).
        ToList();

        var expected = new[]
        {
            new {OrderId = 5, CustomerName = "John Smith", Status=OrderStatus.Delivered, ProductNames=new List<string>{"Gaming Console"}, TotalAmount = 1999.96m, OrderDate = new DateTime(2023, 2, 3)},
            new {OrderId = 6, CustomerName = "Noah Davis", Status=OrderStatus.Processing, ProductNames=new List<string>{"Coffee Maker"}, TotalAmount = 449.95m, OrderDate = new DateTime(2023, 2, 10)},
            new {OrderId = 7, CustomerName = "Emma Johnson", Status=OrderStatus.Shipped, ProductNames=new List<string>{"Fitness Tracker"}, TotalAmount = 779.94m, OrderDate = new DateTime(2023, 2, 15)},
            new {OrderId = 8, CustomerName = "Sophia Martinez", Status=OrderStatus.Processing, ProductNames=new List<string>{"Bluetooth Headphones", "Coffee Maker", "Blender"}, TotalAmount = 1199.92m, OrderDate = new DateTime(2023, 2, 20)},
        };
        Assert.Equal(
            expected.Select(e => (e.OrderId, e.CustomerName, e.Status, e.ProductNames, e.TotalAmount, e.OrderDate)),
            result.Select(r => (r.OrderId, r.CustomerName, r.Status, r.ProductNames, r.TotalAmount, r.OrderDate))
        );

    }

    /*
     * Group employees by department, then for each department show:
     * - Department name
     * - Number of employees
     * - Average salary
     * - Most senior employee (based on hire date)
     * - A list of all distinct skills that the department employees possess
     *   (combined), with their maximum proficiency level
     * - Sort departments by average salary (highest first).
     */
    [Fact]
    public void Advaned_Grouping_with_Nested_Results()
    {
        var result = Sample.Employees.
            GroupBy(x => x.Department).
            Select(g => new
            {
                Department = g.Key,
                EmployeeCount = g.Count(),
                AverageSalary = Math.Round(g.Average(x => x.Salary), 2),
                MostSeniorEmployee = g.MinBy(x => x.HireDate)!.Name,
                Skills = g.SelectMany(x => x.Skills).Select(x => x.Name).Distinct().Order().ToList(),
            }).
            OrderByDescending(g => g.AverageSalary).
            ToList();


        var expected = new[]
        {
            new { Department = "Management", EmployeeCount = 1, AverageSalary = 75000m, MostSeniorEmployee = "Michael Scott", Skills = new List<string>{"Communication", "Leadership", "Presentation"}},
            new { Department = "Sales", EmployeeCount = 3, AverageSalary = 53333.33m, MostSeniorEmployee = "Dwight Schrute", Skills = new List<string>{"Communication", "Customer Service", "Office Management", "Presentation", "Sales"}},
            new { Department = "Accounting", EmployeeCount = 3, AverageSalary = 50500m, MostSeniorEmployee = "Oscar Martinez", Skills = new List<string>{"Accounting", "Communication", "Excel"}},
            new { Department = "Reception", EmployeeCount = 1, AverageSalary = 45000m, MostSeniorEmployee = "Pam Beesly", Skills = new List<string>{"Communication", "Customer Service", "Office Management"}},
        };
        Assert.Equal(
            expected.Select(e => (e.Department, e.EmployeeCount, e.AverageSalary, e.MostSeniorEmployee, e.Skills)),
            result.Select(r => (r.Department, r.EmployeeCount, r.AverageSalary, r.MostSeniorEmployee, r.Skills))
        );
    }

    /**
     * Implement a function that returns a page of products. The function should
     * accept:
     * - page number (1-based)
     * - page size
     * - sort field name
     * - sort direction (ascending/descending)
     * - Test it by getting the 2nd page (with page size 3) of products sorted
     *   by price in descending order.
     */
    [Fact]
    public void Pagination_and_Sorting()
    {
        var result = LinqSampler.GetProductPage(Sample.Products, 2, 3, x => x.Price, true);

        var expected = new List<Product>
        {
            Sample.Products[7], Sample.Products[9], Sample.Products[6]
        };
        Assert.Equal(expected, result);
    }

    /**
     * Calculate the potential revenue lost due to discontinued products. For
     * each discontinued product, assume you could have sold half of the current
     * average monthly sales (based on all orders in the first quarter of 2023)
     * if the product were still available. Return the total potential lost
     * revenue across all discontinued products.
     */
    [Fact]
    public void Custom_Aggregation_and_Reduction()
    {
        var result = Sample.Products.
            Where(x => x.IsDiscontinued).
            GroupJoin(Sample.OrderItems, p => p.Id, i => i.ProductId, (p, i) => new
            {
                p.Name,
                PotentialLoss = Math.Round(
                    i.Sum(x => (x.UnitPrice - x.Discount) * x.Quantity) / 6, 2
                ),
            }).
            ToList();

        var expected = new[]
        {
            new { Name= "Blender", PotentialLoss = 10m},
            new { Name= "Vintage Phone", PotentialLoss = 449.98m}
        };
        Assert.Equal(
            expected.Select(e => (e.Name, e.PotentialLoss)),
            result.Select(r => (r.Name, r.PotentialLoss))
        );
    }
}