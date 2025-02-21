using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestCaseOne.Repositories;

namespace TestCaseOne.Forms
{
    public partial class MainFormEdit : Form
    {
       private readonly string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

        private MainForm _mainForm; // Ana form referansı

        public MainFormEdit(MainForm mainForm, List<string> featureNames)
        {
            InitializeComponent();
            _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm)); // null kontrolü
            listBox1.Items.AddRange(featureNames.ToArray());
        }


        private void MainFormEdit_Load(object sender, EventArgs e)
        {
            ListeyiYukle();
        }

        private void ListeyiYukle()
        {
            listBox1.Items.Clear();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open(); // Bağlantıyı aç

                    string query = "SELECT Name FROM MainFeatures"; 
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["FeatureName"].ToString()); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string yeniOzellik = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(yeniOzellik))
            {
                MessageBox.Show("Özellik adı boş olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (FeatureExists(yeniOzellik))
            {
                MessageBox.Show("Bu isim zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO MainFeatures (FeatureName) VALUES (@name)", con))
                    {
                        cmd.Parameters.AddWithValue("@name", yeniOzellik);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Özellik başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListeyiYukle();
                            textBox1.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Ekleme başarısız oldu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ekleme sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Lütfen bir özellik seçin ve yeni adı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string eskiOzellik = listBox1.SelectedItem.ToString();
            string yeniOzellik = textBox1.Text.Trim();

            if (eskiOzellik.Equals(yeniOzellik, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Zaten aynı isimde, değişiklik yapmadınız!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (FeatureExists(yeniOzellik))
            {
                MessageBox.Show("Bu isim zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MainFeaturesRepository repo = new MainFeaturesRepository();
            if (repo.UpdateFeatureName(eskiOzellik, yeniOzellik))
            {
                _mainForm?.RefreshMainFeaturesAsync();
                this.Close();
            }
            else
            {
                MessageBox.Show("Güncelleme başarısız oldu! Eski isim bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }         

        }


        private void btnSil_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silinecek bir öğe seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string seciliOzellik = listBox1.SelectedItem.ToString();
            DialogResult result = MessageBox.Show("Bu özelliği silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        con.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM MainFeatures WHERE FeatureName = @name", con))
                        {
                            cmd.Parameters.AddWithValue("@name", seciliOzellik);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Silme işlemi başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ListeyiYukle();
                                textBox1.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Silme başarısız oldu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Silme sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool FeatureExists(string featureName)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM MainFeatures WHERE FeatureName = @name", con))
                    {
                        cmd.Parameters.AddWithValue("@name", featureName);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
