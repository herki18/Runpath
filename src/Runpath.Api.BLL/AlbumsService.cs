using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Runpath.Api.Contract.Interfaces.Services;
using Runpath.Api.Contract.Models;

namespace Runpath.Api.BLL
{
    public class AlbumsService : IAlbumsService
    {
        private readonly IAlbumsApiClient _consumeService;
        private readonly IMapper _mapper;

        public AlbumsService(IAlbumsApiClient consumeService, IMapper mapper)
        {
            _consumeService = consumeService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Album>> GetAlbumsWithPhotos()
        {
            // Get Albums
            var consumeAlbums = await _consumeService.GetAlbums();
            // Get Photos
            var consumePhotos = await _consumeService.GetPhotos();
            
            var albums = _mapper.Map<ICollection<Album>>(consumeAlbums);
            var photos = _mapper.Map<ICollection<Photo>>(consumePhotos);

            return MergeAlbumsAndPhotos(albums, photos);
        }

        public async Task<IEnumerable<Album>> GetAlbumsWithPhotosByUserId(int userId)
        {
            // Get Albums
            var consumeAlbums = await _consumeService.GetAlbumsByUserId(userId);
            // Get Photos
            var consumePhotos = await _consumeService.GetPhotosByAlbumIds(consumeAlbums.Select(album => album.Id).ToList());
            
            var albums = _mapper.Map<ICollection<Album>>(consumeAlbums);
            var photos = _mapper.Map<ICollection<Photo>>(consumePhotos);
            
            return MergeAlbumsAndPhotos(albums, photos);
        }

        private ICollection<Album> MergeAlbumsAndPhotos(ICollection<Album> albums, ICollection<Photo> photos)
        {
            foreach (var album in albums)
            {
                album.Photos = photos.Where(photo => photo.AlbumId == album.Id);
            }

            return albums;
        }
    }
}