using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(Guid StudentId);
        Task<List<Gender>> GetGendersAsync();
        Task<Boolean> Exists(Guid StudentId);
        Task<Student> UpdateStudent(Guid StudentId, Student request);
        Task<Student> DeleteStudent(Guid studentId);

        Task<Student> AddStudent(Student request); 
        Task<bool> UpdateProfileImage(Guid StudentId, string profileImageUrl); 
    }
}
