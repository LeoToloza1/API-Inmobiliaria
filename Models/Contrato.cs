using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace inmobiliaria.Models
{
    public class Contrato
    {
        public int? id { get; set; }
        [ForeignKey("id_inquilino")]
        [Column("id_inquilino")]
        public int inquilinoid { get; set; }
        [ForeignKey("id_inmueble")]
        [Column("id_inmueble")]
        public int inmuebleid { get; set; }
        public DateOnly fecha_inicio { get; set; }
        public DateOnly fecha_fin { get; set; }
        public DateOnly fecha_efectiva { get; set; }
        [NotMapped]
        public int? dias_to_fin { get; set; }
        [NotMapped]
        public int meses_to_fin { get; set; }
        [NotMapped]
        public int meses_contrato { get; set; }
        public decimal monto { get; set; }
        public bool borrado { get; set; }
        public Inquilino? inquilino { get; set; }
        public Inmueble? inmueble { get; set; }



        public override string ToString()
        {
            return @$"id: {id}, id_inquilino: {inquilinoid},  id_inmueble: {inmuebleid}, fecha_inicio: {fecha_inicio}, fecha_fin: {fecha_fin}, fecha_efectiva: {fecha_efectiva}, DIAS TO FIN: {dias_to_fin}, 
            monto:{monto}, 
            inquilino: {inquilino}, 
            inmueble: {inmueble}";
        }
        public string ToStringPago()
        {

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return @$"Inquilino: {inquilino.apellido}, {inquilino.nombre}. Direccion del Inmueble: {inmueble.direccion} Finaliza en {dias_to_fin} dias ";
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

        }
        public decimal CalculoMulta()
        {
            //calculo de multa
            double? mediaContrato = (double?)(meses_contrato) / 2;
            decimal multa = 0;
            if (meses_to_fin > mediaContrato)
            {
                multa = monto * 3;
            }
            else if (meses_to_fin < mediaContrato && meses_to_fin > 0)
            {
                multa = monto * 2;
            }
            return multa;
        }
        public void fechaCancelar(string fechaCancela)
        {
            DateOnly fecha1 = (DateOnly.Parse(fechaCancela).AddMonths(1));
            string fecha1_str = $"{fecha1.Year}-{fecha1.Month:00}-01";
            fecha_efectiva = DateOnly.Parse(fecha1_str);
        }
        public bool debeRenovar()
        {
            if (dias_to_fin < 45)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool estaVencido()
        {
            if (fecha_efectiva > fecha_fin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}