using System;

namespace Core.Models.Reunioes
{
    public class PostReuniaoModel
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public DateTime DataReuniao { get; set; }
        public string Titulo { get; set; }
        public string Pauta { get; set; }
    }
}
