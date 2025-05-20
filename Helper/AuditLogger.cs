using Database.Core.Domain;
using Database.Persistence;

public static class AuditLogger
{
    public static async Task LogActionAsync(
        RentalDBContext context,
        int userId,
        string actionType,
        string sourceEntity,
        string dataBefore,
        string dataAfter,
        string affectedRecordKey
    )
    {
        var log = new AuditLog
        {
            UserId = userId,
            ActionType = actionType,
            SourceEntity = sourceEntity,
            Source = "Website",
            AffectedRecordKey = affectedRecordKey,
            DataBeforeAction = dataBefore,
            DataAfterAction = dataAfter,
            Timestamp = DateTime.Now
        };

        context.AuditLogs.Add(log);
        await context.SaveChangesAsync();
    }
}
