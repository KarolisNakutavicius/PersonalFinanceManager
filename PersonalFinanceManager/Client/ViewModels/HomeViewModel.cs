using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Util;
using PersonalFinanceManager.Client.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class HomeViewModel : IViewModel
    {
        private const int InitalCount = 7;
        private static readonly Random _rng = new Random();

        public BarConfig Config { get; set; } = new BarConfig
        {
            Options = new BarOptions
            {
                Responsive = true,
                Legend = new Legend
                {
                    Position = Position.Top
                },
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = "ChartJs.Blazor Bar Chart"
                }
            }
        };
        public Chart Chart { get; set; }

        public async Task OnInit()
        {
            IDataset<int> dataset1 = new BarDataset<int>(RandomScalingFactor(InitalCount))
            {
                Label = "My first dataset",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.Red)),
                BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                BorderWidth = 1
            };

            IDataset<int> dataset2 = new BarDataset<int>(RandomScalingFactor(InitalCount))
            {
                Label = "My second dataset",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.Blue)),
                BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                BorderWidth = 1
            };

            ((List<string>)Config.Data.Labels).AddRange(Months.Take(InitalCount));
            Config.Data.Datasets.Add(dataset1);
            Config.Data.Datasets.Add(dataset2);
        }

        public static IReadOnlyList<string> Months { get; } = new ReadOnlyCollection<string>(new[]
{
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        });

        public static IEnumerable<int> RandomScalingFactor(int count)
        {
            int[] factors = new int[count];
            lock (_rng)
            {
                for (int i = 0; i < count; i++)
                {
                    factors[i] = RandomScalingFactorThreadUnsafe();
                }
            }

            return factors;
        }

        private static int RandomScalingFactorThreadUnsafe() => _rng.Next(-100, 100);
    }
}
