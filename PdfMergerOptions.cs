using System.Text;
using CommandLine;

namespace PdfMerger
{
    public class PdfMergerOptions
    {
        [Option('d', "Directory", Required = true) ]
        public string Directory { get; set; }

        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("PdfMerger");
            usage.AppendLine("This tool merges all pdfs files in a directory - pass the path to the source directory on the command line");
            usage.AppendLine("It outputs the the merged file to the same directory with the name merged_<timestamp>.pdf");

            return usage.ToString();
        }
    }
}