using EventSourcing.Common.Projection;
using MongoDB.Driver;

namespace EventSourcing.Domain.ComicClub.Comic.Projection.ComicCount;

public class ComicCountRepository
{
    private readonly MongoTransactionalProjectionOperator _mongoOperator;
    private static string _collectionName = "ComicClub_ComicCount_Count";

    public ComicCountRepository(MongoTransactionalProjectionOperator mongoOperator)
    {
        _mongoOperator = mongoOperator;
    }

    public void Save(ComicCount cuisine)
    {
        _mongoOperator.ReplaceOne(
            _collectionName,
            c => c.Id == cuisine.Id,
            cuisine,
            new ReplaceOptions { IsUpsert = true }
        );
    }

    public ComicCount? FindOneById(string id)
    {
        return _mongoOperator.Find<ComicCount>(
            _collectionName,
            c => c.Id == id
        ).FirstOrDefault();
    }
}