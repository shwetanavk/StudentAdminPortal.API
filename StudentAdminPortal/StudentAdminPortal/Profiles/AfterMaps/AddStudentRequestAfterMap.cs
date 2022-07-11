using StudentAdminPortal.DomainModels;
using AutoMapper;

namespace StudentAdminPortal.Profiles.AfterMaps
{
    public class AddStudentRequestAfterMap : IMappingAction<AddStudentRequest, API.DataModels.Student>
    {
        public void Process(AddStudentRequest source, API.DataModels.Student destination, ResolutionContext context)
        {
            destination.ID = Guid.NewGuid();
            destination.Address = new API.DataModels.Address()
            {
                Id = Guid.NewGuid(),
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress = source.PostalAddress
            };
        }
    }
}
