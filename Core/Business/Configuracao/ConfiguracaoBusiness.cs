using Core.Business.Eventos;
using Core.Models.Configuracao;
using Data.Entities;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Utils.Extensions;

namespace Core.Business.Configuracao
{
    public class ConfiguracaoBusiness : IConfiguracaoBusiness
    {
        private readonly IGenericRepository<Data.Entities.Configuracao> repo;
        private readonly IGenericRepository<ConfiguracaoCampos> camposRepo;
        private readonly IGenericRepository<ConfiguracaoEquipes> equipesRepo;
        private readonly IEventosBusiness eventosBusiness;

        public ConfiguracaoBusiness(IEventosBusiness eventosBusiness,IGenericRepository<Data.Entities.Configuracao> repo, IGenericRepository<ConfiguracaoCampos> camposRepo, IGenericRepository<ConfiguracaoEquipes> equipesRepo)
        {
            this.repo = repo;
            this.camposRepo = camposRepo;
            this.equipesRepo = equipesRepo;
            this.eventosBusiness = eventosBusiness;
        }

        public IQueryable<Data.Entities.Configuracao> GetConfiguracoes()
        {
            return repo.GetAll().Include(x => x.LogoRelatorio).Include(x => x.BackgroundCelular).Include(x => x.Logo).Include(x => x.Background);
        }

        public PostConfiguracaoModel GetConfiguracao(int? configId)
        {
            return repo.GetAll(x => x.Id == (configId.HasValue ? configId.Value : 1)).Include(x => x.LogoRelatorio).Include(x => x.BackgroundCelular).Include(x => x.Logo).Include(x => x.Background).ToList().Select(x => new PostConfiguracaoModel
            {
                Id = x.Id,
                Titulo = x.Titulo,
                BackgroundId = x.BackgroundId,
                BackgroundCelularId = x.BackgroundCelularId,
                CorBotao = x.CorBotao,
                CorHoverBotao = x.CorHoverBotao,
                CorHoverScroll = x.CorHoverScroll,
                TipoCirculoId = x.TipoCirculo,
                TipoCirculo = x.TipoCirculo.GetDescription(),
                CorLoginBox = x.CorLoginBox,
                CorScroll = x.CorScroll,
                LogoId = x.LogoId,
                LogoRelatorioId = x.LogoRelatorioId,
                MsgConclusao = x.MsgConclusao,
                MsgConclusaoEquipe = x.MsgConclusaoEquipe,
                Logo = x.Logo != null ? Convert.ToBase64String(x.Logo.Conteudo) : "",
                Background = x.Background != null ? Convert.ToBase64String(x.Background.Conteudo) : "",
                LogoRelatorio = x.LogoRelatorio != null ? Convert.ToBase64String(x.LogoRelatorio.Conteudo) : "",
                BackgroundCelular = x.BackgroundCelular != null ? Convert.ToBase64String(x.BackgroundCelular.Conteudo) : ""

            }).FirstOrDefault();
        }


        public IEnumerable<CamposModel> GetCampos(int id)
        {
            return camposRepo.GetAll(x => x.ConfiguracaoId == id).ToList().Select(x => new CamposModel
            {
                Campo = x.Campo.GetDescription(),
                CampoId = x.Campo,
                Id = x.Id
            });
        }

        public IEnumerable<EquipesModel> GetEquipes(int id)
        {
            return equipesRepo.GetAll(x => x.ConfiguracaoId == id).Include(x => x.Equipe).ToList().Select(x => new EquipesModel
            {
                Equipe = x.Equipe.Nome,
                EquipeId = x.EquipeId,
                Id = x.Id
            });
        }

