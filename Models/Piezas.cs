using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace hki.web.Models
{
    public class Piezas
    {
        public string Id { get; set; }
        public Orden Orden { get; set; }
        public Ubicaciones Ubicacion { get; set; }
        public EstatusP Estatus { get; set; }
        public string Levantamiento { get; set; }
        public string UltimaModificacion { get; set; }
        public string Comentarios { get; set; }
        public bool Terminado { get; set; }
    }

    public enum EstatusP
    {
        Null,
        Surtir,
        Preparado,
        [Display(Name = "Acero/Consumible")]
        Acero,
        Eliminada,
        [Display(Name = "Surtir en ventanilla")]
        SurtirV
    }
}