using EventSourcing.Common.Command;
using EventSourcing.Common.EventStore;
using EventSourcing.Common.Projection;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.Domain.ComicClub.Comic.Command.UploadComic;

[ApiController]
[Route("api/v1/cooking-club/membership/command")]
[Produces("application/json")]
[Consumes("application/json")]
public class UploadComicCommandController : CommandController
{
    private UploadComicCommandHandler _uploadComicCommandHandler;

    public UploadComicCommandController(
        PostgresTransactionalEventStore postgresTransactionalEventStore,
        MongoTransactionalProjectionOperator mongoTransactionalProjectionOperator,
        ILogger<UploadComicCommandController> logger,
        UploadComicCommandHandler uploadComicCommandHandler
        ) : base(postgresTransactionalEventStore, mongoTransactionalProjectionOperator, logger)
    {
        _uploadComicCommandHandler = uploadComicCommandHandler;
    }


    [HttpGet("generate-random-comic-upload")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult UploadComic()
    {
        var command = GenerateRandomCommand();
        
        ProcessCommand(command, _uploadComicCommandHandler);
        
        return new OkObjectResult(new { });
    }
    
    public static UploadComicCommand GenerateRandomCommand()
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var getRandomElement = (string[] array) =>
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var scale = BitConverter.ToUInt32(bytes, 0) / (double)uint.MaxValue;
            return array[Math.Min((int)(scale * array.Length), array.Length - 1)];
        };

        var getRandomNumber = (int min, int max) =>
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var scale = BitConverter.ToUInt32(bytes, 0) / (double)uint.MaxValue;
            return (int)(min + scale * (max - min));
        };

        var titlePrefixes = new[] { "The", "Tales of", "Chronicles of", "Legend of", "Rise of" };
        var titleNouns = new[] { "Dragon", "Knight", "Warrior", "City", "World", "Kingdom" };
        var firstNames = new[] { "James", "Emma", "Liam", "Olivia", "Noah" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones" };
        var genres = new[] { "Action", "Adventure", "Comedy", "Drama", "Fantasy" };
        var descriptions = new[] 
        {
            "An epic tale of adventure and discovery.",
            "A thrilling journey through unknown territories.",
            "A story of love, loss, and redemption."
        };

        return new UploadComicCommand
        {
            Title = $"{getRandomElement(titlePrefixes)} {getRandomElement(titleNouns)}",
            Author = $"{getRandomElement(firstNames)} {getRandomElement(lastNames)}",
            Description = getRandomElement(descriptions),
            Genre = getRandomElement(genres),
            NumberOfPages = getRandomNumber(20, 300),
            TextContent = "This is the comic's text content. Imagine there's a great story here."
        };
    }
}