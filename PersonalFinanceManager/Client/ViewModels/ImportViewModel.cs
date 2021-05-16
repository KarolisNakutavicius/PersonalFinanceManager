using CsvHelper;
using CsvHelper.Configuration;
using FileHelpers;
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

            var engine = new FileHelperEngine(typeof(LuminorStatement));

            using (var reader = new StreamReader(browserFile.OpenReadStream()))
            {
                string content = await reader.ReadToEndAsync();

                var statements = engine.ReadStringAsList(content);

                foreach (var statement in statements)
                {
                    Debug.WriteLine(((LuminorStatement)statement).Amount);
                }
            }

        }
    }
}
