using Data.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class CirculoDirigentes : UsuarioBase
    {
        [Key]
        public int Id { get; set; }
        public int CirculoId { get; set; }
        public Circulo Circulo { get; set; }
        public int EquipanteId { get; set; }
        public EquipanteEvento Equipante { get; set; }
    }
}