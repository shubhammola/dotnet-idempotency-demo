# .NET Idempotency Key Demo

## Overview

This project demonstrates how to prevent duplicate operations in an ASP.NET Core API using an idempotency key.

In real-world systems such as payments, bookings, and order processing, the same request can be triggered multiple times due to retries, network instability, or repeated user actions. Without proper safeguards, this can lead to duplicate data and inconsistent system behavior.

This implementation ensures that repeated requests with the same idempotency key are handled safely by returning the original response instead of processing the operation again.

---

## Problem Statement

In distributed systems, clients may unintentionally send the same request multiple times. Common causes include:

- Network retries
- Double-click actions
- Timeout handling in clients
- Page refreshes during submission

If the backend processes each request independently, it can result in:

- Duplicate orders
- Multiple payments
- Data inconsistency

---

## Solution Approach

The API uses an idempotency key provided in the request headers to uniquely identify each operation.

Example header:
```Idempotency-Key: 123```

The server performs the following steps:

1. Checks if the idempotency key exists in the request store
2. If it exists, returns the previously stored response
3. If it does not exist, processes the request and stores the result
4. Ensures that repeated requests produce the same outcome

---

## Implementation Details

The application is built using ASP.NET Core Minimal API.

An in-memory dictionary is used to store processed requests:

```csharp
var requestStore = new Dictionary<string, string>();

Core logic for handling duplicate requests:

if (requestStore.ContainsKey(key))
{
    return Results.Ok(new
    {
        message = "Duplicate request detected",
        data = requestStore[key]
    });
}
```

---

## How to Run the Application

1.	Clone the repository:
```
git clone https://github.com/YOUR_USERNAME/dotnet-idempotency-demo.git
cd dotnet-idempotency-demo
```
2.	Run the application:
```dotnet run```
3.	The API will be available at:
```http://localhost:5000/order```

---

## How to Test

You can test the API using Postman or curl.

Step 1: Send the first request  
- Method: POST  
- URL: http://localhost:5000/order  
- Header: Idempotency-Key: 123  

Expected result:  
The API processes the request and returns a success response indicating that the order has been created.

Step 2: Send the same request again with the same key  
- Method: POST  
- URL: http://localhost:5000/order  
- Header: Idempotency-Key: 123  

Expected result:  
The API detects the duplicate request and returns the previously stored response instead of creating a new order.

---

## Important Note

This project uses in-memory storage for simplicity.

var requestStore = new Dictionary<string, string>();

In production systems, this should be replaced with a persistent or distributed storage mechanism such as:

- Redis  
- Database (SQL/NoSQL)  
- Distributed cache  

This ensures consistency across multiple instances of the application.

---

## Use Cases

This pattern is commonly used in:

- Payment processing systems  
- E-commerce order placement  
- Ticket booking platforms  
- Financial transactions  
- API retry handling  

---

## Tech Stack

- .NET 8  
- ASP.NET Core Minimal API  

---

## Conclusion

Idempotency is a critical concept for building reliable APIs. By ensuring that duplicate requests do not result in repeated operations, systems can maintain consistency and prevent costly errors.

This project provides a simple and practical demonstration of how idempotency can be implemented in a backend service.
