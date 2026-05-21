using System.ComponentModel.DataAnnotations;

namespace ProjetoOS.Models;

public class Equipamento
{
    public int Id { get; set; }
    public bool PossuiNI { get; set; }
    public string? NI { get; set; }

    [Required(ErrorMessage = "Informe o nome do equipamento.")]
    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }
    public string? Localizacao { get; set; }
    public string? Vinculo { get; set; }
    public bool Ativo { get; set; } = true;
}
