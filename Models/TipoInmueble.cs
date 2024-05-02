using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    [Table("tipo_inmueble")]
    public class TipoInmueble
    {
        public int id { get; set; }
        public string? tipo { get; set; }
        public bool borrado { get; set; }

    }
}