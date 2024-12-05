using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GestionDeGrossite.Model
{
    public class produits
    {
        public string nom { get; set; }
        public decimal prix { get; set; }
        public int quantite { get; set; }
        public string categorie { get; set; }
        public DateTime data_peremtion { get; set; }
        public string emplacement { get; set; }
    }

    }
}
