using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models
{
    public class Donos
    {
        public Donos()
        {
            ListaAnimais = new HashSet<Animais>();//estou a 'colocar dados na lista de animais de cada dono
        }
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "O Nome é de preenchimento obrigatório.")]
        [StringLength(40, ErrorMessage = "=O {0} não pode ter mais de {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O {0} deve ter exatamente {1} caracteres.")]
        [RegularExpression("[1356][0-9]{8}", ErrorMessage = "Deve escrever exatamente 9 algarismos, começados por 1, 3, 5 e 6")]
        public string NIF { get; set; }

        //select * from Animais a, Donos d where a.DonoFK = d.ID and d.id = ?
        public ICollection<Animais> ListaAnimais { get; set; }
    }
}
