using System.Collections.Generic;
using System.Threading.Tasks;
using Runpath.Api.Contract.Models.Consume;

namespace Runpath.Api.Contract.Interfaces.Services
{
    public interface IAlbumsApiClient
    {
        Task<IEnumerable<AlbumConsume>> GetAlbums();
        Task<IEnumerable<PhotoConsume>> GetPhotos();
        Task<IEnumerable<AlbumConsume>> GetAlbumsByUserId(int userId);
        Task<IEnumerable<PhotoConsume>> GetPhotosByAlbumIds(IList<int> albumIds);
    }
}
