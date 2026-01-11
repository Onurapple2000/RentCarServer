using System;
using System.Collections.Generic;
using System.Text;

namespace RentCarServer.Infrastructure.Options;

internal sealed class MailSettingOption
{
    public string Email { get; set; } = default!;
    public string Smtp { get; set; } = default!;
    public int Port { get; set; }
    public bool Ssl { get; set; }
    public string UserId { get; set; }= default!;
    public string Password { get; set; } = default!;

}
