using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CommandLine;

namespace PdfMerger
{
    public class PdfMerger
    {
        public static int Main(string[] args)
        {
            var pdfMergerOptions = new PdfMergerOptions();
            if (!Parser.Default.ParseArguments(args, pdfMergerOptions))
            {
                Console.WriteLine(pdfMergerOptions.GetUsage());
                return -1;
            }
            
            var program = new PdfMerger();
            program.GetFilesAndMerge(pdfMergerOptions.Directory, pdfMergerOptions.Directory);

            return 0;
        }

        public void GetFilesAndMerge(string sourceDirectory, string targetDirectory)
        {
            try
            {
                var fileContents = new List<byte[]>();
                var files = Directory.GetFiles(sourceDirectory, "*.pdf").ToList();
                files.Sort();

                foreach (var file in files)
                {
                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        var bytes = new byte[fileStream.Length];
                        fileStream.Read(bytes, 0, (int) fileStream.Length);
                        fileContents.Add(bytes);
                    }
                }

                var pdfMerger = new PdfMerger();
                var mergedPdf = pdfMerger.MergeFiles(fileContents);

                var mergedFilePath = Path.Combine(targetDirectory,
                    string.Format("merged_{0}.pdf", DateTime.Now.ToFileTime()));

                Console.WriteLine("Merged file will be written to here: {0}", mergedFilePath);

                using (var file = new FileStream(mergedFilePath, FileMode.Create, FileAccess.Write))
                {
                    file.Write(mergedPdf, 0, mergedPdf.Length);
                    file.Flush();
                    file.Close();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error caught while merging PDF files. Message from exception was {0}", err.Message);
                throw;
            }
        }

        public byte[] MergeFiles(List<byte[]> sourceFiles)
        {
            var document = new Document();
            var output = new MemoryStream();
            
            // Initialize pdf writer
            var writer = PdfWriter.GetInstance(document, output);
            writer.PageEvent = new PdfPageEvents();

            // Open document to write
            document.Open();
            var content = writer.DirectContent;

            // Iterate through all pdf documents
            foreach (byte[] sourceFile in sourceFiles)
            {
                var reader = new PdfReader(sourceFile);
                var numberOfPages = reader.NumberOfPages;

                // Iterate through all pages
                for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                {
                    // Determine page size for the current page
                    document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex));

                    // Create page
                    document.NewPage();
                    var importedPage = writer.GetImportedPage(reader, currentPageIndex);


                    // Determine page orientation
                    var pageOrientation = reader.GetPageRotation(currentPageIndex);
                    if ((pageOrientation == 90) || (pageOrientation == 270))
                    {
                        content.AddTemplate(importedPage, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(currentPageIndex).Height);
                    }
                    else
                    {
                        content.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                    }
                }
            }
            
            document.Close();
            
            return output.GetBuffer();
        }
    }
}
