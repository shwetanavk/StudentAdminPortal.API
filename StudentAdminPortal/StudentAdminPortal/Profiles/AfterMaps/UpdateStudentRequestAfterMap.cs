using AutoMapper;
using StudentAdminPortal.DomainModels;
using DataModels = StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.Profiles.AfterMaps
{
    public class UpdateStudentRequestAfterMap:IMappingAction<UpdateStudentRequest, DataModels.Student>
    {
        public void Process(UpdateStudentRequest source, DataModels.Student destination, ResolutionContext context)
        {
            destination.Address = new DataModels.Address()
            {
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress = source.PostalAddress
            };
        }
    }
}
