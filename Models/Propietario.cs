using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{

    public class Propietario
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? dni { get; set; }
        public string? email { get; set; }
        public string? telefono { get; set; }
        public string? password { get; set; }
        public bool borrado { get; set; }
        public string? avatarUrl { get; set; }

        [NotMapped]
        public IFormFile? avatarFile { get; set; }

        public override string ToString()
        {
            return $"id: {id}, nombre: {nombre}, apellido: {apellido}, dni: {dni}, email: {email}, telefono: {telefono}, borrado: {borrado}";
        }
        public string ToStringWeb()
        {
            return $" {apellido} , {nombre} dni: {dni} ";
        }

    }
}