using System.ComponentModel.DataAnnotations.Schema;
using inmobiliaria.Models;

namespace inmobiliaria
{
    public class Pago
    {
        public int id { get; set; }
        [ForeignKey("id_contrato")]
        [Column("id_contrato")]
        public int contratoid { get; set; }
        public DateOnly fecha_pago { get; set; }
        public decimal importe { get; set; }
        public bool estado { get; set; }
        public int numero_pago { get; set; }
        public string? detalle { get; set; }
        public Contrato? Contrato { get; set; }

        // [NotMapped]
        // public DateOnly fecha_creado { get; set; }
        // [NotMapped]
        // public DateOnly fecha_editado { get; set; }
        // [NotMapped]
        // public Usuario? creado_usuario { get; set; }
        // [NotMapped]
        // public Usuario? editado_usuario { get; set; }

    }
}

