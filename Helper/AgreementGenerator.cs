using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Database.Core.Domain;
using System.IO;

public static class AgreementGenerator
{
    public static MemoryStream GeneratePdf(RentalRequest request)
    {
        var doc = new PdfDocument();
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);

        var headerFont = new XFont("Verdana", 13, XFontStyle.Bold);
        var sectionFont = new XFont("Verdana", 11, XFontStyle.Bold);
        var labelFont = new XFont("Verdana", 10, XFontStyle.Bold);
        var valueFont = new XFont("Verdana", 10, XFontStyle.Regular);

        var greenBrush = new XSolidBrush(XColor.FromArgb(0x3C, 0xAD, 0x68)); // #3CAD68
        var blackBrush = XBrushes.Black;

        double margin = 40;
        double y = margin;
        double lineHeight = 18;
        double pageWidth = page.Width;

        void WriteLine(string text, XFont font, XBrush color = null)
        {
            gfx.DrawString(text, font, color ?? blackBrush, new XPoint(margin, y));
            y += lineHeight;
        }

        void WriteLabelValue(string label, string value, bool isSignature = false)
        {
            double labelWidth = gfx.MeasureString(label, labelFont).Width;
            gfx.DrawString(label, labelFont, isSignature ? greenBrush : blackBrush, new XPoint(margin, y));
            gfx.DrawString(value, valueFont, isSignature ? greenBrush : blackBrush, new XPoint(margin + labelWidth, y));
            y += lineHeight;
        }

        void WriteLeftHeading(string title)
        {
            gfx.DrawString(title, sectionFont, greenBrush, new XPoint(margin, y));
            y += lineHeight + 5;
        }

        // Header
        gfx.DrawString("EQUIPMENT RENTAL AGREEMENT", headerFont, greenBrush,
            new XRect(0, y, pageWidth, lineHeight), XStringFormats.TopCenter);
        y += lineHeight + 20;

        // Company Info
        WriteLabelValue("Rental Company Name: ", "TechRental");
        WriteLabelValue("Address: ", "Building 215, Road 3605, Block 436, Seef District, Manama, Kingdom of Bahrain");
        WriteLabelValue("Phone: ", "+973 17 000 123");
        WriteLabelValue("Email: ", "info@techrental.bh");

        y += 10;
        WriteLeftHeading("Customer Information");
        WriteLabelValue("First Name: ", request.Customer?.FirstName ?? "");
        WriteLabelValue("Last Name: ", request.Customer?.LastName ?? "");
        WriteLabelValue("Email: ", request.Customer?.Email ?? "");
        WriteLabelValue("Phone Number: ", request.Customer?.PhoneNumber ?? "");

        y += 10;
        WriteLeftHeading("Equipment Details");
        WriteLabelValue("Equipment Name: ", request.Equipment?.Name ?? "");
        WriteLabelValue("Equipment Id: ", request.Equipment?.Id.ToString() ?? "");
        WriteLabelValue("Rental Start Date: ", request.StartDate.ToShortDateString());
        WriteLabelValue("Expected Return Date: ", request.ReturnDate.ToShortDateString());
        WriteLabelValue("Rental Rate: ", $"{request.RentalPerDay:0.000} BHD/Day");

        // Calculated fields
        int numberOfDays = (request.ReturnDate - request.StartDate).Days;
        decimal deposit = request.RentalPerDay.Value * 0.7m;
        decimal totalCost = (numberOfDays * request.RentalPerDay.Value) + deposit;

        WriteLabelValue("Number of Days: ", numberOfDays.ToString());
        WriteLabelValue("Security Deposit: ", $"{deposit:0.000} BHD");
        WriteLabelValue("Total Cost: ", $"{totalCost:0.000} BHD");

        y += 10;
        WriteLeftHeading("Terms & Conditions");

        string[] terms = new[]
        {
            "• The equipment shall be used only for its intended purpose and in a safe and responsible",
            "  manner.",
            "• The customer is fully responsible for any loss, theft, or damage that occurs during the",
            "  rental period.",
            "• The security deposit will be refunded upon the safe and timely return of the equipment,",
            "  subject to inspection.",
            "• Late returns will incur additional charges at the daily rate unless otherwise agreed in",
            "  writing.",
            "• The customer agrees to return the equipment in the same condition as it was rented,",
            "  excluding normal wear and tear.",
            "• TechRental reserves the right to charge repair or replacement costs if the equipment is",
            "  returned damaged.",
            "• The rental period ends only upon the return and inspection of the equipment."
        };
        foreach (var line in terms)
            WriteLine(line, valueFont);

        y += 10;
        WriteLeftHeading("Acknowledgment");
        WriteLine("I, the undersigned, confirm that I have received the above equipment in good condition and", valueFont);
        WriteLine("agree to the terms of this rental agreement.", valueFont);

        y += lineHeight;
        WriteLabelValue("Customer Signature: ", "______________________", isSignature: true);

        var stream = new MemoryStream();
        doc.Save(stream, false);
        return stream;
    }
}
