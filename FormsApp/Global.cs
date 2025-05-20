using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Database.Core;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Helper;
using Image = System.Drawing.Image;

namespace FormsApp
{
    /// <summary>
    /// Provides global application utilities, constants, and helper methods
    /// </summary>
    public static class Global
    {
        #region User Session Properties
        /// <summary>
        /// Currently logged-in user ID
        /// </summary>
        public static int userID = 40;

        /// <summary>
        /// Type/role of the currently logged-in user (e.g., "admin", "manager")
        /// </summary>
        public static string userType = "admin";

        /// <summary>
        /// Available page sizes for pagination controls
        /// </summary>
        public static BindingList<int> pageSizes = new BindingList<int>() { 10, 20, 30 };
        #endregion

        #region Brand Colors
        /// <summary>
        /// Primary brand green color
        /// </summary>
        public static Color Green = Color.FromArgb(60, 173, 104);

        /// <summary>
        /// Light green accent color
        /// </summary>
        public static Color LightGreen = Color.FromArgb(200, 228, 211);

        /// <summary>
        /// Dark green accent color
        /// </summary>
        public static Color DarkGreen = Color.FromArgb(49, 129, 80);

        /// <summary>
        /// Neon green accent color
        /// </summary>
        public static Color NeonGreen = Color.FromArgb(120, 226, 161);
        #endregion

        #region Permission Mapping
        /// <summary>
        /// Maps entity types to permission flags (Add, Edit, Delete) based on user type
        /// </summary>
        /// <remarks>
        /// Determines which operations are allowed for different entity types based on user role.
        /// </remarks>
        /// <returns>null if the role does not match</returns>
        public static Dictionary<Type, (bool allowAdd, bool allowEdit, bool allowDelete)>? TabTypeMap
        {
            get
            {
                if (Global.userType.ToLower() == "admin")
                {
                    return new()
                    {
                        { typeof(AuditLog), (false, false, false) },
                        { typeof(SystemErrorLog), (false, false, false) },
                        { typeof(User), (false, false, false) },
                        { typeof(Category), (true, true, true) },
                        { typeof(Equipment), (true, true, true) },
                        { typeof(RentalRequest), (false, true, false) },
                        { typeof(RentalRecord), (false, true, false) },
                    };
                }

                if (Global.userType.ToLower() == "manager")
                {
                    return new()
                    {
                        { typeof(Category), (false, false, false) },
                        { typeof(RentalRequest), (false, true, false) },
                        { typeof(Equipment), (false, false, false) },
                        { typeof(RentalRecord), (false, true, false) }
                    };
                }

                return null;
            }
        }
        #endregion

        #region UI Helper Methods
        /// <summary>
        /// Paints a rounded border around a panel control
        /// </summary>
        /// <param name="sender">The panel control to paint</param>
        /// <param name="e">Paint event arguments</param>
        public static void Panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            // Style configuration
            int borderRadius = 20; // Adjust roundness
            int borderSize = 3;   // Border thickness
            Color borderColor = Color.Red; // Border color

            // Enable anti-aliasing for smooth edges
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

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

        /// <summary>
        /// Creates a rounded rectangle graphics path
        /// </summary>
        /// <param name="rect">Rectangle to round</param>
        /// <param name="radius">Corner radius</param>
        /// <returns>GraphicsPath representing the rounded rectangle</returns>
        public static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int arcWidth = radius * 2;

            // Add arcs for each corner
            path.AddArc(rect.X, rect.Y, arcWidth, arcWidth, 180, 90); // Top-left
            path.AddArc(rect.Right - arcWidth, rect.Y, arcWidth, arcWidth, 270, 90); // Top-right
            path.AddArc(rect.Right - arcWidth, rect.Bottom - arcWidth, arcWidth, arcWidth, 0, 90); // Bottom-right
            path.AddArc(rect.X, rect.Bottom - arcWidth, arcWidth, arcWidth, 90, 90); // Bottom-left

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Sizes and centers a form relative to the primary screen
        /// </summary>
        /// <param name="frm">Form to position</param>
        /// <param name="screenPercent">Percentage of screen size to use (0.0 - 1.0)</param>
        public static void SizeAndCenterForm(Form frm, float screenPercent)
        {
            Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            frm.Size = new Size(
                Convert.ToInt32(screenPercent * workingRectangle.Width),
                Convert.ToInt32(screenPercent * workingRectangle.Height));
        }

        /// <summary>
        /// Draws a rounded border around any control
        /// </summary>
        /// <param name="control">Control to decorate</param>
        /// <param name="e">Paint event arguments</param>
        /// <param name="borderColor">Border color</param>
        /// <param name="borderRadius">Corner radius</param>
        /// <param name="borderWidth">Border thickness</param>
        public static void DrawRoundedBorder(Control control, PaintEventArgs e, Color borderColor,
            int borderRadius = 10, int borderWidth = 1)
        {
            if (control == null) return;

            using Pen borderPen = new Pen(borderColor, borderWidth)
            {
                Alignment = PenAlignment.Inset
            };

            var path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);
            int arcSize = borderRadius * 2;

