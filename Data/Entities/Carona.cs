using Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Carona : UsuarioBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Capacidade { get; set; }
        [ForeignKey("Evento")]
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
        [ForeignKey("Motorista")]
        public int? MotoristaId { get; set; }
        public Equipante Motorista { get; set; }
    }
}