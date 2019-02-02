using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Runpath.Api.Contract;
using Runpath.Api.Contract.Interfaces.Services;
using Runpath.Api.Contract.Models.Consume;
using Xunit;

namespace Runpath.Api.BLL.Unit.Tests
{
    public class AlbumsServiceTests
    {
        private readonly IMapper _mapper;
        public AlbumsServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task Should_Return_Merged_Albums_And_Photos_When_Calling_GetAlbumsWithPhotos()
        {
            var albumsApiService = new Mock<IAlbumsApiClient>();
            albumsApiService.Setup(x => x.GetAlbums()).ReturnsAsync(new List<AlbumConsume>()
            {
                new AlbumConsume{Id = 1, Title = "Test", UserId = 1}
            });
            albumsApiService.Setup(x => x.GetPhotos()).ReturnsAsync(new List<PhotoConsume>()
            {
                new PhotoConsume() { Id = 1, AlbumId = 1, Title = "Test Photo", ThumbnailUrl = "something", Url = "something"}
            });

            var albumsService = new AlbumsService(albumsApiService.Object, _mapper);

            var albums = await albumsService.GetAlbumsWithPhotos();

            Assert.Single(albums);
            Assert.Collection(albums, album => Assert.Single(album.Photos));
        }

        [Fact]
        public async Task Should_Return_Merged_Albums_And_Photos_When_Calling_GetAlbumsWithPhotosByUserId()
        {
            var albumsApiService = new Mock<IAlbumsApiClient>();
            albumsApiService.Setup(x => x.GetAlbumsByUserId(1)).ReturnsAsync(new List<AlbumConsume>()
            {
                new AlbumConsume{Id = 1, Title = "Test", UserId = 1},
            });
            albumsApiService.Setup(x => x.GetPhotosByAlbumIds(new List<int> { 1 })).ReturnsAsync(new List<PhotoConsume>()
            {
                new PhotoConsume() { Id = 1, AlbumId = 1, Title = "Test Photo", ThumbnailUrl = "something", Url = "something"},
            });

            var albumsService = new AlbumsService(albumsApiService.Object, _mapper);

            var albums = await albumsService.GetAlbumsWithPhotosByUserId(1);

            Assert.Single(albums);
            Assert.Collection(albums, album => Assert.Single(album.Photos));
        }
    }
}
