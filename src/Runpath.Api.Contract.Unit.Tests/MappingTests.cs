using System;
using AutoMapper;
using Runpath.Api.Contract.Models;
using Runpath.Api.Contract.Models.Consume;
using Xunit;

namespace Runpath.Api.Contract.Unit.Tests
{
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _mapper = new MapperConfiguration(mappings =>
            {
                mappings.CreateMissingTypeMaps = false;
                mappings.AddProfile<MappingProfiles>();
            }).CreateMapper();
        }

        [Fact]
        public void Should_Map_AlbumConsume_To_Album()
        {
            var albumConsume = new AlbumConsume
            {
                Id = 1,
                Title = "Test",
                UserId = 1
            };

            var album = _mapper.Map<Album>(albumConsume);

            Assert.Equal(albumConsume.Id, album.Id);
            Assert.Equal(albumConsume.Title, album.Title);
            Assert.Equal(albumConsume.UserId, album.UserId);
        }

        [Fact]
        public void Should_Map_PhotoConsume_To_Photo()
        {
            var photoConsume = new PhotoConsume
            {
                Id = 1,
                Title = "Test",
                AlbumId = 2,
                ThumbnailUrl = "EmptyThumb",
                Url = "EmptyUrl"
            };

            var photo = _mapper.Map<Photo>(photoConsume);

            Assert.Equal(photoConsume.Id, photo.Id);
            Assert.Equal(photoConsume.AlbumId, photo.AlbumId);
            Assert.Equal(photoConsume.Title, photo.Title);
            Assert.Equal(photoConsume.ThumbnailUrl, photo.ThumbnailUrl);
            Assert.Equal(photoConsume.Url, photo.Url);
        }
    }
}
