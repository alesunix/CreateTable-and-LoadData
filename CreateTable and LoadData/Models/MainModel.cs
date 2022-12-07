using System.Data;
using System.Text.RegularExpressions;

namespace CreateTable_and_LoadData.Models
{
    internal class MainModel : BaseModel
    {
        public List<string> listColumn = new List<string>();
        public DataTable dt = new DataTable();
        public List<string> TNSNamesParse()// Парсер TNSNames.ora
        {
            List<string> DbNames = new List<string>();
            string RegEx = @"[\n][\s]*[^\(][a-zA-Z0-9_.]+[\s]*=[\s]*\(";
            FileInfo file = new FileInfo("TNSNames.ora");

            int count;
            var regex = Regex.Matches(File.ReadAllText(file.FullName), RegEx);
            for (count = 0; count < regex.Count; count++)
            {
                DbNames.Add(regex[count].Value.Trim().Substring(0, regex[count].Value.Trim().IndexOf(" ")).ToUpper());
            }
            return DbNames;
        }
        public string ConnTest()
        {
            return GetSingleResult($@"SELECT sysdate dTime, vl FROM sys_config WHERE param = 'logsat'").ToString();
        }
        public void CreateTableAndLoadData()
        {
            string columns = string.Join(", ", listColumn.ToArray());
            if (!string.IsNullOrEmpty(columns))
            {
                SetQuery($@"DROP TABLE TEMP_TABLE PURGE");
                SetQuery($@"CREATE TABLE TEMP_TABLE ({columns})");

                int count = 0;
                columns = columns.Replace(" VARCHAR2(300)", "");
                string server = СonnString.Split('=')[3];
                foreach (DataRow item in dt.Rows)
                {
                    string rowParse = string.Join(", ", item.ItemArray.Select(x => "'" + x.ToString().Replace("'", "\'\'") + "'").ToArray());
                    SetQuery($@"INSERT INTO TEMP_TABLE ({columns}) VALUES ({rowParse})");
                    count++;
                    if (Message != "OK")
                        break;
                }
                if (Message == "OK")
                    MessageBox.Show($"Таблица создана на сервере {server}. \r\nЗагружено {count} записей в базу", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show($@"Импортируйте данные из Excel", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
