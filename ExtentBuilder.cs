using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace Zadania
{
    static public class ExtentBuilder
    {
        static ExtentReports extent;
        public static string ReportFolderPath { get; set; }
        public static ExtentReports GetExtent()
        {
            if (extent == null)
            {
                string reportPath =
                    Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\")) 
                    + @"\Reports\report_" + DateTime.Now.ToString().Replace(':', '-');
                
                Directory.CreateDirectory(reportPath);
                ReportFolderPath = reportPath;

                string reportPathAndReportName =
                    reportPath + @"\report.html";

                var htmlReporter = new ExtentV3HtmlReporter(reportPathAndReportName);
                extent = new ExtentReports();
                extent.AttachReporter(htmlReporter);
            }

            return extent;
        }
    }
}
