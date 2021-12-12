using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities.CourseQuizes;
using DAL.Repositories;

namespace Services.Services
{
    public class QuizesService
    {
        private readonly IGenericRepository<QuizOptions> _quizOptionsRepository;
        private readonly IGenericRepository<SectionQuiz> _sectionQuizRepository;
        private readonly IGenericRepository<StudentAnswer> _studentAnswerRepository;
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;

        public QuizesService(IGenericRepository<QuizOptions> quizOptionsRepository, IGenericRepository<SectionQuiz> sectionQuizRepository, IGenericRepository<StudentAnswer> studentAnswerRepository, IIdentityRepository identityRepository, IMapper mapper)
        {
            _quizOptionsRepository = quizOptionsRepository;
            _sectionQuizRepository = sectionQuizRepository;
            _studentAnswerRepository = studentAnswerRepository;
            _identityRepository = identityRepository;
            _mapper = mapper;
        }
    }
    public interface IQuizesService
    {
        
    }
}