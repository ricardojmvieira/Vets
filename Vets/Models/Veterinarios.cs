using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models
{
    public class Veterinarios
    {
        public Veterinarios()
        {
            ListaConsultas = new HashSet<Consultas>();
        }

        [Key]
        public int ID { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Nº Cédula Profissional")]
        [RegularExpression("vet-[0-9]{5}", ErrorMessage = "O {0} deve ser escrito em minúsculas. <br />Começar por 'vet-', seguido de 5 algarismos.")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        [StringLength(9)]
        public string NumCedulaProf { get; set; }

        public string Fotografia { get; set; }

        public virtual ICollection<Consultas> ListaConsultas { get; set; }
    }
}
