using MongoDB.Bson.Serialization.Attributes;

namespace EventSourcing.Domain.ComicClub.Comic.Projection.ComicCount;

public class ComicCount
{
    [BsonId]
    public required string Id { get; init; }
    public required int TotalComics { get; set; }
}