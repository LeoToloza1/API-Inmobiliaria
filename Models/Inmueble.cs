using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace inmobiliaria.Models
{
    public enum EstadoInmueble
    {
        Disponible,
        Retirado
    }
    public enum UsoDeInmueble
    {
        Comercial,
        Residencial
    }

    public class Inmueble
    {
        [Key]
        public int id { get; set; }
        [StringLength(100)]
        [Display(Name = "Dirección")]
        public string? direccion { get; set; }
        [StringLength(20)]
        [Display(Name = "Uso")]
        public string? uso { get; set; }
        [ForeignKey("id_tipo")]
        [Column("id_tipo")]
        public int tipoInmuebleid { get; set; }
        [Display(Name = "Ambientes")]
        public int ambientes { get; set; }
        [Display(Name = "Coordenadas")]
        public string? coordenadas { get; set; }
        [Display(Name = "Precio")]
        public decimal precio { get; set; }
        [ForeignKey("id_propietario")]
        [Column("id_propietario")]
        public int PropietarioId { get; set; }
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string? estado { get; set; }
        public bool borrado { get; set; }
        [Display(Name = "Descripción")]
        public string? descripcion { get; set; }
        [Display(Name = "Propietario")]
        public Propietario? propietario { set; get; }
        [Display(Name = "Tipo Inmueble")]

        public TipoInmueble? tipoInmueble { set; get; }
        [Display(Name = "Mapa")]
        public string mapa => $"https://www.google.com/maps?q={coordenadas}";
        public override string ToString()
        {
            return $"{direccion}    {uso}   Ambientes: {ambientes}  $ {precio}   Descripción: {descripcion} ";
        }
        public string datosPropietario()
        {
            return $"{propietario?.nombre}, {propietario?.apellido}";
        }

    }
}