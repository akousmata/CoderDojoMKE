using AutoMapper;
using CoderDojoMKE.Web.Models.Data;
using CoderDojoMKE.Web.Models.View;

namespace CoderDojoMKE.Models
{
    public static class ObjectMap
    {
        public static void MapModels()
        {
            Mapper.CreateMap<Event, EventViewModel>();
            Mapper.CreateMap<Event, EventSignupViewModel>();
            Mapper.CreateMap<Enrollment, EventEnrolleeViewModel>()
                .ForMember(dest => dest.PersonID, opt => opt.MapFrom(src => src.Enrollee.PersonID))
                .ForMember(dest => dest.EnrollerID, opt => opt.MapFrom(src => src.Enroller.PersonID))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Enrollee.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Enrollee.LastName));
        }
    }
}
