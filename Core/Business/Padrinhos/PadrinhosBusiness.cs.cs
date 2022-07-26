using Core.Models.Padrinhos;
using Core.Models.Quartos;
using Data.Entities;
using Data.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Utils.Enums;
using Utils.Services;

namespace Core.Business.Padrinhos
{
    public class PadrinhosBusiness : IPadrinhosBusiness
    {
        private readonly IGenericRepository<Padrinho> padrinhoRepository;
        private readonly IGenericRepository<Participante> participanteRepository;
        private readonly IGenericRepository<EquipanteEvento> equipanteEventoRepository;

        public PadrinhosBusiness(IGenericRepository<Participante> participanteRepository, IGenericRepository<EquipanteEvento> equipanteEventoRepository, IGenericRepository<Padrinho> padrinhoRepository)
        {
            this.padrinhoRepository = padrinhoRepository;
            this.participanteRepository = participanteRepository;
            this.equipanteEventoRepository = equipanteEventoRepository;
        }

        public void ChangePadrinho(int participanteId, int? destinoId)
        {
            var participante = participanteRepository.GetById(participanteId);


            participante.PadrinhoId = destinoId;

            participanteRepository.Update(participante);
            participanteRepository.Save();

        }

        public void DeletePadrinho(int id)
        {
            padrinhoRepository.Delete(id);
            padrinhoRepository.Save();

            participanteRepository.GetAll(x => x.PadrinhoId == id).ToList().ForEach(part =>
            {
                part.PadrinhoId = null;
                participanteRepository.Update(part);
            });
            participanteRepository.Save();
        }

        public void DistribuirPadrinhos(int eventoId)
        {
            List<Participante> listParticipantes = GetParticipantesSemPadrinho(eventoId);

            foreach (var participante in listParticipantes)
            {
                var padrinho = GetNextPadrinho(eventoId);

                if (padrinho != null)
                {
                    participante.PadrinhoId = padrinho.Id;
                    participanteRepository.Update(participante);
                    participanteRepository.Save();
                }
            }
        }

        public Padrinho GetPadrinhoById(int id)
        {
            return padrinhoRepository.GetById(id);
        }

        public IQueryable<Padrinho> GetPadrinhos()
        {
            return padrinhoRepository.GetAll().Include(x => x.Participantes).Include(x => x.EquipanteEvento).Include(x => x.EquipanteEvento.Equipante);
        }

        public IQueryable<Participante> GetPadrinhosComParticipantes(int eventoId)
        {
            return participanteRepository.GetAll(x => x.Padrinho.EquipanteEvento.EventoId == eventoId)
               .Include(x => x.Padrinho)
                .Include(x => x.Padrinho.EquipanteEvento)
                .Include(x => x.Padrinho.EquipanteEvento.Equipante);
        }

        public Padrinho GetNextPadrinho(int eventoId)
        {
            var query = padrinhoRepository
                .GetAll(x => x.EquipanteEvento.EventoId == eventoId)
                .Include(x => x.EquipanteEvento)
                .Include(x => x.Participantes)
                .Select(x => new
                {
                    Padrinho = x,
                    Qtd = x.Participantes.Count()
                })
                .ToList();

            return query.Any() ?
                query.OrderBy(x => x.Qtd)
                .FirstOrDefault().Padrinho
                : null;
        }

        public IQueryable<Participante> GetParticipantesByPadrinhos(int id)
        {
            return participanteRepository.GetAll(x => x.PadrinhoId == id)
                .Include(x => x.Padrinho)
                .Include(x => x.Padrinho.EquipanteEvento)
                .Include(x => x.Padrinho.EquipanteEvento.Equipante);
        }

        public List<Participante> GetParticipantesSemPadrinho(int eventoId)
        {

            var listParticipantes = participanteRepository
                 .GetAll(x => x.EventoId == eventoId && !x.PadrinhoId.HasValue)
                 .Where(x => x.Status == StatusEnum.Inscrito || x.Status == StatusEnum.Confirmado)
                 .OrderBy(x => x.DataCadastro)
                 .ToList();

            listParticipantes.ForEach(x => x.Nome = UtilServices.CapitalizarNome(x.Nome));
            listParticipantes.ForEach(x => x.Apelido = UtilServices.CapitalizarNome(x.Apelido));

            return listParticipantes;
        }

        public void PostPadrinho(PostPadrinhoModel model)
        {
            Padrinho padrinho = null;

            padrinho = new Padrinho
            {
                EquipanteEventoId = model.EquipanteEventoId
            };

            padrinhoRepository.Insert(padrinho);
            padrinhoRepository.Save();
        }
    }
}
