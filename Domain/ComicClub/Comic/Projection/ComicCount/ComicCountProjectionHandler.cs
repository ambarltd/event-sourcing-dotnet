using EventSourcing.Common.Projection;
using EventSourcing.Domain.ComicClub.Comic.Event;
using EventSourcing.Domain.CookingClub.Membership.Projection.MembersByCuisine;

namespace EventSourcing.Domain.ComicClub.Comic.Projection.ComicCount;

public class ComicCountProjectionHandler : ProjectionHandler
{
    private readonly ComicCountRepository _comicCountRepository;

    public ComicCountProjectionHandler(
        ComicCountRepository comicCountRepository
    )
    {
        _comicCountRepository = comicCountRepository;
    }

    public override void Project(Common.Event.Event @event)
    {
        switch (@event)
        {
            case ComicUploaded _:
                var comicCount = _comicCountRepository.FindOneById("Total");

                if (comicCount == null) {
                    comicCount = new ComicCount
                    {
                        Id = "Total",
                        TotalComics = 1
                    };
                } else {
                    comicCount.TotalComics++;
                }
                _comicCountRepository.Save(comicCount);

                break;
        }
    }
}