using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WpfApp3
{
    internal static class PDFImporter
    {
        internal static async Task<byte[]> ImportPDF(string filename)
        {
            var f = await StorageFile.GetFileFromPathAsync(filename);
            using (var stream = await f.OpenReadAsync())
            {
                var d = await PdfDocument.LoadFromStreamAsync(stream);
                using (var page0 = d.GetPage(0))
                {
                    byte[] content;
                    await page0.PreparePageAsync();
                    using (var randomAccessStream = new InMemoryRandomAccessStream())
                    {
                        await page0.RenderToStreamAsync(randomAccessStream, new PdfPageRenderOptions { BitmapEncoderId = BitmapEncoder.PngEncoderId });
                        content = new byte[randomAccessStream.Size];
                        await randomAccessStream.ReadAsync(content.AsBuffer(), (uint)randomAccessStream.Size, InputStreamOptions.None);
                        return content;
                    }
                }
            }
        }
    }
}
