using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Models
{
    public class Consultas
    {
        [Key]
        public int ID { get; set; }
        public DateTime Data { get; set; }
        public string Observacoes { get; set; }

        //definir FKs
        //FK -> Animais
        [ForeignKey(nameof(Animal))]//nameoF() -> devolve a designação (como string) do objeto que é colocado como parametro
        public int AnimalFK { get; set; }
        public Animais Animal { get; set; }

        //FK -> Veterinarios
        [ForeignKey(nameof(Veterinario))]//nameoF() -> devolve a designação (como string) do objeto que é colocado como parametro
        public int VeterinarioFK { get; set; }
        public Veterinarios Veterinario { get; set; }
    }
}
