using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Runpath.Api.Contract.Interfaces.Services;
using Runpath.Api.Contract.Models;

namespace Runpath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumsService _albumsService;

        public AlbumsController(IAlbumsService albumsService)
        {
            _albumsService = albumsService;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> Get()
        {
            var albums = await _albumsService.GetAlbumsWithPhotos();


            if (albums?.Count() == 0)
            {
                return NoContent();
            }

            return Ok(albums);
        }

        // GET api/values/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<Album>> Get(int userId)
        {
            if (userId < 1)
            {
                return BadRequest();
            }

            var albums = await _albumsService.GetAlbumsWithPhotosByUserId(userId);

            if (albums?.Count() == 0)
            {
                return NoContent();
            }

            return Ok(albums);
        }
    }
}
