using Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.Enums;

namespace Data.Entities
{
    public class Quarto : UsuarioBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Evento")]
        public int? EventoId { get; set; }
        public Evento Evento { get; set; }
        public int? EquipanteId { get; set; }
        public Equipante Equipante { get; set; }
        public string Titulo { get; set; }
        public SexoEnum Sexo { get; set; }
        public TipoPessoaEnum TipoPessoa { get; set; }
        public int Capacidade { get; set; }
    }
}