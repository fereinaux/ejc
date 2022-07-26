using Core.Models.MeioPagamento;
using Data.Repository;
using System.Linq;
using Utils.Enums;

namespace Core.Business.MeioPagamento
{
    public class MeioPagamentoBusiness : IMeioPagamentoBusiness
    {
        private readonly IGenericRepository<Data.Entities.MeioPagamento> meioPagamentoRepository;

        public MeioPagamentoBusiness(IGenericRepository<Data.Entities.MeioPagamento> meioPagamentoRepository)
        {
            this.meioPagamentoRepository = meioPagamentoRepository;
        }

        public void DeleteMeioPagamento(int id)
        {
            meioPagamentoRepository.Delete(id);
            meioPagamentoRepository.Save();
        }

        public Data.Entities.MeioPagamento GetMeioPagamentoById(int id)
        {
            return meioPagamentoRepository.GetById(id);
        }

        public IQueryable<Data.Entities.MeioPagamento> GetMeioPagamentos(int configuracaoId)
        {
            return meioPagamentoRepository.GetAll(x => x.ConfiguracaoId == configuracaoId);
        }

        public void PostMeioPagamento(PostMeioPagamentoModel model)
        {
            Data.Entities.MeioPagamento meioPagamento = null;

            if (model.Id > 0)
            {
                meioPagamento = meioPagamentoRepository.GetById(model.Id);

                meioPagamento.Descricao = model.Descricao;
                meioPagamento.ConfiguracaoId = model.ConfiguracaoId;

                meioPagamentoRepository.Update(meioPagamento);
            }
            else
            {
                meioPagamento = new Data.Entities.MeioPagamento
                {
                    Descricao = model.Descricao,
                    ConfiguracaoId = model.ConfiguracaoId,
                    Status = StatusEnum.Ativo
                };

                meioPagamentoRepository.Insert(meioPagamento);
            }

            meioPagamentoRepository.Save();
        }
    }
}
