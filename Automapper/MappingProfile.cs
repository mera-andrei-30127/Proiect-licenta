using AutoMapper;
using WebApplicationForDidacticPurpose.DAL.Models;
using WebApplicationForDidacticPurpose.MODELS.RegisterAndLoginModels;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.Homework;

namespace WebApplicationForDidacticPurpose.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HomeworkViewModel, HomeworkEntity>().ReverseMap();
            CreateMap<AttendeeEntity, LoginUserEntity>().ReverseMap();
        }
    }
}
