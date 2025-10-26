using AutoMapper;
using CosmeticClinicManagement.Domain.PatientAggregateRoot;
using CosmeticClinicManagement.Services.Dtos;

namespace CosmeticClinicManagement.ObjectMapping;

public class CosmeticClinicManagementAutoMapperProfile : Profile
{
    public CosmeticClinicManagementAutoMapperProfile()
    {
        CreateMap<PatientDto, Patient>();
        CreateMap<Patient, PatientDto>();
    }
}
