﻿using Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class ReuniaoEvento : StatusBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventoId { get; set; }
        public string Titulo { get; set; }
        public Evento Evento { get; set; }
        public DateTime DataReuniao { get; set; }
        public ICollection<PresencaReuniao> Presenca { get; set; }
        public string Pauta { get; set; }
    }
}