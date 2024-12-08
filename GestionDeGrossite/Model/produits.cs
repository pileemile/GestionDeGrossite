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
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prix { get; set; }
        public int Quantite { get; set; }
        public DateTime? date_peremtion { get; set; }
        public string Emplacement { get; set; }
        public int Categorie { get; set; }
    }

}
