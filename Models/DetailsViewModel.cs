using System;
using System.Collections.Generic;
using hki.web.Models.Identity;

namespace hki.web.Models
{
    public class DetailsViewModel
    {

        public Orden Orden { get; set; }
        public List<Piezas> Piezas { get; set; }
    }
}