using Core.Business.Eventos;
using Core.Models.Equipantes;
using Data.Entities;
using Data.Repository;
using System;
using System.Data.Entity;
using System.Linq;
using Utils.Enums;

namespace Core.Business.Equipantes
{
    public class EquipantesBusiness : IEquipantesBusiness
    {
        private readonly IEventosBusiness eventosBusiness;
        private readonly IGenericRepository<Equipante> equipanteRepository;
        private readonly IGenericRepository<EquipanteEvento> equipanteEventoRepository;
        private readonly IGenericRepository<ParticipantesEtiquetas> ParticipantesEtiquetasRepo;

        public EquipantesBusiness(IGenericRepository<Equipante> equipanteRepository, IEventosBusiness eventosBusiness, IGenericRepository<ParticipantesEtiquetas> ParticipantesEtiquetasRepo, IGenericRepository<EquipanteEvento> equipanteEventoRepository)
        {
            this.equipanteRepository = equipanteRepository;
            this.eventosBusiness = eventosBusiness;
            this.equipanteEventoRepository = equipanteEventoRepository;
            this.ParticipantesEtiquetasRepo = ParticipantesEtiquetasRepo;
        }

        public void DeleteEquipante(int id)
        {
            equipanteRepository.Delete(id);
            equipanteRepository.Save();
        }

        public Equipante GetEquipanteById(int id)
        {

            return equipanteRepository.GetAll(x => x.Id == id).Include(x => x.ParticipantesEtiquetas).Include(x => x.ParticipantesEtiquetas.Select(y => y.Etiqueta)).FirstOrDefault();
        }

        public IQueryable<Equipante> GetEquipantes()
        {
            return equipanteRepository
                .GetAll();
        }

        public Equipante PostEquipante(PostEquipanteModel model)
        {
            Equipante equipante = null;

            if (model.Id > 0)
            {
                equipante = equipanteRepository.GetById(model.Id);

                equipante.Nome = model.Nome;
                equipante.Apelido = model.Apelido;
                equipante.DataNascimento = model.DataNascimento?.AddHours(5);
                equipante.Fone = model.Fone;
                equipante.Email = model.Email;
                equipante.HasAlergia = model.HasAlergia;
                equipante.Alergia = model.HasAlergia ? model.Alergia : null;
                equipante.HasMedicacao = model.HasMedicacao;
                equipante.Medicacao = model.HasMedicacao ? model.Medicacao : null;
                equipante.HasRestricaoAlimentar = model.HasRestricaoAlimentar;
                equipante.RestricaoAlimentar = model.HasRestricaoAlimentar ? model.RestricaoAlimentar : null;
                equipante.Sexo = model.Sexo;
                equipante.CEP = model.CEP;
                equipante.Logradouro = model.Logradouro;
                equipante.Bairro = model.Bairro;
                equipante.Cidade = model.Cidade;
                equipante.Estado = model.Estado;
                equipante.Numero = model.Numero;
                equipante.Complemento = model.Complemento;
                equipante.Referencia = model.Referencia;
                equipante.Latitude = model.Latitude;
                equipante.Longitude = model.Longitude;
                equipante.HasVacina = model.HasVacina;

                ParticipantesEtiquetasRepo.GetAll(x => x.EquipanteId == model.Id).ToList().ForEach(etiqueta => ParticipantesEtiquetasRepo.Delete(etiqueta.Id));
                if (model.Etiquetas != null)
                {
                    foreach (var etiqueta in model.Etiquetas)
                    {
                        ParticipantesEtiquetasRepo.Insert(new ParticipantesEtiquetas { EquipanteId = model.Id, EventoId = model.EventoId, EtiquetaId = Int32.Parse(etiqueta) });
                    }

                }
                ParticipantesEtiquetasRepo.Save();

                equipanteRepository.Update(equipante);
            }
            else
            {
                equipante = new Equipante
                {
                    Nome = model.Nome,
                    Apelido = model.Apelido,
                    DataNascimento = model.DataNascimento?.AddHours(5),
                    Fone = model.Fone,
                    Email = model.Email,
                    Status = StatusEnum.Ativo,
                    HasAlergia = model.HasAlergia,
                    Alergia = model.HasAlergia ? model.Alergia : null,
                    HasMedicacao = model.HasMedicacao,
                    Medicacao = model.HasMedicacao ? model.Medicacao : null,
                    HasRestricaoAlimentar = model.HasRestricaoAlimentar,
                    RestricaoAlimentar = model.HasRestricaoAlimentar ? model.RestricaoAlimentar : null,
                    Sexo = model.Sexo,
                    CEP = model.CEP,
                    Logradouro = model.Logradouro,
                    Bairro = model.Bairro,
                    Cidade = model.Cidade,
                    Estado = model.Estado,
                    Numero = model.Numero,
                    Complemento = model.Complemento,
                    Referencia = model.Referencia,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                };

                equipanteRepository.Insert(equipante);
            }

            equipanteRepository.Save();
            return equipante;
        }

        public void ToggleSexo(int id)
        {
            var equipante = GetEquipanteById(id);
            equipante.Sexo = equipante.Sexo == SexoEnum.Feminino ? SexoEnum.Masculino : SexoEnum.Feminino;
            equipanteRepository.Update(equipante);
            equipanteRepository.Save();
        }

        public void ToggleVacina(int id)
        {
            var equipante = GetEquipanteById(id);
            equipante.HasVacina = !equipante.HasVacina;
            equipanteRepository.Update(equipante);
            equipanteRepository.Save();
        }

        public void ToggleTeste(int id)
        {
            var equipante = GetEquipanteById(id);
            equipante.HasTeste = !equipante.HasTeste;
            equipanteRepository.Update(equipante);
            equipanteRepository.Save();
        }

        public void ToggleCheckin(int id, int eventoid)
        {
            var equipante = equipanteEventoRepository.GetAll(x => x.EventoId == eventoid && x.EquipanteId == id).FirstOrDefault();
            equipante.Checkin = !equipante.Checkin;
            equipanteEventoRepository.Update(equipante);
            equipanteEventoRepository.Save();
        }

        public void PostEtiquetas(string[] etiquetas, int id, string obs, int? eventoId)
        {
            Equipante equipante = equipanteRepository.GetById(id);

            ParticipantesEtiquetasRepo.GetAll(x => x.EquipanteId == id && x.EventoId == eventoId).ToList().ForEach(etiqueta => ParticipantesEtiquetasRepo.Delete(etiqueta.Id));
            if (etiquetas != null)
            {
                foreach (var etiqueta in etiquetas)
                {
                    ParticipantesEtiquetasRepo.Insert(new ParticipantesEtiquetas { EquipanteId = id, EventoId = eventoId, EtiquetaId = Int32.Parse(etiqueta) });
                }

            }
            ParticipantesEtiquetasRepo.Save();
            if (!string.IsNullOrEmpty(obs))
            {
                equipante.Observacao = obs;
            }
            equipanteRepository.Update(equipante);
            equipanteRepository.Save();
        }
    }
}
