using EventSourcing.Common.Event;

namespace EventSourcing.Domain.ComicClub.Comic.Event;

public class ComicUploaded : CreationEvent<Aggregate.Comic>
{
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required string Description { get; init; }
    public required string Genre { get; init; }
    public required int NumberOfPages { get; init; }
    public required string TextContent { get; init; }
    
    public override Aggregate.Comic CreateAggregate()
    {
        return new Aggregate.Comic()
        {
            AggregateId = AggregateId,
            AggregateVersion = AggregateVersion,
            Title = Title,
            Author = Author,
            Description = Description,
            Genre = Genre,
            NumberOfPages = NumberOfPages,
            TextContent = TextContent
        };
    }
}