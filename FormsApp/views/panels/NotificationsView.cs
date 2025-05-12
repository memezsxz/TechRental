using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Core.Domain;
using Database.Persistence;

namespace FormsApp.views.panels
{
    /// <summary>
    /// User control for displaying and managing user notifications
    /// </summary>
    public partial class NotificationsView : UserControl
    {
        #region Fields
        /// <summary>
        /// Database context for accessing notification data
        /// </summary>
        private UnitOfWork context = new UnitOfWork();
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the NotificationsView control
        /// </summary>
        public NotificationsView()
        {
            InitializeComponent();

            // Add green border to "Mark All as Read" label
            lblAllRead.Paint += (s, e) => Global.SetBorderColor(lblAllRead, e, Global.Green);

            // Load initial notifications
            FetchAllNotifications();
        }
        #endregion

        #region Notification Methods
        /// <summary>
        /// Fetches all notifications for the current user and binds them to the DataGridView
        /// </summary>
        private void FetchAllNotifications()
        {
            // Get notifications from database
            List<Notification> list = context.Notifications.GetAllByUser(Global.userID);

            // Transform data for display
            var newList = list.Select(n => new
            {
                Id = n.Id,
                Type = n.NotificationType.TypeName,
                SentAt = n.CreatedAt,
                Message = n.MessageContent,
                Read = n.IsRead ?? true,  // Default to true if null
            }).ToList();

            // Bind to DataGridView
            dgvNotifications.DataSource = newList;
        }

        /// <summary>
        /// Marks all notifications as read for the current user
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">Event arguments</param>
        private void lblAllRead_Click(object sender, EventArgs e)
        {
            bool result = context.Notifications.MarkAllAsRead(Global.userID);

            if (!result)
            {
                MessageBox.Show(
                    "A problem occurred while updating the notifications, please try again later",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                // Refresh notification list after successful update
                FetchAllNotifications();
            }
        }
        #endregion

        #region UI Helper Methods
        /// <summary>
        /// Customizes the appearance of notification rows based on read status
        /// </summary>
        private void dgvNotifications_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if this is the "Read" column (index 4)
            if (e.ColumnIndex == 4 && e.Value != null)
            {
                bool isRead = (bool)e.Value;

                // Style unread notifications differently
                if (!isRead)
                {
                    e.CellStyle.BackColor = Color.LightYellow;
                    e.CellStyle.Font = new Font(dgvNotifications.Font, FontStyle.Bold);
                }
            }
        }
        #endregion
    }
}