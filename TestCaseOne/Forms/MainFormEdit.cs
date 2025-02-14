using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCaseOne.Forms
{
    public partial class MainFormEdit : Form
    {
        public MainFormEdit(List<string> featureNames)
        {
            InitializeComponent();
            listBox1.Items.AddRange(featureNames.ToArray());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFeature = listBox1.SelectedItem.ToString();
            MessageBox.Show($"Seçilen Özellik: {selectedFeature}");
        }
    }
}
