using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace hki.web.Models
{
    public class IndexViewModel
    {
        public int Soldadura { get; set; }
        public int Acabado { get; set; }
        public int Limpieza { get; set; }
        public int Electricos { get; set; }
        public int Calidad { get; set; }
        public int Total { get; set; }
        public List<Orden> Ordenes { get; set; }

    
    }
}