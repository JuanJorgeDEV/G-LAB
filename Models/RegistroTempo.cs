namespace ProjetoOS.Models;

public class RegistroTempo
{
    public int Id { get; set; }

    public int OrdemServicoId { get; set; }
    public OrdemServico OrdemServico { get; set; } = null!;

    public int ResponsavelId { get; set; }
    public Usuario Responsavel { get; set; } = null!;

    public DateTime Inicio { get; set; }
    public DateTime? Fim { get; set; }
}
