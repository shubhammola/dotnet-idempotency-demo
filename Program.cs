var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Store processed requests (in-memory)
var requestStore = new Dictionary<string, string>();

app.MapPost("/order", (HttpRequest request) =>
{
    var key = request.Headers["Idempotency-Key"].ToString();

    if (string.IsNullOrEmpty(key))
    {
        return Results.BadRequest("Missing Idempotency-Key");
    }

    // Check duplicate
    if (requestStore.ContainsKey(key))
    {
        return Results.Ok(new
        {
            message = "Duplicate request detected",
            data = requestStore[key]
        });
    }

    // Simulate order creation
    var result = $"Order created at {DateTime.Now}";

    requestStore[key] = result;

    return Results.Ok(new
    {
        message = "Order created successfully",
        data = result
    });
});

app.Run();