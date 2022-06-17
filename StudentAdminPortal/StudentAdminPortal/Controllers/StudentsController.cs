using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModel;
using StudentAdminPortal.API.Repositories;
using StudentAdminPortal.DomainModels;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("[controller]")]
        public IActionResult GetAllStudents()
        {
            var students = studentRepository.GetStudents();

            var domainModelStudents = new List<Student>();

            foreach(var stu in students)
            {
                domainModelStudents.Add(new Student
                {
                    ID = stu.ID,
                    FirstName = stu.FirstName,
                    LastName = stu.LastName,
                    DateOfBirth = stu.DateOfBirth,
                    Email = stu.Email,
                    Mobile = stu.Mobile,
                    ProfileImageUrl = stu.ProfileImageUrl,
                    GenderId = stu.GenderId,
                    Address = new Address()
                    {
                        Id = stu.Address.Id,
                        PhysicalAddress = stu.Address.PhysicalAddress,
                        PostalAddress = stu.Address.PostalAddress,

                    },
                    Gender = new Gender()
                    {
                        ID = stu.Gender.ID,
                        Description = stu.Gender.Description
                    }
                }); ;
            }
        
            return Ok(domainModelStudents);
        }
    }
}
