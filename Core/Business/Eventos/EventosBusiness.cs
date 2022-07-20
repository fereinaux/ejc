using Core.Models.Eventos;
using Data.Entities;
using Data.Repository;
using System.Linq;
using System.Data.Entity;
using Utils.Enums;

namespace Core.Business.Eventos
{
    public class EventosBusiness : IEventosBusiness
    {
        private readonly IGenericRepository<Evento> eventoRepository;

        public EventosBusiness(IGenericRepository<Evento> eventoRepository)
        {
            this.eventoRepository = eventoRepository;
        }

        public void DeleteEvento(int id)
        {
            eventoRepository.Delete(id);
            eventoRepository.Save();
        }

        public Evento GetEventoById(int id)
        {
            return eventoRepository.GetAll(x => x.Id == id).Include(x => x.Configuracao).FirstOrDefault();
        }

        public IQueryable<Evento> GetEventos()
        {
            return eventoRepository.GetAll().Include(x => x.Configuracao);
        }

        public void PostEvento(PostEventoModel model)
        {
            Evento evento = null;

            if (model.Id > 0)
            {
                evento = eventoRepository.GetById(model.Id);

                evento.DataEvento = model.DataEvento.AddHours(5);
                evento.Capacidade = model.Capacidade;
                evento.ConfiguracaoId = model.ConfiguracaoId;
                evento.Descricao = model.Descricao;
                evento.Numeracao = model.Numeracao;
                evento.Valor = model.Valor;
                evento.ValorTaxa = model.ValorTaxa;

                eventoRepository.Update(evento);
            }
            else
            {
                evento = new Evento
                {
                    DataEvento = model.DataEvento.AddHours(5),
                    Descricao = model.Descricao,
                    Capacidade = model.Capacidade,
                    Valor = model.Valor,
                    Numeracao = model.Numeracao,
                    ValorTaxa = model.ValorTaxa,
                   ConfiguracaoId = model.ConfiguracaoId,
                    Status = StatusEnum.Encerrado 
           
                };

                eventoRepository.Insert(evento);
            }

            eventoRepository.Save();
        }

        public bool ToggleEventoStatus(int id)
        {
            Evento evento = eventoRepository.GetById(id);

            StatusEnum status = evento.Status == StatusEnum.Aberto ?
                StatusEnum.Encerrado :
                StatusEnum.Aberto;

            evento.Status = status;

            eventoRepository.Update(evento);

            eventoRepository.Save();

            return true;
        }
    }
}
