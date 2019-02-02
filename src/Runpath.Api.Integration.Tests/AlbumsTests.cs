using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Runpath.Api.Contract.Models;
using Xunit;

namespace Runpath.Api.Integration.Tests
{
    public class AlbumsTests
    {
        [Fact]
        public async Task Should_Return_All_Albums()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/albums");
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Should_Return_Album_Based_User_Id()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/albums/1");
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var album = JsonConvert.DeserializeObject<IEnumerable<Album>>(response.Content.ReadAsStringAsync().Result);

                Assert.NotEmpty(album);
            }
        }

        [Fact]
        public async Task Should_Return_NoContent_When_UserId_does_not_exist()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/albums/6666");
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_UserId_Is_Negative()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/albums/-1");
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_UserId_Is_Not_Number()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/albums/as");
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}