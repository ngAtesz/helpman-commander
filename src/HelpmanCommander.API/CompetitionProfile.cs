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
            CreateMap<Exercise, ExerciseModel>().ReverseMap();
            CreateMap<Task, TaskModel>().ReverseMap();
        }
    }
}
