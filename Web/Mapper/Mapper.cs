using Arquitetura.ViewModels;
using AutoMapper;
using Core.Business.Arquivos;
using Core.Business.Equipantes;
using Core.Business.Equipes;
using Core.Business.Eventos;
using Core.Business.Lancamento;
using Core.Business.MeioPagamento;
using Core.Business.Reunioes;
using Core.Models.Carona;
using Core.Models.Equipantes;
using Core.Models.Etiquetas;
using Core.Models.Eventos;
using Core.Models.Lancamento;
using Core.Models.Participantes;
using Core.Models.Quartos;
using Data.Entities;
using SysIgreja.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utils.Constants;
using Utils.Enums;
using Utils.Extensions;
using Utils.Services;

namespace SysIgreja.Controllers
{

    public class MapperRealidade
    {

        public IMapper mapper;

        public MapperRealidade()
        {
            var configuration = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Equipante, PostEquipanteModel>()
                .ForMember(dest => dest.EtiquetasList, opt => opt.MapFrom(x => x.ParticipantesEtiquetas.Select(y => y.Etiqueta)))
                .ForMember(dest => dest.Foto, opt => opt.MapFrom(x => x.Arquivos.Any(y => y.IsFoto) ? Convert.ToBase64String(x.Arquivos.FirstOrDefault(y => y.IsFoto).Conteudo) : ""));
                cfg.CreateMap<Carona, PostCaronaModel>().ForMember(dest => dest.Motorista, opt => opt.MapFrom(x => x.Motorista.Nome));
                cfg.CreateMap<Quarto, PostQuartoModel>().ForMember(dest => dest.Equipante, opt => opt.MapFrom(x => x.Equipante != null ? x.Equipante.Nome : ""));
                cfg.CreateMap<Evento, PostEventoModel>();
                cfg.CreateMap<Participante, ParticipanteSelectModel>();
                cfg.CreateMap<Etiqueta, PostEtiquetaModel>();
                cfg.CreateMap<Participante, ParticipanteExcelViewModel>()
                          .ForMember(dest => dest.HasVacina, opt => opt.MapFrom(x => x.HasVacina ? "Sim" : "Não"))
           .ForMember(dest => dest.Nome, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Nome)))
          .ForMember(dest => dest.Apelido, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Apelido)))
            .ForMember(dest => dest.Idade, opt => opt.MapFrom(x => UtilServices.GetAge(x.DataNascimento)))
            .ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(x => x.DataNascimento.HasValue ? x.DataNascimento.Value.ToString("dd/MM/yyyy") : ""))
            .ForMember(dest => dest.Sexo, opt => opt.MapFrom(x => x.Sexo.GetDescription()))
                         .ForMember(dest => dest.Circulo, opt => opt.MapFrom(x => x.Circulos.Any() ? x.Circulos.LastOrDefault().Circulo.Cor.GetDescription() : ""))
                                      .ForMember(dest => dest.Motorista, opt => opt.MapFrom(x => x.Caronas.Any() ? x.Caronas.LastOrDefault().Carona.Motorista.Nome : ""))
            .ForMember(dest => dest.Situacao, opt => opt.MapFrom(x => x.Status.GetDescription()));
                cfg.CreateMap<Participante, ParticipanteListModel>()
                                  .ForMember(dest => dest.Nome, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Nome)))
                 .ForMember(dest => dest.Apelido, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Apelido)))
                    .ForMember(dest => dest.Idade, opt => opt.MapFrom(x => UtilServices.GetAge(x.DataNascimento)))
                    .ForMember(dest => dest.QtdAnexos, opt => opt.MapFrom(x => x.Arquivos.Count()))
                    .ForMember(dest => dest.HasFoto, opt => opt.MapFrom(x => x.Arquivos.Any(y => y.IsFoto)))
                    .ForMember(dest => dest.HasContact, opt => opt.MapFrom(x => x.MsgFoto || x.MsgGeral || x.MsgVacina || x.MsgPagamento || !string.IsNullOrEmpty(x.Observacao)))
                    .ForMember(dest => dest.Sexo, opt => opt.MapFrom(x => x.Sexo.GetDescription()))
                    .ForMember(dest => dest.Padrinho, opt => opt.MapFrom(x => x.PadrinhoId.HasValue ? x.Padrinho.EquipanteEvento.Equipante.Nome : null))
                    .ForMember(dest => dest.Circulo, opt => opt.MapFrom(x => x.Circulos.Any() ? x.Circulos.LastOrDefault().Circulo.Cor.GetDescription() : ""))
                    .ForMember(dest => dest.Etiquetas, opt => opt.MapFrom(x => x.ParticipantesEtiquetas.Select(y => y.Etiqueta)))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.Status.GetDescription()));
                cfg.CreateMap<Equipante, EquipanteListModel>()
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Nome)))
                    .ForMember(dest => dest.Apelido, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Apelido)))
                    .ForMember(dest => dest.Etiquetas, opt => opt.MapFrom(x => x.ParticipantesEtiquetas.Select(y => y.Etiqueta)))
                    .ForMember(dest => dest.Idade, opt => opt.MapFrom(x => UtilServices.GetAge(x.DataNascimento)))
                    .ForMember(dest => dest.Sexo, opt => opt.MapFrom(x => x.Sexo.GetDescription()))
                    .ForMember(dest => dest.HasFoto, opt => opt.MapFrom(x => x.Arquivos.Any(y => y.IsFoto)))
                    .ForMember(dest => dest.QtdAnexos, opt => opt.MapFrom(x => x.Arquivos.Count()))
                    .ForMember(dest => dest.Faltas, opt => opt.MapFrom(x => x.Equipes.LastOrDefault().Evento.Reunioes.Count() - x.Equipes.LastOrDefault().Presencas.Count()))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.Status.GetDescription()))
                    .ForMember(dest => dest.HasOferta, opt => opt.MapFrom(x => x.Lancamentos.Any(y => y.CentroCustoId == y.Evento.Configuracao.CentroCustoTaxaId && y.EventoId == x.Equipes.LastOrDefault().EventoId)))
                    .ForMember(dest => dest.Equipe, opt => opt.MapFrom(x => (x.Equipes.Any() ? x.Equipes.LastOrDefault().Equipe.Nome : null)))
                    .ForMember(dest => dest.Checkin, opt => opt.MapFrom(x => x.Equipes.LastOrDefault().Checkin));
                cfg.CreateMap<EquipanteEvento, EquipanteListModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.EquipanteId))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Equipante.Nome)))
                .ForMember(dest => dest.Apelido, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Equipante.Apelido)))
                .ForMember(dest => dest.Etiquetas, opt => opt.MapFrom(x => x.Equipante.ParticipantesEtiquetas.Select(y => y.Etiqueta)))
                .ForMember(dest => dest.Idade, opt => opt.MapFrom(x => UtilServices.GetAge(x.Equipante.DataNascimento)))
                .ForMember(dest => dest.Sexo, opt => opt.MapFrom(x => x.Equipante.Sexo.GetDescription()))
                .ForMember(dest => dest.HasFoto, opt => opt.MapFrom(x => x.Equipante.Arquivos.Any(y => y.IsFoto)))
                .ForMember(dest => dest.QtdAnexos, opt => opt.MapFrom(x => x.Equipante.Arquivos.Count()))
                .ForMember(dest => dest.Faltas, opt => opt.MapFrom(x => x.Evento.Reunioes.Count() - x.Presencas.Count()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.Equipante.Status.GetDescription()))
                .ForMember(dest => dest.HasOferta, opt => opt.MapFrom(x => x.Equipante.Lancamentos.Any(y => y.CentroCustoId == y.Evento.Configuracao.CentroCustoTaxaId && y.EventoId == x.EventoId)))
                .ForMember(dest => dest.Equipe, opt => opt.MapFrom(x => (x.Equipe.Nome)))
                .ForMember(dest => dest.Checkin, opt => opt.MapFrom(x => x.Checkin));
                cfg.CreateMap<Equipante, EquipanteExcelModel>()
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Nome)))
                    .ForMember(dest => dest.Apelido, opt => opt.MapFrom(x => UtilServices.CapitalizarNome(x.Apelido)))
                    .ForMember(dest => dest.Idade, opt => opt.MapFrom(x => UtilServices.GetAge(x.DataNascimento)))
                    .ForMember(dest => dest.Sexo, opt => opt.MapFrom(x => x.Sexo.GetDescription()))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.Status.GetDescription()))
                    .ForMember(dest => dest.HasVacina, opt => opt.MapFrom(x => x.HasVacina ? "Sim" : "Não"))
                    .ForMember(dest => dest.HasOferta, opt => opt.MapFrom(x => x.Lancamentos.Any(y => y.CentroCustoId == y.Evento.Configuracao.CentroCustoTaxaId && x.Equipes.Select(z => z.EventoId).Contains(y.EventoId))))
                    .ForMember(dest => dest.Equipe, opt => opt.MapFrom(x => (x.Equipes.Any() ? x.Equipes.LastOrDefault().Equipe.Nome : null)));


            });
            mapper = configuration.CreateMapper();
        }
    }

}
