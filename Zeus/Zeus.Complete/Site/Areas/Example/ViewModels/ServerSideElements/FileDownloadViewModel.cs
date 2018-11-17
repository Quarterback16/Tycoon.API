using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System.ComponentModel.DataAnnotations;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.ServerSideElements
{
    [Group("File download")]
    [Button("Get CSV file!", "File download", Primary=true, ResultsInDownload=true)]
    public class FileDownloadViewModel
    {
        [Display(GroupName="File download")]
        public ContentViewModel content
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph(@"File for download can be easily produced using the FileStreamResult class. Noe the use of the ""ResultsInDownload"" attribute on the button definition. Click the button below to get a list of ten randomly generated addresses as a CSV list. Note that this uses the internal ""Address"" structure")
                    .AddPreformatted(@"    [Group(""File download"")]
    [Button(""Get CSV file!"", ""File download"", Primary=true, ResultsInDownload=true)]
    public class FileDownloadViewModel
    {
        ....
    }")
                    .AddLineBreak()
                    .AddPreformatted(@"
    // Controller action
    [HttpPost]
    public FileStreamResult FileDownload(FileDownloadViewModel model)
    {
        string[] streets = new string[] { ""Flower"", ""McManus"", ""Kingston"", ""Queen"", ""High"", ""Anzac"", ""Berrington"", ""Newberry"" };
        string[] streetTypes = new string[] { ""Street"", ""Place"", ""Crescent"", ""Parade"", ""Avenue"" };
        string[] suburbs = new string[] { ""Bonner"", ""Chifley"", ""Pyrmont"", ""Ryde"", ""Oxley"", ""Toorak"", ""Newtown"" };
        string[] states = new string[] { ""NSW"", ""ACT"", ""VIC"", ""QLD"" };
        Random random = new Random();
        var addressList = new List<Address>();
        for (int i = 1; i < 10; i++)
        {
            var a1 = (random.Next(100) + 1).ToString();
            var a2 = streets[random.Next(streets.Length)];
            var a3 = streetTypes[random.Next(streetTypes.Length)];
            var suburb = suburbs[random.Next(suburbs.Length)];
            var state = states[random.Next(states.Length)];
            var postcode = (random.Next(9000) + 1000).ToString();
            addressList.Add(new Address(a1, a2, a3, suburb, state, postcode));
        }

        var fileStream = new MemoryStream();
        var sw = new StreamWriter(fileStream);
        var csv = new CsvWriter(sw);
        csv.WriteRecords(addressList);
        sw.Flush();
        fileStream.Seek(0, SeekOrigin.Begin);
        return File(fileStream, ""text/csv"", ""Randomly generated addresses.csv"");
        //return File(fileStream, ""application/vnd.ms-excel"", ""Randomly generated addresses.xls""); // excel
   }
");

            }
        }
    }
}