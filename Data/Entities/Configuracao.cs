using Data.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Utils.Enums;

namespace Data.Entities
{
    public class Configuracao : UsuarioBase
    {
        [Key]
        public int Id { get; set; }
        public int? LogoId { get; set; }
        public virtual ICollection<ConfiguracaoCampos> Campos { get; set; }
        public virtual ICollection<ConfiguracaoEquipes> Equipes { get; set; }
        public virtual ICollection<MeioPagamento> MeioPagamentos { get; set; }
        public virtual ICollection<CentroCusto> CentroCustos { get; set; }
        public virtual ICollection<Etiqueta> Etiquetas { get; set; }
        public virtual ICollection<Mensagem> Mensagens { get; set; }
        public Arquivo Logo { get; set; }
        public int? LogoRelatorioId { get; set; }
        public Arquivo LogoRelatorio { get; set; }
        public int? EquipeCirculoId { get; set; }
        public Equipe EquipeCirculo { get; set; }
        public int? CentroCustoInscricaoId { get; set; }
        public CentroCusto CentroCustoInscricao { get; set; }
        public int? CentroCustoTaxaId { get; set; }
        public CentroCusto CentroCustoTaxa { get; set; }
        public int? BackgroundId { get; set; }
        public Arquivo Background { get; set; }
        public int? BackgroundCelularId { get; set; }
        public Arquivo BackgroundCelular { get; set; }
        public string Titulo { get; set; }
        public string CorBotao { get; set; }
        public string InscricaoConcluida { get; set; }
        public string InscricaoCompleta { get; set; }
        public string CorHoverBotao { get; set; }
        public string CorLoginBox { get; set; }
        public string CorScroll { get; set; }
        public string CorHoverScroll { get; set; }
        public string MsgConclusao { get; set; }
        public string MsgConclusaoEquipe { get; set; }
        public TipoCirculoEnum TipoCirculo { get; set; }
    }
}
