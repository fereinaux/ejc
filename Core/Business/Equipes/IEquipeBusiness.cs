using Core.Models.Equipe;
using Data.Entities;
using System.Collections.Generic;
using System.Linq;
using Utils.Enums;

namespace Core.Business.Equipes
{
    public interface IEquipesBusiness
    {
        IQueryable<Equipe> GetEquipes(int eventoId);
        IQueryable<Equipe> GetEquipesConfig();
        IQueryable<EquipanteEvento> GetMembrosEquipe(int eventoId, int equipeId);
        IQueryable<EquipanteEvento> GetMembrosEquipeDatatable(int eventoId, int equipeId);

        EquipanteEvento GetEquipanteEventoByUser(int eventoId, string userId);
        List<Data.Entities.Equipante> GetEquipantesEventoSemEquipe(int eventoId);
        List<Data.Entities.Equipante> GetEquipantesByEvento(int eventoId);
        List<Data.Entities.EquipanteEvento> GetEquipantesEvento(int eventoId);
        IQueryable<Data.Entities.EquipanteEvento> GetQueryEquipantesEvento(int eventoId);
        List<Data.Entities.Equipante> GetEquipantesAniversariantesByEvento(int eventoId);
        List<Data.Entities.Equipante> GetEquipantesRestricoesByEvento(int eventoId);

        void AddMembroEquipe(PostEquipeMembroModel model);
        string DeleteMembroEquipe(int id);
        void ToggleMembroEquipeTipo(int id);
        EquipanteEvento GetEquipeAtual(int eventoId, int equipeId);
        IQueryable<PresencaReuniao> GetPresenca(int reuniaoId);
        void TogglePresenca(int equipanteEventoId, int reuniaoId);

        void PostEquipe(PostEquipeModel model);
        void DeleteEquipe(int id);
    }
}
