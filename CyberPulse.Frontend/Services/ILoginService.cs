﻿namespace CyberPulse.Frontend.Services;

public interface ILoginService
{
    Task LoginAsync(string token);
    Task LogoutAsync();
}
