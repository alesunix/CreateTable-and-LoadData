using ExcelDataReader;
using System.Data;

namespace CreateTable_and_LoadData.Models
{
    internal class ExcelModel
    {
        public DataTable dt = new DataTable();
        public void ImportExcel()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Excel|*.xlsx;*.xls"

            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                OpenExcelFile(ofd.FileName);
            }
        }
        private void OpenExcelFile(string path)
        {
            try
            {
                using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
