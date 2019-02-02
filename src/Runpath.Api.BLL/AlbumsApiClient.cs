using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Runpath.Api.Contract.Extensions;
using Runpath.Api.Contract.Interfaces.Services;
using Runpath.Api.Contract.Models.Consume;

namespace Runpath.Api.BLL
{
    public class AlbumsApiClient: IAlbumsApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<AlbumsApiClient> _logger;

        public AlbumsApiClient(HttpClient client, ILogger<AlbumsApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<AlbumConsume>> GetAlbums()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "albums");

            using (var response = _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                CheckStatusCode(response);

                var stream = await response.Result.Content.ReadAsStreamAsync();
                var albums = stream.ReadAndDeserializeFromJson<IEnumerable<AlbumConsume>>();
                return albums;
            }
        }

        public async Task<IEnumerable<PhotoConsume>> GetPhotos()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "photos");

            using (var response = _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                CheckStatusCode(response);

                var stream = await response.Result.Content.ReadAsStreamAsync();
                var photos = stream.ReadAndDeserializeFromJson<IEnumerable<PhotoConsume>>();
                return photos;
            }
        }

        public async Task<IEnumerable<AlbumConsume>> GetAlbumsByUserId(int userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"albums?userId={userId}");

            using (var response = _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                CheckStatusCode(response);

                var stream = await response.Result.Content.ReadAsStreamAsync();
                var albums = stream.ReadAndDeserializeFromJson<IEnumerable<AlbumConsume>>();
                return albums;
            }
        }

        public async Task<IEnumerable<PhotoConsume>> GetPhotosByAlbumIds(IList<int> albumIds)
        {
            var query = BuildAlbumIdsQuery(albumIds);
            
            var request = new HttpRequestMessage(HttpMethod.Get, $"photos?{query}");

            using (var response = _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                CheckStatusCode(response);

                var stream = await response.Result.Content.ReadAsStreamAsync();
                var photos = stream.ReadAndDeserializeFromJson<IEnumerable<PhotoConsume>>();
                return photos;
            }
        }

        private StringBuilder BuildAlbumIdsQuery(IList<int> albumIds)
        {
            StringBuilder query = new StringBuilder();
            var count = 0;
            foreach (var albumId in albumIds)
            {
                query.Append($"albumId={albumId}");

                count++;

                if (count != albumIds.Count)
                {
                    query.Append("&");
                }
            }

            return query;
        }

        private void CheckStatusCode(Task<HttpResponseMessage> response)
        {
            if (!response.Result.IsSuccessStatusCode)
            {
                if (response.Result.StatusCode == HttpStatusCode.BadRequest)
                {
                    _logger.LogWarning($"Getting albums returned with BadRequest {response.Result.ReasonPhrase}");
                }
                else if (response.Result.StatusCode == HttpStatusCode.NoContent)
                {
                    _logger.LogWarning($"Getting albums returned with NoContent {response.Result.ReasonPhrase}");
                }
                else if (response.Result.StatusCode == HttpStatusCode.InternalServerError)
                {
                    _logger.LogError($"Getting albums returned with InternalServerError {response.Result.ReasonPhrase}");
                }
            }

            try
            {
                response.Result.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
