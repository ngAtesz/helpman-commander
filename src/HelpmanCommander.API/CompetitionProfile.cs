using AutoMapper;
using HelpmanCommander.API.Models;
using HelpmanCommander.Data.Entities;
using Task = HelpmanCommander.Data.Entities.Task;

namespace HelpmanCommander.API
{
    public class CompetitionProfile : Profile
    {
        public CompetitionProfile()
        {
            CreateMap<Competition, CompetitionModel>().ReverseMap();
            CreateMap<Station, StationModel>().ReverseMap();
            CreateMap<Exercise, ExerciseModel>()
                .ReverseMap();
            CreateMap<Task, TaskModel>()
                .ReverseMap()
                .ForMember(t => t.Exercises, opt => opt.Ignore());
            
            CreateMap<ExerciseTask, TaskModel>()
                .ForMember(tm => tm.Id, opt => opt.MapFrom(et => et.Task.Id))
                .ForMember(tm => tm.DefaultScore, opt => opt.MapFrom(et => et.Task.DefaultScore))
                .ForMember(tm => tm.IsDefault, opt => opt.MapFrom(et => et.Task.IsDefault))
                .ForMember(tm => tm.Name, opt => opt.MapFrom(et => et.Task.Name))
                .ForMember(tm => tm.PrerequisiteTaskId, opt => opt.MapFrom(et => et.Task.PrerequisiteTaskId))
                .ReverseMap();
        }
    }
}
