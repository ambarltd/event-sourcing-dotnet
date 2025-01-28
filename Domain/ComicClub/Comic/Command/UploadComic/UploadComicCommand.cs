namespace EventSourcing.Domain.ComicClub.Comic.Command.UploadComic;

public class UploadComicCommand: Common.Command.Command
{
    public required string Title { get; init; }
    public required string Author { get; init; }
    public required string Description { get; init; }
    public required string Genre { get; init; }
    public required int NumberOfPages { get; init; }
    public required string TextContent { get; init; }
}