var builder = WebApplication.CreateBuilder(args);

// Setup Services
var app = builder.Build();

// Map APIs
app.MapGet("/", () => "Hello World! I am deployed!");

// Start the Server
app.Run();
