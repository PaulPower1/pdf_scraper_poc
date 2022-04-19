/* Author     : Paul Power
 * Date       : 19-April-2022
 * Description: This simple program extracts text and annotations from a pdf and is essentially a POC of using 
 *              SyncFusion libraries. https://www.syncfusion.com
 * Notes      : 
 *              1. Further tests should be done to ensure changes can be written back to the pdf
 *              2. test that the drawings and graphics can be extracted and written back preserving their integrity 
 *                 (autocad can import a pdf containing drawings)
 *              3. syncfusion libraries appear to work ok - files need to be uncompressed first before manipulation???
 */

using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using System.IO;
using System;

namespace PDF_Scraper
{
 class Program
 {
  static void Main(string[] args)
  {
   var path = @"g:\mutool\pdfs\";
   var inputFilename = $"{path}\\uncompressed_sample.pdf";
   var outputAnnotationsFilename = $"{path}\\annotations.json";

   Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(GetEnvironmentVariable("SyncFusionLicenseKey"));

   //Load the PDF document - make sure you've uncompressed first otherwise you'll
   //end up with a long sequence of what appear to be random bytes :-)
   //I used a tool called muTool.exe that allowed this https://mupdf.com/
   var loadedDocument = new PdfLoadedDocument(inputFilename);

   // Extract all the text from the PDF document pages and output to separate files
   var i = 0;
   foreach (PdfLoadedPage loadedPage in loadedDocument.Pages)
   {
    File.WriteAllText($"{path}\\page_strings{i++}.txt", GetPageText(loadedPage));
   }

   //Output the pdf annotations in json format...
   ExportAnnotations(loadedDocument, outputAnnotationsFilename);

   //Close the document
   loadedDocument.Close(true);
  }

  private static string GetPageText(PdfLoadedPage page)
  {
   return page.ExtractText().ToString() ?? "";
  }

  private static void ExportAnnotations(PdfLoadedDocument doc, string outputFilename)
  {
   doc.ExportAnnotations(outputFilename, AnnotationDataFormat.Json);
  }

  private static string GetEnvironmentVariable(string name)
  {
   return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
  }

  //Todo - this is incomplete!
  private static void ExportDrawings(PdfLoadedDocument doc, string outputFilename)
  {
   foreach (PdfLoadedPage loadedPage in doc.Pages)
   {
    var extractedImages = loadedPage.ExtractImages();
    foreach (var image in extractedImages)
    {
     //Todo!
     //Do something when time permits....
    }
   }
  }
 }
}
