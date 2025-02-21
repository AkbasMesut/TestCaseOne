using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestCaseOne.Models;


namespace TestCaseOne.Repositories
{
    internal class MainFeaturesRepository :BaseRepositories
    {
        public MainFeaturesRepository()
        {
        }

        // Async versiyonu
        public async Task<List<MainFeatures>> GetAllAsync()
        {
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT Id,FeatureName FROM MainFeatures";
                var result = await conn.QueryAsync<MainFeatures>(query);
                return result.ToList();
            }
        }

        // Async olarak GetDataById metodu
        public async Task<MainFeatures> GetDataByIdAsync(int id)
        {
            var parameters = new { Id = id };
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM MainFeatures WHERE MainFeatureId = @Id";
                return await conn.QueryFirstOrDefaultAsync<MainFeatures>(query, parameters);
            }
        }

        public List<string> GetFeatureNames()
        {
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                conn.Open();
                var query = "SELECT FeatureName FROM MainFeatures";
                return conn.Query<string>(query).ToList();
            }
        }

        public bool UpdateFeatureName(string oldName, string newName)
        {
            using (SQLiteConnection con = new SQLiteConnection(connection.ConnectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("UPDATE MainFeatures SET FeatureName = @newName WHERE FeatureName COLLATE NOCASE = @oldName", con))
                    {
                        cmd.Parameters.AddWithValue("@newName", newName);
                        cmd.Parameters.AddWithValue("@oldName", oldName);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        MessageBox.Show($"Güncellenen satır sayısı: {rowsAffected}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }


    }
}
