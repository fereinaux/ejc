using Core.Business.Eventos;
using Core.Models.Equipe;
using Data.Context;
using Data.Entities;
using Data.Repository;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Utils.Enums;
using static Utils.Extensions.EnumExtensions;
using System;

namespace Core.Business.Equipes
{
    public class EquipesBusiness : IEquipesBusiness
    {
        private readonly IGenericRepository<EquipanteEvento> equipanteEventoRepository;
        private readonly IGenericRepository<Equipante> equipanteRepository;
        private readonly IGenericRepository<Equipe> equipeRepository;
        private readonly IGenericRepository<ConfiguracaoEquipes> conifgEquipeRepo;
        private readonly IGenericRepository<PresencaReuniao> presencaRepository;
        private readonly IGenericRepository<ApplicationUser> usuarioRepository;
        private readonly IGenericRepository<Evento> eventoRepository;
        private readonly IEventosBusiness eventosBusiness;
        public EquipesBusiness(IEventosBusiness eventosBusiness, IGenericRepository<ConfiguracaoEquipes> conifgEquipeRepo, IGenericRepository<EquipanteEvento> equipanteEventoRepository, IGenericRepository<Equipe> equipeRepository, IGenericRepository<Evento> eventoRepository, IGenericRepository<Equipante> equipanteRepository, IGenericRepository<ApplicationUser> usuarioRepository, IGenericRepository<PresencaReuniao> presencaRepository)
        {
            this.eventosBusiness = eventosBusiness;
            this.equipanteEventoRepository = equipanteEventoRepository;
            this.equipeRepository = equipeRepository;
            this.conifgEquipeRepo = conifgEquipeRepo;
            this.equipanteRepository = equipanteRepository;
            this.eventoRepository = eventoRepository;
            this.usuarioRepository = usuarioRepository;
            this.presencaRepository = presencaRepository;
        }

        public void AddMembroEquipe(PostEquipeMembroModel model)
        {
            EquipanteEvento equipanteEvento = new EquipanteEvento
            {
                EquipanteId = model.EquipanteId,
                EventoId = model.EventoId,
                Tipo = TiposEquipeEnum.Membro,
                EquipeId = model.EquipeId
            };

            equipanteEventoRepository.Insert(equipanteEvento);
            equipanteEventoRepository.Save();
        }

        public void DeleteMembroEquipe(int id)
        {
            equipanteEventoRepository.Delete(id);
            equipanteEventoRepository.Save();
        }

        public EquipanteEvento GetEquipanteEventoByUser(int eventoId, string userId)
        {
            return equipanteEventoRepository
                .GetAll()
                .Include(x => x.Equipante)
                .Include(x => x.Evento)
                .ToList()
                .FirstOrDefault(x => x.EventoId == eventoId && x.EquipanteId == usuarioRepository.GetById(userId).EquipanteId);
        }

        public List<Equipante> GetEquipantesEventoSemEquipe(int eventoId)
        {
            var equipantesIds = equipanteEventoRepository.GetAll().Where(x => x.EventoId == eventoId).Select(y => y.EquipanteId).ToList();

            return equipanteRepository
                .GetAll()
                .ToList()
                .Where(x => !equipantesIds.Contains(x.Id)).ToList();
        }

        public List<Equipante> GetEquipantesByEvento(int eventoId)
        {
            return equipanteEventoRepository
                .GetAll()
                .Include(x => x.Equipante)
                .Include(x => x.Equipante.Arquivos)
                .Include(x => x.Equipe)
                .Where(x => x.EventoId == eventoId)
                .Select(y => y.Equipante).ToList();
        }

        public EquipanteEvento GetEquipeAtual(int eventoId, int equipanteId)
        {
            return equipanteEventoRepository.GetAll(x => x.EquipanteId == equipanteId && x.EventoId == eventoId).FirstOrDefault();
        }

        public IQueryable<EquipanteEvento> GetMembrosEquipe(int eventoId, int equipeId)
        {
            return equipanteEventoRepository
                .GetAll(x => x.EquipeId == equipeId && x.EventoId == eventoId)
                .Include(x => x.Equipante.Arquivos)
                .OrderBy(x => new { x.Tipo, x.Equipante.Nome });
        }


        public IQueryable<EquipanteEvento> GetMembrosEquipeDatatable(int eventoId, int equipeId)
        {
            return equipanteEventoRepository
                .GetAll(x => x.EquipeId == equipeId && x.EventoId == eventoId)
                .Include(x => x.Equipe)
                .Include(x => x.Equipante)
                .OrderBy(x => new { x.Tipo, x.Equipante.Nome });
        }

        public IQueryable<PresencaReuniao> GetPresenca(int reuniaoId)
        {
            return presencaRepository.GetAll(x => x.ReuniaoId == reuniaoId);
        }

        public void ToggleMembroEquipeTipo(int id)
        {
            EquipanteEvento equipanteEvento = equipanteEventoRepository.GetById(id);

            equipanteEvento.Tipo = equipanteEvento.Tipo == TiposEquipeEnum.Coordenador ?
                TiposEquipeEnum.Membro :
                TiposEquipeEnum.Coordenador;

            equipanteEventoRepository.Update(equipanteEvento);
            equipanteEventoRepository.Save();
        }

        public void TogglePresenca(int equipanteEventoId, int reuniaoId)
        {
            var presenca = presencaRepository.GetAll().FirstOrDefault(x => x.EquipanteEventoId == equipanteEventoId && x.ReuniaoId == reuniaoId);

            if (presenca != null)
                presencaRepository.Delete(presenca.Id);
            else
            {
                var newPresenca = new PresencaReuniao
                {
                    ReuniaoId = reuniaoId,
                    EquipanteEventoId = equipanteEventoId
                };
                presencaRepository.Insert(newPresenca);
            }

            presencaRepository.Save();
        }

        public List<Equipante> GetEquipantesAniversariantesByEvento(int eventoId)
        {
            var data = eventosBusiness.GetEventoById(eventoId).DataEvento;

            return GetEquipantesByEvento(eventoId).Where(x => x.DataNascimento?.Month == data.Month).ToList();
        }

        public List<Equipante> GetEquipantesRestricoesByEvento(int eventoId)
        {
            return GetEquipantesByEvento(eventoId).Where(x => x.HasRestricaoAlimentar).ToList();
        }

        public List<EquipanteEvento> GetEquipantesEvento(int eventoId)
        {
            return equipanteEventoRepository
                .GetAll()
                .Include(x => x.Equipante)
                .Include(x => x.Equipe)
                .Where(x => x.EventoId == eventoId).ToList();
        }

        public IQueryable<Equipe> GetEquipes(int eventoId)
        {
            var evento = eventosBusiness.GetEventoById(eventoId);
            return conifgEquipeRepo.GetAll(x => x.ConfiguracaoId == evento.ConfiguracaoId).Select(x => x.Equipe);
        }

        public IQueryable<Equipe> GetEquipesConfig()
        {
            return equipeRepository.GetAll();
        }

        public void PostEquipe(PostEquipeModel model)
        {
            Data.Entities.Equipe equipe = null;

            if (model.Id > 0)
            {
                equipe = equipeRepository.GetById(model.Id);

                equipe.Nome = model.Nome;

                equipeRepository.Update(equipe);
            }
            else
            {
                equipe = new Data.Entities.Equipe
                {
                    Nome = model.Nome,
                   
                };

                equipeRepository.Insert(equipe);
            }

            equipeRepository.Save();
        }

        public void DeleteEquipe(int id)
        {
            equipeRepository.Delete(id);
            equipeRepository.Save();
        }
    }
}
