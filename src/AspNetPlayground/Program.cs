using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using AspNetPlayground.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// New Endpoints
app.MapGet("/data", DataHandlers.GetDataAsync);
app.MapGet("/file", FileHandlers.ReadFileAsync);
app.MapPost("/echo", RequestHandlers.EchoRequestAsync);

app.Run();
