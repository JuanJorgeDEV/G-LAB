namespace ProjetoOS.Models;

public class HistoricoOS
{
    public int Id { get; set; }

    public int OrdemServicoId { get; set; }
    public OrdemServico OrdemServico { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public string Acao { get; set; } = string.Empty;
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public string? Observacao { get; set; }
}
