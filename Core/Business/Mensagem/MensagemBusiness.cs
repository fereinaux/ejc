using Core.Models.Mensagem;
using Data.Repository;
using System.Linq;

namespace Core.Business.Mensagem
{
    public class MenssagemBusinesss : IMensagemBusiness
    {
        private readonly IGenericRepository<Data.Entities.Mensagem> repo;

        public MenssagemBusinesss(IGenericRepository<Data.Entities.Mensagem> repo)
        {
            this.repo = repo;
        }



        public void DeleteMensagem(int id)
        {
            repo.Delete(id);
            repo.Save();
        }

        public Data.Entities.Mensagem GetMensagemById(int id)
        {
            return repo.GetById(id);
        }

        public IQueryable<Data.Entities.Mensagem> GetMensagems(int configuracaoId)
        {
            return repo.GetAll(x => x.ConfiguracaoId == configuracaoId);
        }

        public void PostMensagem(PostMessageModel model)
        {
            Data.Entities.Mensagem mensagem = null;

            if (model.Id > 0)
            {
                mensagem = repo.GetById(model.Id);

                mensagem.Conteudo = model.Conteudo;
                mensagem.ConfiguracaoId = model.ConfiguracaoId;
                mensagem.Tipos = string.Join(",", model.Tipos);
                mensagem.Titulo = model.Titulo;

                repo.Update(mensagem);
            }
            else
            {
                mensagem = new Data.Entities.Mensagem
                {
                    Conteudo = model.Conteudo,
                    Titulo = model.Titulo,
                    ConfiguracaoId = model.ConfiguracaoId,
                    Tipos = string.Join(",", model.Tipos)
                };

                repo.Insert(mensagem);
            }

            repo.Save();
        }


    }
}
