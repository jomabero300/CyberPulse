using CyberPulse.Backend.Repositories.Interfaces.Gene;

namespace CyberPulse.Backend.Repositories.Implementations.Gene;

public class SqlInjeValRepository : ISqlInjeValRepository
{
    private readonly string[] _sqlKeywords = {
        "--", ";", "'", "\"", "/*", "*/", "@@",
        "char", "nchar", "varchar", "nvarchar",
        "alter", "begin", "cast", "create", "cursor",
        "declare", "delete", "drop", "end", "exec",
        "execute", "fetch", "insert", "kill", "select",
        "sys", "sysobjects", "syscolumns", "table",
        "update", "union", "where", "xp_", "sp_"
    };
    
    public bool HasSqlInjection(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var lowerInput = input.ToLowerInvariant();

        foreach (var keyword in _sqlKeywords)
        {
            if (lowerInput.Contains(keyword))
            {
                // Verificar que no sea un falso positivo
                if (!IsFalsePositive(lowerInput, keyword))
                    return true;
            }
        }

        return false;
    }

    public string SanitizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        // Remover caracteres peligrosos
        var sanitized = input
            .Replace("'", "")
            .Replace("--", "")
            .Replace(";", "")
            .Replace("/*", "")
            .Replace("*/", "");

        // Limitar longitud
        return sanitized.Length > 100 ? sanitized[..100] : sanitized;
    }

    private bool IsFalsePositive(string input, string keyword)
    {
        // Ejemplo: Permitir palabras comunes que contengan "select" como parte de otra palabra
        if (keyword.Length <= 3) return false;

        // Aquí puedes agregar lógica para identificar falsos positivos
        return false;
    }

}
