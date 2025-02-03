using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestCaseOne.Repositories;

namespace TestCaseOne.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // FlowLayoutPanel ayarlarını yapılandırın
            flowAna.AutoSize = true; // FlowLayoutPanel'in otomatik boyutlanmasını aktif et
            flowAna.AutoSizeMode = AutoSizeMode.GrowAndShrink; // İçerik değiştikçe boyutlanmasın
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetMainFeatures();
        }

        void GetMainFeatures()
        {
            flowAna.Controls.Clear(); // FlowLayoutPanel'deki mevcut kontrolleri temizle

            using (MainFeaturesRepository mainFeaturesRepository = new MainFeaturesRepository())
            {
                var features = mainFeaturesRepository.GetAll(); // MainFeature tablosundan verileri çek

                foreach (var feature in features)
                {
                    Button btn = new Button();
                    btn.BackColor = Color.LightBlue; // Arka plan rengi
                    btn.FlatStyle = FlatStyle.Flat; // Daha modern bir görünüm
                    btn.Font = new Font("Arial", 11);
                    btn.ForeColor = Color.Black;
                    btn.Text = feature.Name;
                    btn.Tag = feature.Id;
                    btn.Width = 350; // Buton genişliği
                    btn.Height = 50; // Buton yüksekliği
                    btn.Margin = new Padding(3); // Butonlar arasında boşluk bırak
                    btn.Click += Button_Click; // Buton tıklama olayını ekle
                    flowAna.Controls.Add(btn); // Butonu mevcut FlowLayoutPanel'e ekle
                }
            }

            // FlowLayoutPanel boyutu güncellenir, ancak form boyutu değişmez
            flowAna.PerformLayout(); // FlowLayoutPanel kontrol düzenini güncelle
        }

        private void Button_Click(object sender, EventArgs e)
        {
            // Buton tıklama olayı işlemleri
        }
    }
}
