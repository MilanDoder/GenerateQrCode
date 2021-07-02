using Aspose.Words;
using Aspose.Words.Tables;

using QRCoder;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode;
using Spire.Barcode;
using SkiaSharp;
using GemBox.Document;
using Section = GemBox.Document.Section;
using Paragraph = GemBox.Document.Paragraph;
using SkiaSharp.QrCode;
using QRCodeGenerator = SkiaSharp.QrCode.QRCodeGenerator;

namespace AplicationQR.Elasticsearch
{
    public class FileCreator
    {

        public static void createSimplefile(string content) {
            string path = @"~\MyTest.txt";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(content);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public static void createDocxFileMethod(string content)
        {
            string path = @"~\MyTest.docx";
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Specify font formatting
            Aspose.Words.Font font = builder.Font;
            font.Size = 32;
            font.Bold = true;
            font.Color = System.Drawing.Color.Black;
            font.Name = "Arial";
            font.Underline = Underline.Single;

            // Insert text
            builder.Writeln("This is the first page.");
            builder.Writeln();

            // Change formatting for next elements.
            font.Underline = Underline.None;
            font.Size = 10;
            font.Color = System.Drawing.Color.Blue;

            builder.Writeln("This following is a table");
            // Insert a table
            Table table = builder.StartTable();
            // Insert a cell
            builder.InsertCell();
            // Use fixed column widths.
            table.AutoFit(AutoFitBehavior.AutoFitToContents);
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("This is row 1 cell 1");
            // Insert a cell
            builder.InsertCell();
            builder.Write("This is row 1 cell 2");
            builder.EndRow();
            builder.InsertCell();
            builder.Write("This is row 2 cell 1");
            builder.InsertCell();
            builder.Write("This is row 2 cell 2");
            builder.EndRow();
            builder.EndTable();
            builder.Writeln();


            // Insert image
            SKBitmap mapImg = createQr(content);


            builder.InsertImage(mapImg);
            // Insert page break 
            builder.InsertBreak(BreakType.PageBreak);
            // all the elements after page break will be inserted to next page.

            // Save the document
            doc.Save(path);
        }

        public static void createPdfFileMethod(string content)
        {
            string path = @"~\MyTest.pdf";
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Specify font formatting
           

            // Insert text
            builder.Writeln("This is the first page.");
            builder.Writeln();

            // Change formatting for next elements.

            builder.Writeln("This following is a table");
            // Insert a table
            Table table = builder.StartTable();
            // Insert a cell
            builder.InsertCell();
            // Use fixed column widths.
            table.AutoFit(AutoFitBehavior.AutoFitToContents);
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("This is row 1 cell 1");
            // Insert a cell
            builder.InsertCell();
            builder.Write("This is row 1 cell 2");
            builder.EndRow();
            builder.InsertCell();
            builder.Write("This is row 2 cell 1");
            builder.InsertCell();
            builder.Write("This is row 2 cell 2");
            builder.EndRow();
            builder.EndTable();
            builder.Writeln();


            // Insert image
            SKBitmap mapImg = createQr(content);


            builder.InsertImage(mapImg);
            // Insert page break 
            builder.InsertBreak(BreakType.PageBreak);
            // all the elements after page break will be inserted to next page.

            // Save the document
            doc.Save(path);
        }

        public static void createPdfFile(string content)
        {
            string folder = @"~";

            string path = @"~\MyTest.pdf";
            BarcodeSettings settings = new BarcodeSettings();
            settings.Type = BarCodeType.QRCode;
            settings.Data = content;
            //settings.ForeColor = Color.Red;

            //settings.QRCodeDataMode = QRCodeDataMode.AlphaNumber;
            //settings.X = 1.0f;
            //settings.QRCodeECL = QRCodeECL.H;
            BarCodeGenerator generator = new BarCodeGenerator(settings);
            
            
            SKImage snapI = generator.GenerateImage();
            SKData pngImage = snapI.Encode();
            var fullpath = folder + "PicName.png";
            File.WriteAllBytes(fullpath, pngImage.ToArray());
            SKBitmap bitmap = SKBitmap.Decode(fullpath);





        }

        private static SKBitmap createQr(string content ) {
            string folder = @"~";

            string path = @"~\MyTest.pdf";
            string name = createQrImage(content);
            SKBitmap bitmap = SKBitmap.Decode($"{folder}{name}.png");
            File.Delete($"{folder}{name}.png");
            return bitmap;
        }

        public static void createQrToPodf(string content) {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // Create a new document.
            var document = new DocumentModel();

            var qrCodeValue = "123456789";
            var qrCodeField = new Field(document, FieldType.DisplayBarcode, $"{qrCodeValue} QR");


            var qrCodeValue1 = "www.google.com";
            var qrCodeField1 = new Field(document, FieldType.DisplayBarcode, $"{qrCodeValue1} QR");

            var qrCodeValue2 = "";
            var qrCodeField2 = new Field(document, FieldType.DisplayBarcode, $"{qrCodeValue2} QR");
            string s = "" + content;
            var qrCodeValue3 = content;
            var qrCodeField3 = new Field(document, FieldType.DisplayBarcode, $"{s} QR");

            document.Sections.Add(
                new Section(document,
                    new Paragraph(document, "QR Code"),
                    new Paragraph(document, qrCodeField),
                    new Paragraph(document, qrCodeField1),
                    new Paragraph(document, qrCodeField2),
                    new Paragraph(document, qrCodeField3)));

            document.Save(@"C:\JDeveloper\CGApp\Materijali\File\QRCodeOutput.pdf");

        }


        public static string createQrImage(string content) {
            string name = "qrCode";
            string folder = @"~";

            using var generator = new QRCodeGenerator();

            var level = ECCLevel.H;
            var qr = generator.CreateQrCode(content, level);

            var info = new SKImageInfo(75, 75);
            using var surface = SKSurface.Create(info);

            var canvas = surface.Canvas;
            canvas.Render(qr, 75, 75);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(@$"{folder}{name}.png");
            data.SaveTo(stream);
            return name;
        }

     
    }
}
