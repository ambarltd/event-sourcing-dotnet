using System.Text.Json;
using System.Text.Json.Nodes;
using EventSourcing.Domain.ComicClub.Comic.Event;
using EventSourcing.Domain.CookingClub.Membership.Aggregate;
using EventSourcing.Domain.CookingClub.Membership.Event;

namespace EventSourcing.Common.SerializedEvent;

public class Deserializer
{
    public Event.Event Deserialize(SerializedEvent serializedEvent)
    {
        var recordedOn = ToDateTime(serializedEvent.RecordedOn);

        return serializedEvent.EventName switch
        {
            "CookingClub_Membership_ApplicationSubmitted" => new ApplicationSubmitted
            {
                EventId = serializedEvent.EventId,
                AggregateId = serializedEvent.AggregateId,
                AggregateVersion = serializedEvent.AggregateVersion,
                CorrelationId = serializedEvent.CorrelationId,
                CausationId = serializedEvent.CausationId,
                RecordedOn = recordedOn,
                FirstName = PayloadString(serializedEvent.JsonPayload, "firstName"),
                LastName = PayloadString(serializedEvent.JsonPayload, "lastName"),
                FavoriteCuisine = PayloadString(serializedEvent.JsonPayload, "favoriteCuisine"),
                YearsOfProfessionalExperience = PayloadInt(serializedEvent.JsonPayload, "yearsOfProfessionalExperience"),
                NumberOfCookingBooksRead = PayloadInt(serializedEvent.JsonPayload, "numberOfCookingBooksRead")
            },
            "CookingClub_Membership_ApplicationEvaluated" => new ApplicationEvaluated
            {
                EventId = serializedEvent.EventId,
                AggregateId = serializedEvent.AggregateId,
                AggregateVersion = serializedEvent.AggregateVersion,
                CorrelationId = serializedEvent.CorrelationId,
                CausationId = serializedEvent.CausationId,
                RecordedOn = recordedOn,
                EvaluationOutcome = MembershipStatusHelper.FromString(PayloadString(serializedEvent.JsonPayload, "evaluationOutcome"))
            },
            "ComicClub_Comic_ComicUploaded" => new ComicUploaded
            {
                EventId = serializedEvent.EventId,
                AggregateId = serializedEvent.AggregateId,
                AggregateVersion = serializedEvent.AggregateVersion,
                CorrelationId = serializedEvent.CorrelationId,
                CausationId = serializedEvent.CausationId,
                RecordedOn = recordedOn,
                Title = PayloadString(serializedEvent.JsonPayload, "title"),
                Author = PayloadString(serializedEvent.JsonPayload, "author"),
                Description = PayloadString(serializedEvent.JsonPayload, "description"),
                Genre = PayloadString(serializedEvent.JsonPayload, "genre"),
                NumberOfPages = PayloadInt(serializedEvent.JsonPayload, "numberOfPages"),
                TextContent = PayloadString(serializedEvent.JsonPayload, "textContent"),
            },
            "ComicClub_Comic_ComicAuthorAmended" => new ComicAuthorAmended
            {
                EventId = serializedEvent.EventId,
                AggregateId = serializedEvent.AggregateId,
                AggregateVersion = serializedEvent.AggregateVersion,
                CorrelationId = serializedEvent.CorrelationId,
                CausationId = serializedEvent.CausationId,
                RecordedOn = recordedOn,
                NewAuthor = PayloadString(serializedEvent.JsonPayload, "newAuthor"),
                Reason = PayloadString(serializedEvent.JsonPayload, "reason")
            },
            _ => throw new ArgumentException($"Unknown event type: {serializedEvent.EventName}")
        };
    }

    private static DateTime ToDateTime(string recordedOn)
    {
        if (!recordedOn.EndsWith(" UTC"))
        {
            throw new ArgumentException($"Invalid date format: {recordedOn}");
        }
        
        if (DateTime.TryParseExact(recordedOn[..^4], 
                "yyyy-MM-dd HH:mm:ss.ffffff",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal,
                out var result))
        {
            return DateTime.SpecifyKind(result, DateTimeKind.Utc);
        }

        throw new ArgumentException($"Invalid date format: {recordedOn}");
    }

    private static string PayloadString(string jsonString, string fieldName)
    {
        var jsonNode = JsonNode.Parse(jsonString);
        var value = jsonNode?[fieldName]?.GetValue<string>();
        if (value == null)
            throw new ArgumentException($"Required field {fieldName} is null or missing");
        return value;
    }
    
    private static int PayloadInt(string jsonString, string fieldName)
    {
        var jsonNode = JsonNode.Parse(jsonString);
        if (jsonNode?[fieldName] == null)
            throw new ArgumentException($"Required field {fieldName} is null or missing");
        return jsonNode[fieldName]!.GetValue<int>();
    }
}
