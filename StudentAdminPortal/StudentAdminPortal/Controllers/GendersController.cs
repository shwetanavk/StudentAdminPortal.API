using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;
using StudentAdminPortal.DomainModels;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class GendersController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public GendersController(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }


        //public IStudentRepository StudentRepository { get; }


        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllGenders()
        {
            var genderList = await studentRepository.GetGendersAsync();
            if (genderList == null || !genderList.Any())
                return NotFound();

            var domainModelGender = mapper.Map<List<Gender>>(genderList);

            return Ok(domainModelGender);
        }
    }
}
