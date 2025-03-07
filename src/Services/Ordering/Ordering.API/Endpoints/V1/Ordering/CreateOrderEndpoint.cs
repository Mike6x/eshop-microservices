﻿using Ordering.Application.Orders.V1;

namespace Ordering.API.Endpoints.V1.Ordering;

//- Accepts a CreateOrderRequest object.
//- Maps the request to a CreateOrderCommand.
//- Uses MediatR to send the command to the corresponding handler.
//- Returns a response with the created order's ID.

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);

public static class CreateOrderEndpoint
{
    internal static RouteHandlerBuilder MapCreateOrderEndpoint(this IEndpointRouteBuilder endpoint)
    {
        return endpoint
            .MapPost("/", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/orders/{response.Id}", response);
            })
            .WithName(nameof(CreateOrderEndpoint))
            .WithSummary("Create Order")
            .WithDescription("Create Order")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1);
    }
}