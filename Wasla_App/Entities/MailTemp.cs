using System;
using System.Collections.Generic;

namespace Wasla_App.Entities;

public partial class MailTemp
{
    public int id { get; set; }

    public string? lang { get; set; }

    public string? mail_Subject { get; set; }

    public string? mail_body { get; set; }

    /// <summary>
    /// 1 =&gt; for mail Confirmation
    /// 2 =&gt; for otp verify
    /// </summary>
    public int? type { get; set; }
}
