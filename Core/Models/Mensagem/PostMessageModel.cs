namespace Core.Models.Mensagem
{
    public class PostMessageModel
    {
        public int Id { get; set; }
        public int ConfiguracaoId { get; set; }
        public string Conteudo { get; set; }
        public string[] Tipos { get; set; }
        public string Titulo { get; set; }
    }
}
