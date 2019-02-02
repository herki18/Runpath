using AutoMapper;
using Runpath.Api.Contract.Models;
using Runpath.Api.Contract.Models.Consume;

namespace Runpath.Api.Contract
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AlbumConsume, Album>();
            CreateMap<PhotoConsume, Photo>();
        }
    }
}
