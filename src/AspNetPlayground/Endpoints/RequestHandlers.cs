namespace AspNetPlayground.Endpoints;

using System.Text.Json;
using System.Text;

public static class RequestHandlers
{
    public record EchoRequest(string Message);

    public static async Task<IResult> EchoRequestAsync(HttpRequest request)
    {
        try
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var requestBody = await reader.ReadToEndAsync();
            Console.WriteLine($"Request Body: {requestBody}");

            var echoRequest = JsonSerializer.Deserialize<EchoRequest>(requestBody);

            if (echoRequest == null)
                return Results.BadRequest("Invalid JSON payload.");

            var response = new
            {
                ReceivedMessage = echoRequest.Message,
                Timestamp = DateTime.UtcNow
            };

            return Results.Json(response);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error processing request: {ex.Message}");
        }
    }
}
