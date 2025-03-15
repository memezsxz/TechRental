using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechRental
{
    static class Global
    {
        public static int userID = 2;
        public static int userType = 1; // admin


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
    }
}
