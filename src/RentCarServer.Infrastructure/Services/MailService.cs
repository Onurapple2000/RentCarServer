using FluentEmail.Core;
using RentCarServer.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentCarServer.Infrastructure.Services;

internal sealed class MailService(
    IFluentEmail fluentEmail) : IMailService
{
    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellation = default)
    {
       var sendResponse = await fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body, isHtml: true)
            .SendAsync(cancellation);


        if (!sendResponse.Successful) { 
            var errors = string.Join(", ", sendResponse.ErrorMessages);
            throw new Exception($"Email gönderilemedi: {errors}");
        }
    }
}
