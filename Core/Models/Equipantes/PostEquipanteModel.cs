using Core.Models.Etiquetas;
using System;
using System.Collections.Generic;
using Utils.Enums;

namespace Core.Models.Equipantes
{
    public class PostEquipanteModel
    {
        public int Id { get; set; }
        public int? EventoId { get; set; }
        public string Nome { get; set; }
        public string Equipe { get; set; }
        public string Observacao { get; set; }
        public string Apelido { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string[] Etiquetas { get; set; }
        public IEnumerable<PostEtiquetaModel> EtiquetasList { get; set; }
        public string Email { get; set; }
        public string Fone { get; set; }
        public bool HasRestricaoAlimentar { get; set; }
        public string RestricaoAlimentar { get; set; }
        public string Quarto { get; set; }
        public bool HasMedicacao { get; set; }
        public string Medicacao { get; set; }
        public bool HasAlergia { get; set; }
        public bool HasVacina { get; set; }
        public bool HasTeste { get; set; }
        public bool Checkin { get; set; }
        public bool Inscricao { get; set; }
        public string Alergia { get; set; }
        public string Foto { get; set; }
        public SexoEnum Sexo { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Referencia { get; set; }
        public string Numero { get; set; }
        public string Estado { get; set; }
    }
}
