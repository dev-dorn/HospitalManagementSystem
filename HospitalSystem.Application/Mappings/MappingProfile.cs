public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreatePatientCommand, Patient>();
        CreateMap<Patient, PatientDto>();
    }
}