            // Create rounded rectangle path
            path.AddArc(rect.X, rect.Y, arcSize, arcSize, 180, 90); // Top-left
            path.AddArc(rect.Right - arcSize, rect.Y, arcSize, arcSize, 270, 90); // Top-right
            path.AddArc(rect.Right - arcSize, rect.Bottom - arcSize, arcSize, arcSize, 0, 90); // Bottom-right
            path.AddArc(rect.X, rect.Bottom - arcSize, arcSize, arcSize, 90, 90); // Bottom-left
            path.CloseFigure();

            // Draw the path
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawPath(borderPen, path);
        }

        /// <summary>
        /// Sets a border color for a label control
        /// </summary>
        /// <param name="sender">Label control</param>
        /// <param name="e">Paint event arguments</param>
        /// <param name="color">Border color</param>
        public static void SetBorderColor(Label sender, PaintEventArgs e, Color color)
        {
            ControlPaint.DrawBorder(e.Graphics, sender.DisplayRectangle, color, ButtonBorderStyle.Solid);
        }
        #endregion

        #region Image Handling
        /// <summary>
        /// Retrieves an image from storage by its GUID
        /// </summary>
        /// <param name="guid">Unique identifier for the image</param>
        /// <param name="format">Image format (e.g., "image/png")</param>
        /// <returns>Image object or null if not found</returns>
        public static async Task<Image> GetImage(Guid guid, string? format)
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

        /// <summary>
        /// Loads an image into a display panel with status feedback
        /// </summary>
        /// <param name="guid">Image GUID</param>
        /// <param name="imageType">Image MIME type</param>
        /// <param name="displayPanel">Panel to display the image</param>
        /// <param name="imageLabel">Label for status messages</param>
        /// <returns>True if image loaded successfully, false otherwise</returns>
        public static async Task<bool> LoadImage(Guid? guid, string? imageType, Panel displayPanel, Label imageLabel)
        {
            try
            {
                if (!guid.HasValue)
                {
                    imageLabel.Text = "No Image Selected";
                    return false;
                }

                var image = await Global.GetImage(guid.Value, imageType);

                if (image == null)
                {
                    imageLabel.Text = "Unable To Load Image";
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
                imageLabel.Text = "Image Not Found";
                Console.WriteLine($"Image loading error: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Error Handling
        /// <summary>
        /// Displays an error dialog and optionally reports the error
        /// </summary>
        /// <param name="e">Exception to report</param>
        public static void DisplayReportErrorDialog(Exception e)
        {
            var result = MessageBox.Show(
                "Something went wrong while processing your action.\n\nWould you like to report this error to the support team?",
                "Unexpected Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(userID);

                string sourceProc = FormatStackTraceForSourceProcedure(e);
                string className = (e.TargetSite?.DeclaringType?.Name ?? "") + e.InnerException?.Message;
                if (!string.IsNullOrWhiteSpace(className)) sourceProc = className + "." + sourceProc;
                sourceProc = Truncate(sourceProc, 255);

                var errorLog = new SystemErrorLog
                {
                    ErrorMessage = Truncate(e.GetType().Name + ": " + e.Message, 255),
                    ErrorSource = "FormsApp",
                    SourceProcedure = sourceProc,
                    UserId = unitOfWork.UserId,
                    Timestamp = DateTime.Now
                };

                unitOfWork.SystemErrorLogs.Add(errorLog);
                unitOfWork.SaveChanges();

                MessageBox.Show(
                    "Thank you. The error has been reported anonymously and will be reviewed.",
                    "Report Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception exception)
            {
                //Console.WriteLine(exception.Message);
                //Console.WriteLine(exception.InnerException?.Message);
                //Console.WriteLine(exception.StackTrace);
                MessageBox.Show("Could not report error, please contact support.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Formats stack trace information for error reporting
        /// </summary>
        /// <param name="e">Exception to format</param>
        /// <returns>Formatted stack trace string</returns>
        private static string FormatStackTraceForSourceProcedure(Exception e)
        {
            if (e?.StackTrace == null) return "Unknown";

            var lines = e.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var formatted = new List<string>();

            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (!trimmed.StartsWith("at ")) continue;

                var methodPart = trimmed.Substring(3); // remove "at "
                var lineInfo = "";

                var inIndex = methodPart.IndexOf(" in ");
                if (inIndex >= 0)
                {
                    var pathPart = methodPart.Substring(inIndex + 4);
                    methodPart = methodPart.Substring(0, inIndex);

                    var lineNumberIndex = pathPart.IndexOf(":line ");
                    if (lineNumberIndex >= 0)
                    {
                        var lineNumber = pathPart.Substring(lineNumberIndex + 6);
                        lineInfo = $" :: line {lineNumber}";
                    }
                }

                var methodOnly = methodPart.Split('.').Last(); // Remove namespace/class
                formatted.Add(methodOnly + lineInfo);
            }

            return string.Join(" <- ", formatted);
        }

        /// <summary>
        /// Truncates a string with ellipsis if it exceeds max length
        /// </summary>
        /// <param name="input">String to truncate</param>
        /// <param name="maxLength">Maximum length before truncation</param>
        /// <returns>Truncated string if necessary</returns>
        private static string Truncate(string? input, int maxLength)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Length <= maxLength ? input : input.Substring(0, maxLength - 3) + "...";
        }
        #endregion
    }
}