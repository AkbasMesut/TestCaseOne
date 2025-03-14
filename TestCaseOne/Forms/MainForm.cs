using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestCaseOne.Models;
using TestCaseOne.Repositories;

namespace TestCaseOne.Forms
{
    public partial class MainForm : Form
    {
        private static readonly Font DefaultFont = new Font("Arial", 12);
        private static readonly Color DefaultBackColor = Color.LightBlue;

        public MainForm()
        {
            InitializeComponent();
            SetupDataGridView();
            //this.Bounds = Screen.PrimaryScreen.Bounds; // Ekranı tamamen kaplar
            // this.FormBorderStyle = FormBorderStyle.None; // Kenarlıkları kaldır
            this.WindowState = FormWindowState.Maximized; // Formu tam ekran aç
           // this.StartPosition = FormStartPosition.CenterScreen; // Formu ekranın ortasına getir
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.GridColor = Color.White;
            dataGridView1.VirtualMode = false;
            await GetMainFeaturesAsync();
            dataGridView1.ClearSelection();
        }

        #region DataGridView Setup

        private void SetupDataGridView()
        {
            ConfigureDataGridViewStyle();
            BindDataGridViewEvents();
            AddDataGridViewColumns();
        }

        private void ConfigureDataGridViewStyle()
        {
            dataGridView1.VirtualMode = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.DefaultCellStyle.Font = DefaultFont;
            dataGridView1.DefaultCellStyle.BackColor = DefaultBackColor;
        }

        private void BindDataGridViewEvents()
        {
            dataGridView1.RowsAdded -= DataGridView1_RowsAdded;
            dataGridView1.RowsAdded += DataGridView1_RowsAdded;
            dataGridView1.CellPainting -= DataGridView1_CellPainting;
            dataGridView1.CellPainting += DataGridView1_CellPainting;
        }

        private void AddDataGridViewColumns()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("UserStoryName", "Test Adı");
            dataGridView1.Columns.Add("Info", "Açıklama");
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["UserStoryName"].FillWeight = 70;
            dataGridView1.Columns["Info"].FillWeight = 30;
        }
        #endregion

        #region DataGridView Events
        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);
                using (StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoWrap
                })
                {
                    e.Graphics.DrawString(e.FormattedValue?.ToString(), e.CellStyle.Font, new SolidBrush(e.CellStyle.ForeColor), e.CellBounds, format);
                    e.Handled = true;
                }
            }
        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                AdjustRowHeight(row);
            }
        }

        private void AdjustRowHeight(DataGridViewRow row)
        {
            int padding = 5;
            int maxHeight = 0;
            foreach (DataGridViewCell cell in row.Cells)
            {
                int cellHeight = CalculateCellHeight(cell, padding);
                maxHeight = Math.Max(maxHeight, cellHeight);
            }
            row.Height = Math.Max(row.Height, maxHeight);
        }

        private int CalculateCellHeight(DataGridViewCell cell, int padding)
        {
            if (cell.Value != null)
            {
                string text = cell.Value.ToString();
                Size textSize = TextRenderer.MeasureText(text, cell.Style.Font ?? DefaultFont);
                int lines = (int)Math.Ceiling((double)textSize.Width / (cell.Size.Width - padding));
                return (textSize.Height * lines) + padding;
            }
            return cell.Size.Height;
        }
        #endregion

        #region Load Main Features and Test Cases
        private async Task GetMainFeaturesAsync()
        {
            try
            {
                flowAna.Controls.Clear();
                using (MainFeaturesRepository mainFeaturesRepository = new MainFeaturesRepository())
                {
                    var features = await mainFeaturesRepository.GetAllAsync();
                    foreach (var feature in features)
                    {
                        CreateFeatureButton(feature.FeatureName, feature.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void CreateFeatureButton(string featureName, int featureId)
        {
            Button btn = new Button
            {
                BackColor = DefaultBackColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 11),
                ForeColor = Color.Black,
                Text = featureName,
                Tag = featureId,
                Width = 360,
                Height = 50,
                Margin = new Padding(0)
            };
            btn.FlatAppearance.BorderColor = Color.White;
            btn.Click += FeatureButton_Click;
            flowAna.Controls.Add(btn);
        }

        private async void FeatureButton_Click(object sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag != null)
            {
                int featureId = (int)clickedButton.Tag;
                await LoadTestCasesAsync(featureId);
            }
        }

        private async Task LoadTestCasesAsync(int featureId)
        {
            var userStories = await FetchTestCasesAsync(featureId);
            UpdateDataGridView(userStories);
        }

        private async Task<List<UserStory>> FetchTestCasesAsync(int featureId)
        {
            using (UserStoryRepositories userStory = new UserStoryRepositories())
            {
                return await userStory.GetAllByModulIdAsync(featureId);
            }
        }

        private void UpdateDataGridView(List<UserStory> userStories)
        {
            dataGridView1.Rows.Clear();
            foreach (var userStori in userStories)
            {
                dataGridView1.Rows.Add(userStori.Id, userStori.UserStoryName, userStori.Info);
            }
            AdjustDataGridViewHeight();
            groupBox1.Visible = dataGridView1.Rows.Count > 0;
            dataGridView1.ClearSelection();
        }

        private void AdjustDataGridViewHeight()
        {
            int headerHeight = dataGridView1.ColumnHeadersHeight;
            int rowHeight = dataGridView1.RowTemplate.Height;
            int rowCount = dataGridView1.Rows.Count;
            int totalHeight = headerHeight + (rowCount * rowHeight) + 2;
            int maxHeight = 500;
            dataGridView1.Height = Math.Min(totalHeight, maxHeight);
            groupBox1.Height = dataGridView1.Height + 20;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MainFeaturesRepository repository = new MainFeaturesRepository();
            List<string> featureNames = repository.GetFeatureNames();

            if (featureNames.Count > 0)
            {
                MainFormEdit listForm = new MainFormEdit(this, featureNames); // Ana form referansını gönder
                listForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veritabanında özellik bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UserStoryRepositories repo = new UserStoryRepositories();
            List<string> storyNames = repo.GetUserStoryNames();

            if(storyNames.Count > 0)
            {
                UserStoryFormEdit listForm = new UserStoryFormEdit(this, storyNames);
                listForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veritabanında özellik bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        public async Task RefreshMainFeaturesAsync()
        {
            await GetMainFeaturesAsync(); // Butonları güncellemek için mevcut metodu çağır
        }

    }
}
