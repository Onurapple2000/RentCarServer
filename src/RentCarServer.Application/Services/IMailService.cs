using System;
using System.Collections.Generic;
using System.Text;

namespace RentCarServer.Application.Services;

public interface IMailService
{

    Task SendAsync(string to, string subject, string body, CancellationToken cancellation = default);
}
