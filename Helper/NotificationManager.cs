using Database.Core.Domain;
using Database.Persistence;
using Microsoft.EntityFrameworkCore;

public class NotificationManager
{
    private readonly RentalDBContext _context;

    public NotificationManager(RentalDBContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a notification for a specific user and record.
    /// Automatically resolves the equipment name from request or return record.
    /// </summary>
    /// <param name="userId">The ID of the user to notify</param>
    /// <param name="notificationTypeId">The ID of the notification type</param>
    /// <param name="recordType">Either "request" or "return"</param>
    /// <param name="recordId">The ID of the related request or return record</param>
    public async Task CreateAsync(int userId, int notificationTypeId, string recordType, int recordId)
    {
        string message;
        string equipmentName = "";
        int requestId = 0;

        // Load equipment name and request ID based on record type
        if (recordType.ToLower() == "request")
        {
            var request = await _context.RentalRequests
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == recordId);

            if (request == null)
                throw new ArgumentException("Rental request not found.");

            equipmentName = request.Equipment.Name;
            requestId = request.Id;
        }
        else if (recordType.ToLower() == "return")
        {
            var record = await _context.RentalRecords
                .Include(r => r.RentalRequest).ThenInclude(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == recordId);

            if (record == null)
                throw new ArgumentException("Return record not found.");

            equipmentName = record.RentalRequest.Equipment.Name;
            requestId = record.RentalRequest.Id;
        }
        else
        {
            throw new ArgumentException("Invalid record type. Must be 'request' or 'return'.");
        }

        // Generate default message
        var type = await _context.NotificationTypes.FindAsync(notificationTypeId);
        var typeName = type?.TypeName?.ToLower();

        message = typeName switch
        {
            "request approved" => $"Your rental request #{requestId} for {equipmentName} has been approved.",
            "request rejected" => $"Your rental request #{requestId} for {equipmentName} was rejected.",
            "request canceled" => $"Your rental request #{requestId} for {equipmentName} has been canceled.",
            "overdue reminder" => $"Your return for {equipmentName} is overdue. Please return it as soon as possible.",
            "return confirmation" => $"Your return record #{recordId} for {equipmentName} has been confirmed.",
            _ => $"You have a new update regarding {equipmentName}."
        };

        // Insert notification
        var notification = new Notification
        {
            UserId = userId,
            NotificationTypeId = notificationTypeId,
            MessageContent = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}
