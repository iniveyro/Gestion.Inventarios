using AutoMapper;
using server.Models;
using server.Models.DTOs.Aires;
using server.Models.DTOs.Audiovisuales;
using server.Models.DTOs.Equipo;
using server.Models.DTOs.Impresoras;
using server.Models.DTOs.Monitores;
using server.Models.DTOs.Pcs;

namespace server.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Equipo
            CreateMap<EquipoModel, DeleteEquipoModel>().ReverseMap();
            #endregion
            #region Impresora
            CreateMap<ImpresoraModel, GetImpresora>().ReverseMap();
            CreateMap<ImpresoraModel, CreateImpresoraModel>().ReverseMap();
            #endregion
            #region Audiovisual
            CreateMap<AudiovisualModel, CreateAudiovisualModel>().ReverseMap();
            CreateMap<AudiovisualModel, GetAudiovisuales>().ReverseMap();
            #endregion
            #region Monitor
            CreateMap<MonitorModel, CreateMonitorModel>().ReverseMap();
            CreateMap<MonitorModel, GetMonitores>().ReverseMap();
            #endregion
            #region Aires
            CreateMap<AAModel, CreateAAModel>().ReverseMap();
            CreateMap<AAModel, GetAires>().ReverseMap();
            #endregion
            #region Pc
            CreateMap<PcModel, CreatePcModel>().ReverseMap();
            CreateMap<PcModel, GetPcs>().ReverseMap();
            #endregion
        }
    }
}