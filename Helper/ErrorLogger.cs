using Database.Core.Domain;
using Database.Persistence;

public static class ErrorLogger
{
    public static async Task LogErrorAsync(
        RentalDBContext context,
        int userId,
        string errorMessage,
        string errorSource,
        string? sourceProcedure = null)
    {
        var errorLog = new SystemErrorLog
        {
            UserId = userId,
            ErrorMessage = errorMessage,
            ErrorSource = errorSource,
            SourceProcedure = sourceProcedure,
            Timestamp = DateTime.Now
        };

        context.SystemErrorLogs.Add(errorLog);
        await context.SaveChangesAsync();
    }
}
