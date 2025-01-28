using EventSourcing.Common.Command;
using EventSourcing.Common.EventStore;
using EventSourcing.Common.Util;
using EventSourcing.Domain.ComicClub.Comic.Event;

namespace EventSourcing.Domain.ComicClub.Comic.Command.UploadComic;

public class UploadComicCommandHandler : CommandHandler
{
    public UploadComicCommandHandler(
        PostgresTransactionalEventStore postgresTransactionalEventStore
    ) : base(postgresTransactionalEventStore) {
    }
    
    public override void HandleCommand(Common.Command.Command command)
    {
        if (command is UploadComicCommand uploadComicCommand)
        {
            HandleUploadComic(uploadComicCommand);
        } else {
            throw new ArgumentException($"Unsupported command type: {command.GetType().Name}");
        }
    }
    
    public void HandleUploadComic(UploadComicCommand command)
    {
        var eventId = IdGenerator.GenerateRandomId();
        var aggregateId = IdGenerator.GenerateRandomId();

        var comicUploaded = new ComicUploaded() {
            EventId = eventId,
            AggregateId = aggregateId,
            AggregateVersion = 1,
            CorrelationId = eventId,
            CausationId = eventId,
            RecordedOn = DateTime.UtcNow,
            Title = command.Title,
            Author = command.Author,
            Description = command.Description,
            Genre = command.Genre,
            NumberOfPages = command.NumberOfPages,
            TextContent = command.TextContent
        };

        _postgresTransactionalEventStore.SaveEvent(comicUploaded);
    }
}