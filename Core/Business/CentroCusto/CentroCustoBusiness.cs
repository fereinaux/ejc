using Core.Models.CentroCusto;
using Data.Repository;
using System.Linq;

namespace Core.Business.CentroCusto
{
    public class CentroCustoBusiness : ICentroCustoBusiness
    {
        private readonly IGenericRepository<Data.Entities.CentroCusto> centroCustoRepository;

        public CentroCustoBusiness(IGenericRepository<Data.Entities.CentroCusto> centroCustoRepository)
        {
            this.centroCustoRepository = centroCustoRepository;
        }

        public void DeleteCentroCusto(int id)
        {
            centroCustoRepository.Delete(id);
            centroCustoRepository.Save();
        }

        public Data.Entities.CentroCusto GetCentroCustoById(int id)
        {
            return centroCustoRepository.GetById(id);
        }

        public IQueryable<Data.Entities.CentroCusto> GetCentroCustos(int configuracaoId)
        {
            return centroCustoRepository.GetAll(x => x.ConfiguracaoId == configuracaoId);
        }

        public void PostCentroCusto(PostCentroCustoModel model)
        {
            Data.Entities.CentroCusto centroCusto = null;

            if (model.Id > 0)
            {
                centroCusto = centroCustoRepository.GetById(model.Id);
                centroCusto.ConfiguracaoId = model.ConfiguracaoId;
                centroCusto.Descricao = model.Descricao;
                centroCusto.Tipo = model.Tipo;

                centroCustoRepository.Update(centroCusto);
            }
            else
            {
                centroCusto = new Data.Entities.CentroCusto
                {
                    Descricao = model.Descricao,
                    Tipo = model.Tipo,
                    ConfiguracaoId = model.ConfiguracaoId
                };

                centroCustoRepository.Insert(centroCusto);
            }

            centroCustoRepository.Save();
        }
    }
}
