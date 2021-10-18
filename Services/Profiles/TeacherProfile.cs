using AutoMapper;
using DAL.Entities.Teachers;
using Model.Teacher.Inputs;
using Model.Teacher.Outputs;

namespace Services.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<TeacherUpdateInput, Teacher>();
            CreateMap<Teacher, TeacherOutput>();
        }
    }
}