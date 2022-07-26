using Data.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.Enums;

namespace Data.Entities
{
    public class EquipanteEvento : UsuarioBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Evento")]
        public int? EventoId { get; set; }
        public Evento Evento { get; set; }
        public int? EquipanteId { get; set; }
        public Equipante Equipante { get; set; }
        public bool Checkin { get; set; }
        public int? EquipeId { get; set; }
        public Equipe Equipe { get; set; }
        public TiposEquipeEnum Tipo { get; set; }
        public virtual ICollection<PresencaReuniao> Presencas { get; set; }
    }
}