﻿using CyberPulse.Shared.Responses;

namespace CyberPulse.Backend.Helpers;

public interface IMailHelper
{
    ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body, string language);
}
