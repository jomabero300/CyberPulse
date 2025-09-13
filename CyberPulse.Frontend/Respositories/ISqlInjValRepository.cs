namespace CyberPulse.Frontend.Respositories;

public interface ISqlInjValRepository
{
    bool HasSqlInjection(string input);
    string SanitizeInput(string input);
}