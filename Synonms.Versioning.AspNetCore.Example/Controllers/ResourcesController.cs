using System;
using Microsoft.AspNetCore.Mvc;
using Synonms.Versioning.AspNetCore.Example.Models;
using Synonms.Versioning.Core;
using Synonms.Versioning.Core.Serialisation;
using Synonms.Versioning.Web;

namespace Synonms.Versioning.AspNetCore.Example.Controllers
{
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IVersionableSerialiser _serialiser;

        public ResourcesController(IVersionableSerialiser serialiser)
        {
            _serialiser = serialiser;
        }

        [HttpGet]
        [Route("api/v1/[controller]")]
        [Route("api/v2/[controller]")]
        public ActionResult<Resource> Get()
        {
            var requestedVersion = HttpContext.GetRequestedVersion() ?? new Version();

            var response = new Resource
            { 
                Id = 1,
                Name = "My Thingy",
                SuperOldDeprecatedThingy = "I was deprecated in v2",
                BrandNewThingy = "I was added in v2"
            };

            var json = _serialiser.Serialise(response, requestedVersion);

            return Ok(json);
        }

        [HttpGet]
        [Route("api/v2/[controller]/{id}")]
        [VersionHistory(IntroducedMajorVersion = 2, IntroducedMinorVersion = 0)]
        public ActionResult<Resource> GetV2Only(int id)
        {
            var requestedVersion = HttpContext.GetRequestedVersion() ?? new Version();

            var response = new Resource
            {
                Id = id,
                Name = "Requested Thingy",
                SuperOldDeprecatedThingy = "I was deprecated in v2",
                BrandNewThingy = "I was added in v2"
            };

            var json = _serialiser.Serialise(response, requestedVersion);

            return Ok(json);
        }
    }
}
