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
    public partial class NotificationsView : UserControl
    {

        private UnitOfWork context = new UnitOfWork();
        public NotificationsView()
        {
            InitializeComponent();
            lblAllRead.Paint += (s, e) => Global.SetBorderColor(lblAllRead, e, Global.Green);
            FetchAllNotifications();
        }

        private void lblAllRead_Click(object sender, EventArgs e)
        {
          bool result =   context.Notifications.MarkAllAsRead(Global.userID);

          if (!result)
          {
              MessageBox.Show("A problem happened wile updating the notifications, please try again later");
          }
          else
          {
              FetchAllNotifications();
          }
        }

        private void FetchAllNotifications()
        {
            List< Notification>  list= context.Notifications.GetAllByUser(Global.userID);
         var newList =   list.Select(n => new
            {
                Id = n.Id,
                Type = n.NotificationType.TypeName,
                SentAt = n.CreatedAt,
             Message =   n.MessageContent ,
             Read = n.IsRead ?? true,
            }).ToList();

            dgvNotifications.DataSource = newList;
        }
    }
}
