using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.Repositories;
using StudentAdminPortal.DomainModels;
using StudentAdminPortal.Repositories;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;
        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await studentRepository.GetStudentsAsync();



            //var domainModelStudents = new List<Student>();

            //foreach(var stu in students)
            //{
            //    domainModelStudents.Add(new Student
            //    {
            //        ID = stu.ID,
            //        FirstName = stu.FirstName,
            //        LastName = stu.LastName,
            //        DateOfBirth = stu.DateOfBirth,
            //        Email = stu.Email,
            //        Mobile = stu.Mobile,
            //        ProfileImageUrl = stu.ProfileImageUrl,
            //        GenderId = stu.GenderId,
            //        Address = new Address()
            //        {
            //            Id = stu.Address.Id,
            //            PhysicalAddress = stu.Address.PhysicalAddress,
            //            PostalAddress = stu.Address.PostalAddress,

            //        },
            //        Gender = new Gender()
            //        {
            //            ID = stu.Gender.ID,
            //            Description = stu.Gender.Description
            //        }
            //    }); ;
            //}

            var domainModelStudents = mapper.Map<List<Student>>(students);
        
            return Ok(domainModelStudents);
        }

        [HttpGet]

        //[Route("[controller]/{studentId:guid}")]
        [Route("[controller]/{studentId:guid}"), ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentsAsync([FromRoute] Guid studentId)
        {
            var student = await studentRepository.GetStudentAsync(studentId);

            if (student == null)
                return NotFound();
            else
                return Ok(mapper.Map<Student>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if(await studentRepository.Exists(studentId))
            {
                var UpdatedStudent = await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));
            
                if(UpdatedStudent != null)
                {
                    return Ok(mapper.Map<Student>(UpdatedStudent));
                }
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await studentRepository.Exists(studentId))
            {
                var student = await studentRepository.DeleteStudent(studentId);
                return Ok(mapper.Map<Student>(student));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var student = await studentRepository.AddStudent(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudentsAsync), new { studentId = student.ID },
                mapper.Map<Student>(student));
        }


        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
               ".jpeg",
               ".png",
               ".gif",
               ".jpg"
            };

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    if (await studentRepository.Exists(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await imageRepository.Upload(profileImage, fileName);

                        if (await studentRepository.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }
                }

                return BadRequest("This is not a valid Image format");
            }

            return NotFound();
        }
    }
}

