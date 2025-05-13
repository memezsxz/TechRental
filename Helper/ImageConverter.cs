using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

public static class ImageConverter
{
    /// <summary>
    /// Converts any supported image format (e.g., WebP, JPEG, PNG) to PNG format.
    /// </summary>
    /// <param name="inputStream">Input image stream (e.g., from uploaded file).</param>
    /// <returns>A MemoryStream containing the converted PNG image.</returns>
    public static async Task<MemoryStream> ConvertToPngAsync(Stream inputStream)
    {
        inputStream.Position = 0;

        using var image = await Image.LoadAsync(inputStream); // Auto-detect format (WebP, JPG, etc.)
        var outputStream = new MemoryStream();

        await image.SaveAsync(outputStream, new PngEncoder());

        outputStream.Position = 0; // Reset for reading
        return outputStream;
    }
}
