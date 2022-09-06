
using AutoMapper;
using WepApiWithToken.Model.Entities;
using WepApiWithToken.Model.Users;
using WepApiWithToken.ViewModels;

namespace WepApiWithToken.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Get Method
            CreateMap<Address, AddressDto>();
            CreateMap<Student, StudentDto>();
            CreateMap<Maintenance, MaintenanceDto>();
            CreateMap<Course, CourseTypeDto>();
            CreateMap<CleaningService, CleaningServiceDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<Gender, GenderDto>();
            CreateMap<StatusType, StatusTypeDto>();
            CreateMap<MaintenanceType, MaintenanceTypeDto>();
            CreateMap<Manager, ManagerDto>();
            CreateMap<ServiceManagement, ServiceManagerDto>();
            CreateMap<CleaningType, CleaningTypeDto>();
            // Post Method
            CreateMap<ServiceManagerDto, ServiceManagement>();
            CreateMap<ManagerDto, Manager>();
            CreateMap<StudentDto, Student>();
            CreateMap<AddressDto, Address>();
            CreateMap<MaintenanceDto, Maintenance>();
            CreateMap<CourseTypeDto, Course>();
            CreateMap<CleaningServiceDto, CleaningService>();
            CreateMap<RoomDto, Room>();
            CreateMap<GenderDto, Gender>();
            CreateMap<MaintenanceTypeDto, MaintenanceType>();
            CreateMap<StatusTypeDto, StatusType>();
            CreateMap<CleaningTypeDto, CleaningType>();
        }
    }
}
