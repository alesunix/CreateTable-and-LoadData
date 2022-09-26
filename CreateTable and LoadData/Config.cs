using System.Text;

namespace CreateTable_and_LoadData
{
    public static class Config
    {
        public static void CreateConfig()
        {
            if (!File.Exists("Config.ini"))// Если файла нет, то создаем
            {
                using (StreamWriter sw = new StreamWriter("Config.ini", false, Encoding.UTF8))
                {
                    sw.WriteLine(String.Empty);
                }
            }
            if (!File.Exists("TNSNames.ora"))
                MessageBox.Show("Отсутствует файл TNSNames.ora в конревом каталоге программы");
        }
    }
}
