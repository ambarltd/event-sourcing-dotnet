using System.Text.Json.Nodes;
using EventSourcing.Domain.ComicClub.Comic.Event;
using EventSourcing.Domain.CookingClub.Membership.Event;

namespace EventSourcing.Common.SerializedEvent;

public class Serializer
{
    public SerializedEvent Serialize(Event.Event @event)
    {
        return new SerializedEvent
        {
            EventId = @event.EventId,
            AggregateId = @event.AggregateId,
            AggregateVersion = @event.AggregateVersion,
            CorrelationId = @event.CorrelationId,
            CausationId = @event.CausationId,
            RecordedOn = FormatDateTime(@event.RecordedOn),
            EventName = DetermineEventName(@event),
            JsonPayload = CreateJsonPayload(@event),
            JsonMetadata = "{}"
        };
    }

    private static string DetermineEventName(Event.Event @event)
    {
        return @event switch
        {
            ApplicationSubmitted => "CookingClub_Membership_ApplicationSubmitted",
            ApplicationEvaluated => "CookingClub_Membership_ApplicationEvaluated",
            ComicUploaded => "ComicClub_Comic_ComicUploaded",
            ComicAuthorAmended => "ComicClub_Comic_ComicAuthorAmended",
            _ => throw new ArgumentException($"Unknown event type: {@event.GetType().Name}")
        };
    }

    private static string CreateJsonPayload(Event.Event @event)
    {
        var jsonObject = new JsonObject();

        switch (@event)
        {
            case ApplicationSubmitted applicationSubmitted:
                jsonObject.Add("firstName", applicationSubmitted.FirstName);
                jsonObject.Add("lastName", applicationSubmitted.LastName);
                jsonObject.Add("favoriteCuisine", applicationSubmitted.FavoriteCuisine);
                jsonObject.Add("yearsOfProfessionalExperience", applicationSubmitted.YearsOfProfessionalExperience);
                jsonObject.Add("numberOfCookingBooksRead", applicationSubmitted.NumberOfCookingBooksRead);
                break;

            case ApplicationEvaluated applicationEvaluated:
                jsonObject.Add("evaluationOutcome", applicationEvaluated.EvaluationOutcome.ToString());
                break;
            
            case ComicUploaded comicUploaded:
                jsonObject.Add("title", comicUploaded.Title);
                jsonObject.Add("author", comicUploaded.Author);
                jsonObject.Add("description", comicUploaded.Description);
                jsonObject.Add("genre", comicUploaded.Genre);
                jsonObject.Add("numberOfPages", comicUploaded.NumberOfPages);
                jsonObject.Add("textContent", comicUploaded.TextContent);
                break;
            
            case ComicAuthorAmended comicAuthorAmended:
                jsonObject.Add("newAuthor", comicAuthorAmended.NewAuthor);
                jsonObject.Add("reason", comicAuthorAmended.Reason);
                break;
                
        }

        return jsonObject.ToJsonString();
    }

    private static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.ffffff UTC");
    }
}