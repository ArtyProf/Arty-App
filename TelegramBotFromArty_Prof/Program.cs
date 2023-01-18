var builder = WebApplication.CreateBuilder(args);

// Setup Services
var app = builder.Build();

// Map APIs
app.MapGet("/", () => "Hello World!");

// Start the Server
app.Run();
