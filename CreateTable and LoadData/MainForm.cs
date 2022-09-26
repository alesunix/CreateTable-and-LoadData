using CreateTable_and_LoadData.Models;
using System.Data;

namespace CreateTable_and_LoadData
{
    public partial class MainForm : Form
    {
        MainModel mModel;
        ExcelModel eModel = new ExcelModel();
        public MainForm()
        {
            Config.CreateConfig();
            mModel = new MainModel();
            InitializeComponent();
            comboBoxServers.DataSource = mModel.TNSNamesParse();
            LoadConfig();
            textBoxFilter.TextChanged += TextBoxFilter_TextChanged;
            textBoxFilter.CharacterCasing = CharacterCasing.Upper;
            this.FormClosed += MainForm_FormClosed;
        }
        private void TextBoxFilter_TextChanged(object sender, EventArgs e)// Фильтр
        {
            var list = mModel.TNSNamesParse();
            if (!string.IsNullOrEmpty(textBoxFilter.Text))
            {
                string filter = textBoxFilter.Text;
                var data = from x in list
                           where x.Contains(filter)
                           select x;
                comboBoxServers.DataSource = data.ToList();
            }
            else
                comboBoxServers.DataSource = list;
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            eModel.ImportExcel();
            dataGridViewMain.DataSource = eModel.dt;
            mModel.listColumn.Clear();
            foreach (DataColumn item in eModel.dt.Columns)
            {
                mModel.listColumn.Add(item.Caption + " VARCHAR2(300)");
            }
            mModel.dt = eModel.dt.Copy();
        }
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            mModel.GenerateConn(comboBoxServers.Text);
            labelConsole.Text = mModel.ConnTest();
            if(mModel.Message == "OK")
            {
                mModel.CreateTableAndLoadData();
            }
        }
        void LoadConfig()
        {
            try
            {
                string[] index = File.ReadAllLines("Config.ini");
                comboBoxServers.Text = index[0];

                int width = int.Parse(index[1].Split(',')[0].Replace("{Width=", ""));
                int height = int.Parse(index[1].Split(',')[1].Replace(" Height=", "").Replace("}", ""));
                this.Size = new Size(width, height);

                int x = int.Parse(index[2].Split(',')[0].Replace("{X=", ""));
                int y = int.Parse(index[2].Split(',')[1].Replace("Y=", "").Replace("}", ""));
                this.Location = new Point(x, y);
            }
            catch
            {
                /// Если файл настроек пуст
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            string size = this.Size.ToString();
            string location = this.Location.ToString();
            string[] index = new string[3];

            index[0] = comboBoxServers.Text;
            index[1] = size;
            index[2] = location;
            File.WriteAllLines("Config.ini", index);
            Application.Exit();
        }
    }
}