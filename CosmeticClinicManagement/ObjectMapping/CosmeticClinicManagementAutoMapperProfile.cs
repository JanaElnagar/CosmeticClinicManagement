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
        CreateMap<PatientDto, Patient>();
        CreateMap<Patient, PatientDto>();
        CreateMap<TreatmentPlan,TreatmentPlanDto > ();
 
        CreateMap<SessionDto, Session>().ReverseMap();
       
        CreateMap<CreateUpdateTreatmentPlanDto, TreatmentPlan>();
        CreateMap<CreateUpdateSessionDto, Session>().ReverseMap();
        CreateMap<CreateEditTreatmentPlanViewModel, CreateUpdateTreatmentPlanDto>();
        CreateMap<CreateEditSessionViewModel, CreateUpdateSessionDto>();

        CreateMap<TreatmentPlanDto, CreateEditTreatmentPlanViewModel>();
        CreateMap<SessionDto, CreateEditSessionViewModel>();
        CreateMap<CreateUpdateUsedMaterialDto, UsedMaterial>().ReverseMap();


    }
}
