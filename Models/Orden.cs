using System;
using hki.web.Models.Identity;

namespace hki.web.Models
{
    public class Orden
    {
        public string Id { get; set; }
        public Dias Dia { get; set; }
        public string ProductoId { get; set; }
        public string Descripcion { get; set; }
        public float ValorHrs { get; set; }
        public int Cantidad { get; set; }
        public int Finalizadas { get; set; }
        public float TotalHrs { get; set; }
        public Roles Asignado { get; set; }
        public Ubicaciones Ubicacion { get; set; }
        public string Estatus2 { get; set; }
        public string Estatus3 { get; set; }
        public DateTime Levantamiento { get; set; }
        public DateTime FechaReq { get; set; }
        
    }

    public enum Dias
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public enum Ubicaciones
    {
        Soldadura,
        Acabado,
        Limpieza,
        Electricos,
        Calidad
    }
}