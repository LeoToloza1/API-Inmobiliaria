using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? dni { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? rol { get; set; }
        public string? avatarUrl { get; set; }
        [Required(ErrorMessage = "El avatar es requerido")]
        [NotMapped]
        public IFormFile? avatarFile { get; set; }

        public bool borrado { get; set; }
        public override string ToString()
        {
            return $"{id}  | {nombre}  |  {apellido}  |   {dni}  |   {email} | {password}  | {rol}  | {avatarUrl} | {borrado}";
        }
    }
}
