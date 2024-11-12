using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GestionDeGrossite.Model
{
    public class Produit
    {
        [Required]
        int id;
        string nom;
        private int qte;
    }
}