        public void PostCampos(IEnumerable<CamposModel> campos, int id)
        {
            var camposBanco = camposRepo.GetAll(x => x.ConfiguracaoId == id).ToList();

            camposBanco.ForEach(campoBanco =>
            {
                if (!campos.Select(x => x.CampoId).ToList().Any(y => y == campoBanco.Campo))
                {
                    camposRepo.Delete(campoBanco.Id);
                }
            });

            campos.ToList().ForEach(campo =>
            {
                if (!camposBanco.Select(x => x.Campo).ToList().Any(y => y == campo.CampoId))
                {
                    camposRepo.Insert(new Data.Entities.ConfiguracaoCampos
                    {
                        Campo = campo.CampoId,
                        ConfiguracaoId = id
                    });
                }
            });

            camposRepo.Save();
        }

        public void PostEquipes(IEnumerable<EquipesModel> equipes, int id)
        {
            var equipesBanco = equipesRepo.GetAll(x => x.ConfiguracaoId == id).ToList();

            equipesBanco.ForEach(equipeBanco =>
            {
                if (!equipes.Select(x => x.EquipeId).ToList().Any(y => y == equipeBanco.EquipeId))
                {
                    equipesRepo.Delete(equipeBanco.Id);
                }
            });

            equipes.ToList().ForEach(equipe =>
            {
                if (!equipesBanco.Select(x => x.EquipeId).ToList().Any(y => y == equipe.EquipeId))
                {
                    equipesRepo.Insert(new Data.Entities.ConfiguracaoEquipes
                    {
                        EquipeId = equipe.EquipeId,
                        ConfiguracaoId = id
                    });
                }
            });

            equipesRepo.Save();
        }

        public void PostBackground(int backgroundId)
        {
            Data.Entities.Configuracao configuracao = repo.GetAll().FirstOrDefault();
            configuracao.BackgroundId = backgroundId;
            repo.Save();
        }

        public void PostConfiguracao(PostConfiguracaoModel model)
        {
            if (model.Id != null)
            {
                Data.Entities.Configuracao configuracao = repo.GetAll().FirstOrDefault(x => x.Id == model.Id);

                configuracao.CorBotao = model.CorBotao;
                configuracao.TipoCirculo = model.TipoCirculoId;
                configuracao.CorHoverBotao = model.CorHoverBotao;
                configuracao.CorHoverScroll = model.CorHoverScroll;
                configuracao.CorScroll = model.CorScroll;
                configuracao.EquipeCirculoId = model.EquipeCirculoId;
                configuracao.Titulo = model.Titulo;
                configuracao.CorLoginBox = model.CorLoginBox;
                configuracao.MsgConclusao = model.MsgConclusao;
                configuracao.MsgConclusaoEquipe = model.MsgConclusaoEquipe;
                repo.Update(configuracao);
            }
            else
            {
                Data.Entities.Configuracao configuracao = new Data.Entities.Configuracao
                {                
                    Titulo = "SysIgreja"     
                };

                repo.Insert(configuracao);

            }


            repo.Save();
        }

        public void PostLogo(int logoId, int Id)
        {
            Data.Entities.Configuracao configuracao = repo.GetAll(x => x.Id == Id).FirstOrDefault();
            configuracao.LogoId = logoId;
            repo.Save();
        }

        public void PostLogoRelatorio(int logoId, int Id)
        {
            Data.Entities.Configuracao configuracao = repo.GetAll(x => x.Id == Id).FirstOrDefault();
            configuracao.LogoRelatorioId = logoId;
            repo.Save();
        }

        public void PostBackgroundCelular(int backgroundId, int Id)
        {
            Data.Entities.Configuracao configuracao = repo.GetAll(x => x.Id == Id).FirstOrDefault();
            configuracao.BackgroundCelularId = backgroundId;
            repo.Save();
        }

        public void PostBackground(int backgroundId, int Id)
        {
            Data.Entities.Configuracao configuracao = repo.GetAll(x => x.Id == Id).FirstOrDefault();
            configuracao.BackgroundId = backgroundId;
            repo.Save();
        }

        public PostConfiguracaoModel GetConfiguracaoByEventoId(int configId)
        {
            var evento = eventosBusiness.GetEventoById(configId);
            return GetConfiguracao(evento.ConfiguracaoId);
        }
    }
}
