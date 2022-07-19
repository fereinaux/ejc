using System;
using System.ComponentModel;

namespace Utils.Enums
{
    public enum TipoCirculoEnum
    {
        [Description("Endereco")]
        Endereco,
        [Description("Aleatório")]
        Aleatorio,
        [Description("Idade")]
        Idade,
    }

    public enum TipoPessoaEnum
    {
        [Description("Equipante")]
        Equipante,
        [Description("Participante")]
        Participante
    }
    public enum CamposEnum
    {
        [Description("Email")]
        Email,
        [Description("Nome Completo")]
        Nome,
        [Description("Apelido")]
        Apelido,
        [Description("Gênero")]
        Genero,
        [Description("Data Nascimento")]
        DataNascimento,
        [Description("Fone")]
        Fone,
        [Description("Instagram")]
        Instagram,
        [Description("Profissão")]
        Profissao,
        [Description("Endereço")]
        Endereco,
        [Description("Dados da Mãe")]
        Mae,
        [Description("Dados do Pai")]
        Pai,
        [Description("Dados do Contato")]
        Contato,
        [Description("Dados do Convite")]
        Convite,
        [Description("Ônibus")]
        Onibus,
        [Description("Parente")]
        Parente,
        [Description("Congregação")]
        Congregacao,
        [Description("Alergia")]
        Alergia,
        [Description("Medicação")]
        Medicacao,
        [Description("Restrição Alimentar")]
        Restricao,


    }
    public enum StatusEnum
    {
        [Description("Ativo")]
        Ativo = 1,
        [Description("Inativo")]
        Inativo = 2,
        [Description("Deletado")]
        Deletado = 3,
        [Description("Aberto")]
        Aberto = 4,
        [Description("Quitado")]
        Quitado = 5,
        [Description("Cancelado")]
        Cancelado = 6,
        [Description("Encerrado")]
        Encerrado = 7,
        [Description("Inscrito")]
        Inscrito = 8,
        [Description("Confirmado")]
        Confirmado = 9,
        [Description("Em Espera")]
        Espera = 10
    }

    public enum SexoEnum
    {
        [Description("Masculino")]
        Masculino = 1,
        [Description("Feminino")]
        Feminino = 2
    }
    public enum TiposEquipeEnum
    {
        [Description("Coordenador")]
        Coordenador = 1,
        [Description("Membro")]
        Membro = 2
    }
    public enum PerfisUsuarioEnum
    {
        [Description("Master")]
        Master,
        [Description("Admin")]
        Admin,
        [Description("Coordenador")]
        Coordenador,
        [Description("Secretaria")]
        Secretaria,
    }

    public enum TiposLancamentoEnum
    {
        [Description("Receber")]
        Receber = 1,
        [Description("Pagar")]
        Pagar = 2
    }

    public enum TiposCentroCustoEnum
    {
        [Description("Receita")]
        Receita = 1,
        [Description("Despesa")]
        Despesa = 2
    }

    public enum CentroCustoPadraoEnum
    {
        [Description("Inscrições")]
        Inscricoes = 1,
        [Description("Taxa de Equipante")]
        TaxaEquipante = 2
    }

    public enum MeioPagamentoPadraoEnum
    {
        [Description("Pix")]
        Transferencia,
        [Description("Dinheiro")]
        Dinheiro,
        [Description("Isenção")]
        Isencao
    }

    public enum CoresEnum
    {
        [Description("Vermelho")]
        Vermelho,
        [Description("Laranja")]
        Laranja,
        [Description("Rosa")]
        Rosa,
        [Description("Azul Claro")]
        AzulClaro,
        [Description("Azul Escuro")]
        AzulEscuro,
        [Description("Verde Claro")]
        VerdeClaro,
        [Description("Verde Escuro")]
        VerdeEscuro,
        [Description("Amarelo")]
        Amarelo,
        [Description("Cinza")]
        Cinza,
        [Description("Lilás")]
        Lilas
    }

    internal class NicknameAttribute : Attribute
    {
        public virtual string Nickname { get; }

        public NicknameAttribute(string Nickname)
        {
            this.Nickname = Nickname;
        }
    }

    internal class EquipesAttribute : Attribute
    {
        public virtual int[] Equipes { get; }

        public EquipesAttribute(int[] Equipes)
        {
            this.Equipes = Equipes;
        }
    }

    internal class EmailPagSeguroAttribute : Attribute
    {
        public virtual string EmailPagSeguro { get; }

        public EmailPagSeguroAttribute(string EmailPagSeguro)
        {
            this.EmailPagSeguro = EmailPagSeguro;
        }
    }

    internal class TokenPagSeguroAttribute : Attribute
    {
        public virtual string TokenPagSeguro { get; }

        public TokenPagSeguroAttribute(string TokenPagSeguro)
        {
            this.TokenPagSeguro = TokenPagSeguro;
        }
    }
}