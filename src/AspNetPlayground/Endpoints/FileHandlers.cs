namespace AspNetPlayground.Endpoints;

using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public static class FileHandlers
{
    public static async Task<IResult> ReadFileAsync()
    {
        string filePath = "sample.txt";

        if (!File.Exists(filePath))
        {
            return Results.NotFound("File not found.");
        }

        string content;
        try
        {
            content = await File.ReadAllTextAsync(filePath);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error reading file: {ex.Message}");
        }

        return Results.Text(content);
    }
}
