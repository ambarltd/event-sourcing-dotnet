using EventSourcing.Common.Event;

namespace EventSourcing.Domain.ComicClub.Comic.Event;

public class ComicAuthorAmended:TransformationEvent<Aggregate.Comic>
{
    public required string NewAuthor { get; init; }

    public required string Reason { get; init; }

    public override Aggregate.Comic TransformAggregate(Aggregate.Comic aggregate)
    {
        return new Aggregate.Comic
        {
            Title = aggregate.Title,
            Author = NewAuthor,
            Description = aggregate.Description,
            Genre = aggregate.Genre,
            NumberOfPages = aggregate.NumberOfPages,
            TextContent =aggregate.TextContent,
            AggregateId = aggregate.AggregateId,
            AggregateVersion = AggregateVersion
        };
    }
}