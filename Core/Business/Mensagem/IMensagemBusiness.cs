using Core.Models.Mensagem;
using System.Linq;

namespace Core.Business.Mensagem
{
    public interface IMensagemBusiness
    {
        IQueryable<Data.Entities.Mensagem> GetMensagems(int configuracaoId);
        Data.Entities.Mensagem GetMensagemById(int id);
        void PostMensagem(PostMessageModel model);
        void DeleteMensagem(int id);
    }
}
