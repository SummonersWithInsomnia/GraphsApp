using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace GraphsApp
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
            dataBindingSource.DataSource = new List<Data>();
            
            cartesianChart.AxisX.Add(new Axis
            {
                Title = "Year",
                Labels = new[] { "2017", "2018", "2019", "2020", "2021", "2022" }
            });
            
            cartesianChart.AxisY.Add(new Axis
            {
                Title = "Population",
                MinValue = 0
            });
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            cartesianChart.Series.Clear();
            SeriesCollection series = new SeriesCollection();

            var animals = (from o in dataBindingSource.DataSource as List<Data>
                select o.Animal).Distinct();

            foreach (var animal in animals)
            {
                var yearlyCounts = new Dictionary<int, double>();
                
                var animalData = dataBindingSource.DataSource as List<Data>;
                foreach (var data in animalData.Where(d => d.Animal == animal))
                {
                    if (yearlyCounts.ContainsKey(data.Year))
                    {
                        yearlyCounts[data.Year] += data.Count;
                    }
                    else
                    {
                        yearlyCounts[data.Year] = data.Count;
                    }
                }
                
                List<double> values = new List<double>();
                for (int year = 2017; year <= 2022; year++)
                {
                    values.Add(yearlyCounts.ContainsKey(year) ? yearlyCounts[year] : 0);
                }

                series.Add(new LineSeries
                {
                    Title = animal,
                    Values = new ChartValues<double>(values)
                });
            }

            cartesianChart.Series = series;
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Invalid Value", "Opps", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.Cancel = true;
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
        }
    }
}