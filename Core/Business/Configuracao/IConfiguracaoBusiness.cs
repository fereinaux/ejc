using Core.Models.Configuracao;
using System.Collections.Generic;
using System.Linq;

namespace Core.Business.Configuracao
{
    public interface IConfiguracaoBusiness
    {
        IQueryable<Data.Entities.Configuracao> GetConfiguracoes();
        PostConfiguracaoModel GetConfiguracao(int? configId);
        PostConfiguracaoModel GetConfiguracaoByEventoId(int configId);
        IEnumerable<CamposModel> GetCampos(int id);
        IEnumerable<EquipesModel> GetEquipes(int id);
        void PostCampos(IEnumerable<CamposModel> campos, int id);
        void PostEquipes(IEnumerable<EquipesModel> equipes, int id);
        void PostConfiguracao(PostConfiguracaoModel model);
        void PostLogo(int logoId, int Id);
        void PostBackground(int backgroundId, int Id);
        void PostLogoRelatorio(int logoId, int Id);
        void PostBackgroundCelular(int backgroundId, int Id);
    }
}
