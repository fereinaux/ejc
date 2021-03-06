using Core.Models.CentroCusto;
using System.Linq;

namespace Core.Business.CentroCusto
{
    public interface ICentroCustoBusiness
    {
        IQueryable<Data.Entities.CentroCusto> GetCentroCustos(int configuracaoId);
        Data.Entities.CentroCusto GetCentroCustoById(int id);
        void PostCentroCusto(PostCentroCustoModel model);
        void DeleteCentroCusto(int id);
    }
}
