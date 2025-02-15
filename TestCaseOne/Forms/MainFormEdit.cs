using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCaseOne.Forms
{
    public partial class MainFormEdit : Form
    {
        private string connectionString = "Data Source=veritabani.db;Version=3;";

        public MainFormEdit(List<string> featureNames) // Tek parametreli constructor
        {
            InitializeComponent();
            listBox1.Items.AddRange(featureNames.ToArray());
        }

        private void MainFormEdit_Load(object sender, EventArgs e)
        {
            ListeyiYukle();
        }

        // Verileri ListBox'a Yükle
        private void ListeyiYukle()
        {
            listBox1.Items.Clear();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT FeatureName FROM MainFeature", con);
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listBox1.Items.Add(reader["FeatureName"].ToString());
                }
            }
        }

        // ListBox'tan seçim yapıldığında TextBox'a getir
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }

        // Yeni Özellik Ekle
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand("INSERT INTO MainFeature (FeatureName) VALUES (@name)", con);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.ExecuteNonQuery();
                }
                ListeyiYukle();
                textBox1.Clear();
            }
            else
            {
                MessageBox.Show("Özellik adı boş olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Seçili Özelliği Güncelle
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                string eskiOzellik = listBox1.SelectedItem.ToString();
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand("UPDATE MainFeature SET FeatureName = @newName WHERE FeatureName = @oldName", con);
                    cmd.Parameters.AddWithValue("@newName", textBox1.Text);
                    cmd.Parameters.AddWithValue("@oldName", eskiOzellik);
                    cmd.ExecuteNonQuery();
                }
                ListeyiYukle();
                textBox1.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen bir özellik seçin ve yeni adı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Seçili Özelliği Sil
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string seciliOzellik = listBox1.SelectedItem.ToString();
                DialogResult result = MessageBox.Show("Bu özelliği silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        con.Open();
                        SQLiteCommand cmd = new SQLiteCommand("DELETE FROM MainFeature WHERE FeatureName = @name", con);
                        cmd.Parameters.AddWithValue("@name", seciliOzellik);
                        cmd.ExecuteNonQuery();
                    }
                    ListeyiYukle();
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir öğe seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
