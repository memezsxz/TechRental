using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Image = System.Drawing.Image;

namespace FormsApp
{
    static class Global
    {
        public static int userID = 2;
        public static int userType = 1; // admin
        // public static BindingList<string> pageSizes = new BindingList<string>() { "10", "20", "30" };
        public static BindingList<int> pageSizes = new BindingList<int>() { 10, 20, 30 };

        #region Brand Colors

        public static Color Green = Color.FromArgb(60, 173, 104);
        public static Color LightGreen = Color.FromArgb(200, 228, 211);
        public static Color DarkGreen = Color.FromArgb(49, 129, 80);
        public static Color NeonGreen = Color.FromArgb(120, 226, 161);
        #endregion

        /// <summary>
        /// Maps labels to the associated entity type and permission flags (Add, Edit, Delete).
        /// Used to determine which entity is shown and what operations are allowed.
        /// </summary>

        public static Dictionary<Type, (bool allowAdd, bool allowEdit, bool allowDelete)>? TabTypeMap
        {
            get
            {
                if (userType == 1) return new()
                { 
                    { typeof(AuditLog), (false, false, false) }, 
                    { typeof(SystemErrorLog), (false, false, false) }, 
                    { typeof(User), (false, false, false) }, 
                    { typeof(Category), (true, true, true) },
                    { typeof(Equipment), (true, true, true) }, 
                    { typeof(RentalRequest), (false, true, false) },
                    { typeof(RentalRecord), (false, true, false) },
                };

                if (userType == 2) return new()
                {
                    { typeof(Category), (false, false, false) },
                    { typeof(RentalRequest), (false, true, true) },
                    { typeof(Equipment), (false, false, false) },
                    { typeof(RentalRecord), (false, true, true) }
                };

                return null;
            }
        }




        public static void Panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            int borderRadius = 20; // Adjust roundness
            int borderSize = 3;    // Border thickness
            Color borderColor = Color.Red; // Border color

            // Enable anti-aliasing for smooth edges
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Define rectangle for border
            Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);

