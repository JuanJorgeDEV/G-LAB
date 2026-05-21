using System.ComponentModel.DataAnnotations;

namespace ProjetoOS.Models;

public class OrdemServico
{
    public int Id { get; set; }

    public int EquipamentoId { get; set; }
    public Equipamento Equipamento { get; set; } = null!;

    public int SolicitanteId { get; set; }
    public Usuario Solicitante { get; set; } = null!;

    public int? ResponsavelId { get; set; }
    public Usuario? Responsavel { get; set; }

    [Required(ErrorMessage = "Descreva o problema.")]
    public string DescricaoProblema { get; set; } = string.Empty;

    public string? TipoProblema { get; set; }
    public string Status { get; set; } = "Pendente";
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime? DataConclusao { get; set; }
}
