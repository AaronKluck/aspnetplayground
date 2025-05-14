namespace AspNetPlayground.Tests;

using AspNetPlayground.Data;


public class LinqableTests
{
    /**
     * Find all electronics products that are not discontinued, cost less than
     * $500, and return a list of product names along with their prices sorted
     * from least to most expensive.
     */
    [Fact]
    public void Basic_Filtering_and_Projection()
    {

    }

    /**
     * Find all customers who are either VIP customers OR Premium customers who
     * joined before 2021 AND have made purchases totaling more than $2000.
     * Return their names and total purchases.
     */
    [Fact]
    public void Complex_Filtering_with_Multiple_Conditions()
    {

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

    }

    /**
     * Join the products and orderItems collections to find the 3 most
     * frequently ordered products. Return the product name, total quantity
     * ordered, and total revenue generated.
     */
    [Fact]
    public void Joining_Collections()
    {

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

    }

    /**
     * Find all product categories that have at least one product under $100 AND
     * at least one product over $500. Use set operations in your approach.
     */
    [Fact]
    public void Set_Operations()
    {

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

    }
}