using Core.Models.Padrinhos;
using Core.Models.Quartos;
using System.Collections.Generic;
using System.Linq;
using Utils.Enums;

namespace Core.Business.Padrinhos
{
    public interface IPadrinhosBusiness
    {
        List<Data.Entities.Participante> GetParticipantesSemPadrinho(int eventoId);
        IQueryable<Data.Entities.Padrinho> GetPadrinhos();
        IQueryable<Data.Entities.Participante> GetParticipantesByPadrinhos(int id);
        IQueryable<Data.Entities.Participante> GetPadrinhosComParticipantes(int eventoId);
        Data.Entities.Padrinho GetPadrinhoById(int id);
        void PostPadrinho(PostPadrinhoModel model);
        void DeletePadrinho(int id);
        Data.Entities.Padrinho GetNextPadrinho(int eventoId);
        void DistribuirPadrinhos(int eventoId);
        void ChangePadrinho(int participanteId, int? destinoId);
    }
}
