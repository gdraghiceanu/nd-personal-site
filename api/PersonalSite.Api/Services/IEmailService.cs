using PersonalSite.Api.Models;

namespace PersonalSite.Api.Services;

public interface IEmailService
{
    Task<bool> SendContactEmailAsync(ContactRequest request, CancellationToken ct = default);
}
