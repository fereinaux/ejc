using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Business.Eventos;
using Core.Models.Lancamento;
using Data.Entities;
using Data.Repository;
using Utils.Enums;

namespace Core.Business.Lancamento
{
    public class LancamentoBusiness : ILancamentoBusiness
    {
        private readonly IGenericRepository<Data.Entities.Lancamento> lancamentoRepository;
        private readonly IGenericRepository<Data.Entities.Participante> participanteRepository;
        private readonly IGenericRepository<Data.Entities.Equipante> equipanteRepository;
        private readonly IEventosBusiness eventosBusiness;

        public LancamentoBusiness(IEventosBusiness eventosBusiness, IGenericRepository<Data.Entities.Lancamento> lancamentoRepository, IGenericRepository<Data.Entities.Participante> participanteRepository, IGenericRepository<Data.Entities.Equipante> equipanteRepository)
        {
            this.lancamentoRepository = lancamentoRepository;
            this.equipanteRepository = equipanteRepository;
            this.participanteRepository = participanteRepository;
            this.eventosBusiness = eventosBusiness;
        }

        public IQueryable<Data.Entities.Lancamento> GetPagamentosParticipante(int participanteId)
        {
            var participante = participanteRepository.GetById(participanteId);
            return lancamentoRepository.GetAll(x => x.ParticipanteId == participanteId && x.EventoId == participante.EventoId)
                .Include(x => x.CentroCusto)
                .Include(x => x.Participante)
                .Include(x => x.Evento)
                .Include(x => x.MeioPagamento);
        }

        public void PostPagamento(PostPagamentoModel model)
        {
            string descricao = "";
            int centCusto = 0;
            var evento = eventosBusiness.GetEventoById(model.EventoId);

            if (model.ParticipanteId.HasValue)
            {
                Participante participante = participanteRepository.GetById(model.ParticipanteId.Value);

                descricao = $"Inscrição {participante.Nome}";
                centCusto = evento.Configuracao.CentroCustoInscricaoId.Value;

                participante.Status = StatusEnum.Confirmado;
                participanteRepository.Update(participante);
                participanteRepository.Save();

            }

            if (model.EquipanteId.HasValue)
            {
                Equipante equipante = equipanteRepository.GetById(model.EquipanteId.Value);

                descricao = $"Taxa de Equipante {equipante.Nome}";
                centCusto = evento.Configuracao.CentroCustoTaxaId.Value;
            }

            Data.Entities.Lancamento lancamento = new Data.Entities.Lancamento
            {
                Descricao = descricao,
                Origem = model.Origem,
                Valor = model.Valor,
                MeioPagamentoId = model.MeioPagamentoId,
                EventoId =model.EventoId,
                CentroCustoId = centCusto,
                ParticipanteId = model.ParticipanteId,
                EquipanteId = model.EquipanteId,
                Status = StatusEnum.Quitado,
                Tipo = TiposLancamentoEnum.Receber,
                DataCadastro = model.Data
            };

            lancamentoRepository.Insert(lancamento);
            lancamentoRepository.Save();
            if (model.Data.HasValue)
            {
                lancamento.DataCadastro = model.Data;
                lancamentoRepository.Update(lancamento);
                lancamentoRepository.Save();
            }

        }

        public void DeleteLancamento(int id)
        {
            Data.Entities.Lancamento oldLancamento = lancamentoRepository.GetById(id);

            lancamentoRepository.Delete(id);
            lancamentoRepository.Save();

            if (oldLancamento.ParticipanteId.HasValue && lancamentoRepository.GetAll(x => x.ParticipanteId == oldLancamento.ParticipanteId).Count() == 0)
            {
                Participante participante = participanteRepository.GetById(oldLancamento.ParticipanteId.Value);
                participante.Status = StatusEnum.Inscrito;
                participanteRepository.Update(participante);
                participanteRepository.Save();
            }
        }

        public IQueryable<Data.Entities.Lancamento> GetPagamentosEquipante(int equipanteId, int eventoId)
        {
            return lancamentoRepository.GetAll(x => x.EquipanteId == equipanteId && x.EventoId == eventoId)
                .Include(x => x.CentroCusto)
                .Include(x => x.Evento)
                .Include(x => x.Equipante)
                .Include(x => x.MeioPagamento);
        }

        public IQueryable<Data.Entities.Lancamento> GetPagamentosEvento(int eventoId)
        {
            return lancamentoRepository.GetAll(x => x.EventoId == eventoId).Include(x => x.MeioPagamento)
                .Include(x => x.Evento)
                .Include(x => x.MeioPagamento);
        }

        public IQueryable<Data.Entities.Lancamento> GetLancamentos()
        {
            return lancamentoRepository.GetAll().Include(x => x.CentroCusto)
                .Include(x => x.Evento)
                .Include(x => x.MeioPagamento);
        }

        public void PostLancamento(PostLancamentoModel model)
        {
            Data.Entities.Lancamento lancamento = null;

            if (model.Id > 0)
            {
                lancamento = GetLancamentoById(model.Id);
                lancamento.Origem = model.Origem;
                lancamento.Descricao = model.Descricao;
                lancamento.Observacao = model.Observacao;
                lancamento.Valor = model.Valor;
                lancamento.DataCadastro = model.Data;
                lancamento.MeioPagamentoId = model.MeioPagamentoId;
                lancamento.CentroCustoId = model.CentCustoId;

                lancamentoRepository.Update(lancamento);
            }
            else
            {
                lancamento = new Data.Entities.Lancamento
                {
                    Origem = model.Origem,
                    Descricao = model.Descricao,
                    Valor = model.Valor,
                    Observacao = model.Observacao,
                    MeioPagamentoId = model.MeioPagamentoId,
                    EventoId = model.EventoId,
                    CentroCustoId = model.CentCustoId,
                    Status = StatusEnum.Quitado,
                    Tipo = model.Tipo
                };

                lancamentoRepository.Insert(lancamento);
            }

            lancamentoRepository.Save();
        }

        public Data.Entities.Lancamento GetLancamentoById(int id)
        {
            return lancamentoRepository.GetById(id);
        }
    }
}
