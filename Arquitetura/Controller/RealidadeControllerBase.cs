using Arquitetura.ViewModels;
using Core.Business.Account;
using Core.Business.Configuracao;
using Core.Business.Eventos;
using Core.Models.Configuracao;
using Data.Context;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Utils.Extensions;

namespace Arquitetura.Controller
{
    [Authorize]
    public class SysIgrejaControllerBase : System.Web.Mvc.Controller
    {
        private readonly IEventosBusiness eventosBusiness;
        private readonly IAccountBusiness accountBusiness;
        private readonly IConfiguracaoBusiness configuracaoBusiness;

        public SysIgrejaControllerBase(IEventosBusiness eventosBusiness, IAccountBusiness accountBusiness, IConfiguracaoBusiness configuracaoBusiness)
        {
            this.eventosBusiness = eventosBusiness;
            this.accountBusiness = accountBusiness;
            this.configuracaoBusiness = configuracaoBusiness;
        }

        [HttpGet]
        public ActionResult DownloadTempFile(string g, string fileName)
        {
            MemoryStream ms = Session[g] as MemoryStream;
            if (ms == null)
            {
                return new EmptyResult();
            }
            Session[g] = null;
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public void GetEventos()
        {
            ViewBag.Eventos = eventosBusiness
                .GetEventos()
                .OrderByDescending(x => x.DataEvento)
                .ToList()
                .Select(x => new EventoViewModel
                {
                    Id = x.Id,
                    DataEvento = x.DataEvento,
                    Descricao = x.Descricao,
                    Numeracao = x.Numeracao,
                    TipoEvento = x.Configuracao?.Titulo,
                    Status = x.Status.GetDescription()
                });
        }

        public void GetConfiguracao()
        {
            ViewBag.Configuracao = configuracaoBusiness
                .GetConfiguracao(null);
        }

        public void GetConfiguracoes()
        {
         
            ViewBag.Configuracoes = configuracaoBusiness
                .GetConfiguracoes().ToList()
                .Select(x => new PostConfiguracaoModel
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Etiquetas = x.Etiquetas.Select(y => new EtiquetaModel
                    {
                        Cor = y.Cor,
                        Id = y.Id,
                        Nome = y.Nome
                    }).ToList(),
                    MeioPagamentos = x.MeioPagamentos.Select(y => new MeioPagamentoModel
                    {
                        Descricao = y.Descricao,
                        Id = y.Id
                    }).ToList()
                });
        }

        public ApplicationUser GetApplicationUser()
        {
            return accountBusiness.GetUsuarioById(User.Identity.GetUserId());
        }
    }
}
