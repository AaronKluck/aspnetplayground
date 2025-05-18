namespace AspNetPlayground.Endpoints;

/**
 * An interface for the sake of making an async interface.
 */
interface IFileHandler
{
    Task<IResult> ReadFileAsync(string filePath);
}

public class FileHandlerInternal : IFileHandler
{
    public async Task<IResult> ReadFileAsync(string filePath)
    {
        Console.WriteLine($"{Environment.CurrentManagedThreadId}: /file");
        if (!File.Exists(filePath))
        {
            return Results.NotFound($"{Environment.CurrentManagedThreadId}: File not found.");
        }

        string content;
        try
        {
            content = await File.ReadAllTextAsync(filePath);
        }
        catch (Exception ex)
        {
            return Results.Problem($"{Environment.CurrentManagedThreadId}: Error reading file: {ex.Message}");
        }

        return Results.Text($"{Environment.CurrentManagedThreadId}: {content}");
    }
}

public static class FileHandlers
{
    private static readonly IFileHandler @internal = new FileHandlerInternal();

    public static async Task<IResult> ReadFileAsync()
    {
        string filePath = "sample.txt";

        return await @internal.ReadFileAsync(filePath);
    }
}
