using System.Collections.Generic;
using System.Threading.Tasks;
using Runpath.Api.Contract.Models;

namespace Runpath.Api.Contract.Interfaces.Services
{
    public interface IAlbumsService
    {
        Task<IEnumerable<Album>> GetAlbumsWithPhotos();
        Task<IEnumerable<Album>> GetAlbumsWithPhotosByUserId(int userId);
    }
}
