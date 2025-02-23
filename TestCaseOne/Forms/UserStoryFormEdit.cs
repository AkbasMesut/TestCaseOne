using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCaseOne.Forms
{
    public partial class UserStoryFormEdit : Form
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

        private MainForm _mainForm; // Ana form referansı
        public UserStoryFormEdit(MainForm mainForm, List<string> storyNames)
        {
            InitializeComponent();
            _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm)); // null kontrolü
            comboBox1.Items.AddRange(storyNames.ToArray());
        }

        private void UserStoryFormEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
