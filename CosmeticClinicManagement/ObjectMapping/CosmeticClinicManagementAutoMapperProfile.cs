using AutoMapper;
using CosmeticClinicManagement.Domain.ClinicManagement;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Pages.TreatmentPlan;
using CosmeticClinicManagement.Services.Dtos;
using CosmeticClinicManagement.Services.Dtos.Sessions;
using System.Collections.Generic;

namespace CosmeticClinicManagement.ObjectMapping;

public class CosmeticClinicManagementAutoMapperProfile : Profile
{
    public CosmeticClinicManagementAutoMapperProfile()
    {
        CreateMap<PatientDto, Patient>();
        CreateMap<Patient, PatientDto>();
        CreateMap<TreatmentPlan,TreatmentPlanDto > ();
        CreateMap< Session , SessionsLookupDto>();
        CreateMap<SessionDto, Session>();
        CreateMap<Session, SessionDto>();
        CreateMap<CreateUpdateTreatmentPlanDto, TreatmentPlan>();
        CreateMap<CreateUpdateSessionDto, Session>();
        CreateMap<Session, CreateUpdateSessionDto>();
        CreateMap<CreateEditTreatmentPlanViewModel, CreateUpdateTreatmentPlanDto>();
        CreateMap<TreatmentPlanDto, CreateEditTreatmentPlanViewModel>();

    }
}
