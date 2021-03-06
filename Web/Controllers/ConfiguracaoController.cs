using Core.Business.Configuracao;
using Core.Business.Eventos;
using Core.Models.Configuracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utils.Constants;
using Utils.Enums;
using static Utils.Extensions.EnumExtensions;

namespace SysIgreja.Controllers
{

    [Authorize(Roles = Usuario.Master + "," + Usuario.Admin + "," + Usuario.Secretaria)]
    public class ConfiguracaoController : Controller
    {
        private readonly IConfiguracaoBusiness configuracaoBusiness;
        private readonly IEventosBusiness eventoBusiness;

        public ConfiguracaoController(IConfiguracaoBusiness configuracaoBusiness, IEventosBusiness eventoBusiness)
        {
            this.configuracaoBusiness = configuracaoBusiness;
            this.eventoBusiness = eventoBusiness;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Parâmetros";

            return View();
        }

        public ActionResult Equipe()
        {
            ViewBag.Title = "Equipes";

            return View();
        }


        [HttpGet]
        public ActionResult GetConfiguracoes()
        {
            var result = configuracaoBusiness.GetConfiguracoes()
                .ToList()
                .Select(x => new PostConfiguracaoModel
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    BackgroundId = x.BackgroundId,
                    EquipeCirculoId = x.EquipeCirculoId,
                    CentroCustoInscricaoId = x.CentroCustoInscricaoId,
                    CentroCustoTaxaId = x.CentroCustoTaxaId,
                    BackgroundCelularId = x.BackgroundCelularId,
                    CorBotao = x.CorBotao,
                    CorHoverBotao = x.CorHoverBotao,
                    CorHoverScroll = x.CorHoverScroll,
                    TipoCirculoId = x.TipoCirculo,
                    TipoCirculo = x.TipoCirculo.GetDescription(),
                    CentroCustos = x.CentroCustos.Select(y => new CentroCustoModel
                    {
                        Descricao = y.Descricao,
                        Tipo = y.Tipo.GetDescription(),
                        Id = y.Id
                    }).ToList(),
                    CorLoginBox = x.CorLoginBox,
                    CorScroll = x.CorScroll,
                    LogoId = x.LogoId,
                    LogoRelatorioId = x.LogoRelatorioId,
                    Logo = x.Logo != null ? Convert.ToBase64String(x.Logo.Conteudo) : "",
                    Background = x.Background != null ? Convert.ToBase64String(x.Background.Conteudo) : "",
                    LogoRelatorio = x.LogoRelatorio != null ? Convert.ToBase64String(x.LogoRelatorio.Conteudo) : "",
                    BackgroundCelular = x.BackgroundCelular != null ? Convert.ToBase64String(x.BackgroundCelular.Conteudo) : "",
                    MsgConclusao = x.MsgConclusao,
                    MsgConclusaoEquipe = x.MsgConclusaoEquipe,                    

                });

            var jsonRes = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            jsonRes.MaxJsonLength = Int32.MaxValue;
            return jsonRes;
        }

        [HttpGet]
        public ActionResult GetConfiguracao(int? Id)
        {
            var result = configuracaoBusiness.GetConfiguracao(Id);

            var jsonRes = Json(new { Configuracao = result }, JsonRequestBehavior.AllowGet);
            jsonRes.MaxJsonLength = Int32.MaxValue;
            return jsonRes;
        }

        [HttpGet]
        public ActionResult GetConfiguracaoByEventoId(int Id)
        {
            var result = configuracaoBusiness.GetConfiguracaoByEventoId(Id);

            var jsonRes = Json(new { Configuracao = result }, JsonRequestBehavior.AllowGet);
            jsonRes.MaxJsonLength = Int32.MaxValue;
            return jsonRes;
        }



        [HttpGet]
        public ActionResult GetCamposByEventoId(int id)
        {
            var evento = eventoBusiness.GetEventoById(id);
            var result = configuracaoBusiness.GetCampos(evento.ConfiguracaoId.Value);

            return Json(new { Campos = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCampos(int id)
        {
            var result = configuracaoBusiness.GetCampos(id);

            return Json(new { Campos = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEquipes(int id)
        {
            var result = configuracaoBusiness.GetEquipes(id);

            return Json(new { Equipes = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetCamposEnum()
        {
            var result = GetDescriptions<CamposEnum>();

            return Json(new { Campos = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostCampos(List<CamposModel> campos, int id)
        {
            configuracaoBusiness.PostCampos(campos, id);

            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult PostEquipes(List<EquipesModel> equipes, int id)
        {
            configuracaoBusiness.PostEquipes(equipes, id);

            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult PostConfiguracao(PostConfiguracaoModel model)
        {
            configuracaoBusiness.PostConfiguracao(model);

            return new HttpStatusCodeResult(200);
        }


        [HttpPost]
        public ActionResult PostLogo(int id, int sourceId)
        {
            configuracaoBusiness.PostLogo(sourceId, id);

            return new HttpStatusCodeResult(200);
        }


        [HttpPost]
        public ActionResult PostBackground(int id, int sourceId)
        {
            configuracaoBusiness.PostBackground(sourceId, id);

            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult PostLogoRelatorio(int id, int sourceId)
        {
            configuracaoBusiness.PostLogoRelatorio(sourceId, id);

            return new HttpStatusCodeResult(200);
        }


        [HttpPost]
        public ActionResult PostBackgroundCelular(int id, int sourceId)
        {
            configuracaoBusiness.PostBackgroundCelular(sourceId, id);

            return new HttpStatusCodeResult(200);
        }
    }
}