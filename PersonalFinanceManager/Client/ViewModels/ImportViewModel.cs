using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components.Forms;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Helpers.CSV.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class ImportViewModel : IViewModel
    {
        public async Task OnInit()
        {
        }

        public async Task OnFileImported(InputFileChangeEventArgs eventArgs)
        {
            var browserFile = eventArgs.File;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };


            //The following approach is NOT recommended because the file's Stream content is read into a String in memory (reader):
            //    https://docs.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-5.0&pivots=webassembly

            //using (var reader = new StreamReader(browserFile.Ope()))
            //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            //{
            //    var statements = csv.GetRecords<LuminorStatement>().ToList();

            //    foreach (var statement in statements)
            //    {
            //        Debug.WriteLine(statement.Amount);
            //    }
            //}

            using (var stream2 = new MemoryStream())
            {
                await browserFile.OpenReadStream().CopyToAsync(stream2);   // although file.Data is itself a stream, using it directly causes "synchronous reads are not supported" errors below.
                stream2.Seek(0, SeekOrigin.Begin);      // at the end of the copy method, we are at the end of both the input and output stream and need to reset the one we want to work with.
                var reader = new System.IO.StreamReader(stream2);

                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var statements = csv.GetRecords<LuminorStatement>().ToList();

                    foreach (var statement in statements)
                    {
                        Debug.WriteLine(statement.Amount);
                    }
                }
            }



        }
    }
}
