using Database.Core.Domain;
using Database.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Manages creation of user notifications related to rental requests and return records.
/// </summary>
public class NotificationManager
{
    /// <summary>
    /// Creates a notification entry in the database for a specific user based on a rental request or return record.
    /// </summary>
    /// <param name="userId">The ID of the user who should receive the notification.</param>
    /// <param name="notificationTypeId">The type ID indicating the reason for the notification (e.g., Approved, Rejected).</param>
    /// <param name="recordType">The type of record associated with the notification. Must be "request" or "return".</param>
    /// <param name="recordId">The ID of the associated RentalRequest or RentalRecord.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown if the record is not found or the recordType is invalid.</exception>
    /// <remarks>
    /// This method dynamically constructs the notification message based on:
    /// - The type of action (e.g., approval, rejection, confirmation)
    /// - The associated equipment's name
    /// - The record context (request vs return)
    /// 
    /// It loads the related RentalRequest or RentalRecord including equipment name,
    /// looks up the NotificationType name, and builds a meaningful user-friendly message.
    /// </remarks>
    public static async Task CreateAsync(RentalDBContext _context, int userId, int notificationTypeId, string recordType, int recordId)
    {
        string message;
        string equipmentName = "";
        int requestId = 0;

        // Load equipment and request ID depending on record type
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

        // Determine message based on notification type
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

        // Create and save the notification
        var notification = new Notification
        {
            UserId = userId,
            NotificationTypeId = notificationTypeId,
            MessageContent = message,
            IsRead = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}