            // Create rounded rectangle
            using (GraphicsPath path = GetRoundedPath(rect, borderRadius))
            using (Pen borderPen = new Pen(borderColor, borderSize))
            {
                panel.Region = new Region(path); // Apply rounded shape
                e.Graphics.DrawPath(borderPen, path); // Draw border
            }
        }

        // Helper function to create a rounded rectangle path
        public static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int arcWidth = radius * 2;

            path.AddArc(rect.X, rect.Y, arcWidth, arcWidth, 180, 90); // Top-left
            path.AddArc(rect.Right - arcWidth, rect.Y, arcWidth, arcWidth, 270, 90); // Top-right
            path.AddArc(rect.Right - arcWidth, rect.Bottom - arcWidth, arcWidth, arcWidth, 0, 90); // Bottom-right
            path.AddArc(rect.X, rect.Bottom - arcWidth, arcWidth, arcWidth, 90, 90); // Bottom-left

            path.CloseFigure();
            return path;
        }


        public static void SizeAndCenterForm(Form frm, float screenPercent)
        {
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;

            frm.Size = new System.Drawing.Size(Convert.ToInt32(screenPercent * workingRectangle.Width), Convert.ToInt32(screenPercent * workingRectangle.Height));
        }

        public static void DisplayReportErrorDialog(Exception e)
        {
            Console.WriteLine("From Global: Error: " + e.Message);
        }

        public static void DrawRoundedBorder(Control control, PaintEventArgs e, Color borderColor, int borderRadius = 10, int borderWidth = 1)
        {
            if (control == null) return;

            using Pen borderPen = new Pen(borderColor, borderWidth)
            {
                Alignment = System.Drawing.Drawing2D.PenAlignment.Inset
            };

            var path = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);
            int arcSize = borderRadius * 2;

            path.AddArc(rect.X, rect.Y, arcSize, arcSize, 180, 90); // Top-left
            path.AddArc(rect.Right - arcSize, rect.Y, arcSize, arcSize, 270, 90); // Top-right
            path.AddArc(rect.Right - arcSize, rect.Bottom - arcSize, arcSize, arcSize, 0, 90); // Bottom-right
            path.AddArc(rect.X, rect.Bottom - arcSize, arcSize, arcSize, 90, 90); // Bottom-left
            path.CloseFigure();

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawPath(borderPen, path);
        }

        public static async Task<Image> GetImage(Guid guid, string format)
        {
            try
            {
                var stream = await S3Uploader.GetFileByGuidAsync(guid.ToString());
                if (stream == null) return null;

                stream.Position = 0;
                var ext = format.Split('/').Last();
                var tempPath = Path.Combine(Path.GetTempPath(), $"temp_image_{guid}.{ext}");

                await using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                    await stream.CopyToAsync(fs);

                return Image.FromFile(tempPath);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void ApplyRoundedStyle(Panel panel, PaintEventArgs e, Color borderColor, int borderRadius = 10, int borderWidth = 1)
        {
            if (panel == null || panel.Controls.Count == 0) return;

            Control child = panel.Controls[0]; // Assume single child (e.g., Label)
            int arc = borderRadius * 2;

            Rectangle clipRect = new Rectangle(0, 0, panel.Width, panel.Height);
            Rectangle drawRect = new Rectangle(0, 0, child.Width - 1, child.Height - 1);

            // --- Clipping region for panel ---
            using (GraphicsPath clipPath = new GraphicsPath())
            {
                clipPath.AddArc(clipRect.X, clipRect.Y, arc, arc, 180, 90);
                clipPath.AddArc(clipRect.Right - arc, clipRect.Y, arc, arc, 270, 90);
                clipPath.AddArc(clipRect.Right - arc, clipRect.Bottom - arc, arc, arc, 0, 90);
                clipPath.AddArc(clipRect.X, clipRect.Bottom - arc, arc, arc, 90, 90);
                clipPath.CloseFigure();

                panel.Region = new Region(clipPath);
            }

            // --- Border + Fill for child ---
            using (GraphicsPath borderPath = new GraphicsPath())
            {
                borderPath.AddArc(drawRect.X, drawRect.Y, arc, arc, 180, 90);
                borderPath.AddArc(drawRect.Right - arc, drawRect.Y, arc, arc, 270, 90);
                borderPath.AddArc(drawRect.Right - arc, drawRect.Bottom - arc, arc, arc, 0, 90);
                borderPath.AddArc(drawRect.X, drawRect.Bottom - arc, arc, arc, 90, 90);
                borderPath.CloseFigure();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using SolidBrush fillBrush = new SolidBrush(child.BackColor);
                e.Graphics.FillPath(fillBrush, borderPath);

                using Pen pen = new Pen(borderColor, borderWidth)
                {
                    Alignment = PenAlignment.Inset
                };
                e.Graphics.DrawPath(pen, borderPath);
            }
        }

        public static async Task<bool> LoadImage(Guid? guid, string imageType, Panel displayPanel, Label imageLabel)
        {
            try
            {
                if (!guid.HasValue)
                {
                    imageLabel.Text = ("No Image Selected");
                    return false;
                }

                var image = await Global.GetImage(guid.Value, imageType);

                if (image == null)
                {
                    imageLabel.Text = ("Unable To Load Image");
                    return false;
                }

                displayPanel.Controls.Clear();
                displayPanel.Controls.Add(new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = new Bitmap(image)
                });

                imageLabel.Text = "Upload";

                displayPanel.Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                imageLabel.Text = ("Image Not Found");
                Console.WriteLine($"Image loading error: {ex.Message}");
                return false;
            }

        }
     public   static void SetBorderColor(Label sender, PaintEventArgs e, Color color)
        {
            ControlPaint.DrawBorder(e.Graphics, sender.DisplayRectangle, color, ButtonBorderStyle.Solid);
        }
    }
}
