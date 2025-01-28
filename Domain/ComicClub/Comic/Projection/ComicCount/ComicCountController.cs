using EventSourcing.Common.Ambar;
using EventSourcing.Common.Projection;
using EventSourcing.Common.SerializedEvent;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.Domain.ComicClub.Comic.Projection.ComicCount;

[ApiController]
[Route("api/v1/comic-club/comic/projection")]
[Produces("application/json")]
[Consumes("application/json")]
[AmbarAuthMiddleware]
public class ComicCountProjectionController : ProjectionController
{
    private readonly ComicCountProjectionHandler _comicCountProjectionHandler;

    public ComicCountProjectionController(
        MongoTransactionalProjectionOperator mongoOperator,
        Deserializer deserializer,
        ILogger<ComicCountProjectionController> logger,
        ComicCountProjectionHandler comicCountProjectionHandler)
        : base(mongoOperator, deserializer, logger)
    {
        _comicCountProjectionHandler = comicCountProjectionHandler;
    }

    // expose the handler in an http endpoint
}