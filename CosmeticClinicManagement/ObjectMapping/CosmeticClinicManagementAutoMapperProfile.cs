using AutoMapper;
using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Pages.Sessions;
using CosmeticClinicManagement.Pages.TreatmentPlan;
using CosmeticClinicManagement.Services.Dtos;
using System.Collections.Generic;

namespace CosmeticClinicManagement.ObjectMapping;

public class CosmeticClinicManagementAutoMapperProfile : Profile
{
    public CosmeticClinicManagementAutoMapperProfile()
    {
        CreateMap<TreatmentPlan, TreatmentPlanDto>();
        //.ForMember(dest => dest.DoctorName,
        //      opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : null)); 
 
        CreateMap<SessionDto, Session>().ReverseMap();
       
        CreateMap<CreateUpdateTreatmentPlanDto, TreatmentPlan>();
        CreateMap<CreateUpdateSessionDto, Session>().ReverseMap();
        CreateMap<CreateEditTreatmentPlanViewModel, CreateUpdateTreatmentPlanDto>();
        CreateMap<CreateEditSessionViewModel, CreateUpdateSessionDto>();

        CreateMap<TreatmentPlanDto, CreateEditTreatmentPlanViewModel>();
        CreateMap<SessionDto, CreateEditSessionViewModel>();
        CreateMap<CreateUpdateUsedMaterialDto, UsedMaterial>().ReverseMap();



        CreateMap<PatientDto, Patient>().ReverseMap();
        CreateMap<CreateUpdatePatientDto, Patient>().ReverseMap();
        CreateMap<PatientDto, CreateUpdatePatientDto>().ReverseMap();

    }
}
