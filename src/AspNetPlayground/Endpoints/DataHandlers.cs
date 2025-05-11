namespace AspNetPlayground.Endpoints;

public static class DataHandlers
{
    public static async Task<IResult> GetDataAsync()
    {
        // Simulate an async data retrieval (e.g., from a database)
        await Task.Delay(500); // Simulate I/O delay

        var data = new
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Message = "Sample data fetched asynchronously."
        };

        return Results.Json(data);
    }
}
