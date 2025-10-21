using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApiProject.Models;

public class Consumo
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Cpf { get; set; } = null!;

    [Range(1, 12)]
    public int Mes { get; set; }

    [Range(2000, int.MaxValue)]
    public int Ano { get; set; }

    [Range(0.0000001, double.MaxValue)]
    public double M3Consumidos { get; set; }

    public double ConsumoFaturado { get; set; }
    public double Tarifa { get; set; }
    public double ValorAgua { get; set; }
    public double AdicionalBandeira { get; set; }
    public double TaxaEsgoto { get; set; }
    public double Total { get; set; }

    public string Bandeira { get; set; } = "Verde";
    public bool PossuiEsgoto { get; set; }
}
