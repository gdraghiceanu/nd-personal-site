using Microsoft.AspNetCore.Mvc;
using PersonalSite.Api.Models;
using PersonalSite.Api.Services;

namespace PersonalSite.Api.Endpoints;

public static class ContactEndpoints
{
    public static IEndpointRouteBuilder MapContactEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/contact")
            .WithTags("Contact");

        group.MapPost("/", async (
            [FromBody] ContactRequest request,
            IEmailService emailService,
            CancellationToken ct) =>
        {
            var sent = await emailService.SendContactEmailAsync(request, ct);

            return sent
                ? Results.Ok(new { message = "Your message has been sent." })
                : Results.Problem("Failed to send email. Please try again later.");
        })
        .WithName("SendContactMessage")
        .Produces<object>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}
