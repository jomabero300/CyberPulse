namespace CyberPulse.Shared.Entities.Gene;

public class Alerta
{
    public int Id { get; set; }
    public DateTime FechaAlerta { get; set; }
    public bool EstadoEnviado { get; set; }
    public string Email { get; set; }
}
