using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGrossite.Model
{
    public class orders
    {
        public int Id { get; set; }
        public int Client { get; set; }
        public int Produit { get; set; }
        public DateTime DateCommande { get; set; }
        public int Quantite { get; set; }
        public string Statut { get; set; }

    }
}
