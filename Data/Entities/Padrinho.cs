using Data.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Padrinho : UsuarioBase
    {
        [Key]
        public int Id { get; set; }
        public int? EquipanteEventoId { get; set; }
        public EquipanteEvento EquipanteEvento { get; set; }
        public virtual ICollection<Participante> Participantes { get; set; }
    }
}