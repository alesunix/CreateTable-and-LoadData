using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace CreateTable_and_LoadData.Models
{
    public class BaseModel
    {
        public string Message { get; set; } = "OK";
        public static string СonnString { get; set; }
        public void GenerateConn(string sid)
        {
            var connString = new OracleConnectionStringBuilder();
            connString.UserID = "Login";
            connString.Password = "Password";
            connString.DataSource = sid;
            СonnString = connString.ToString();
        }
        /// <summary>
        /// Выполняет sql-выражение ExecuteNonQuery. Подходит для INSERT, UPDATE, DELETE, CREATE
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Возвращает количество измененных записей</returns>
        public int SetQuery(string query)
        {
            using (OracleConnection con = new OracleConnection(СonnString))
            {
                try
                {
                    con.Open();
                    var cmd = new OracleCommand(query, con).ExecuteNonQuery();
                    Message = "OK";
                    return cmd;
                }
                catch (OracleException ex)
                {
                    Message = ex.Message;
                    return -1;
                }
                finally
                {
                    Finally(con);
                }
            }
        }
        /// <summary>
        /// Выполняет sql-выражение ExecuteScalar. Подходит для sql-выражения SELECT в паре с одной из встроенных функций SQL, например, Min, Max, Sum, Count.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Возвращает object - первый стобец первой строки</returns>
        public object GetSingleResult(string query)
        {
            using (OracleConnection con = new OracleConnection(СonnString))
            {
                try
                {
                    con.Open();
                    var cmd = new OracleCommand(query, con).ExecuteScalar();
                    Message = "OK";
                    return cmd;
                }
                catch (OracleException ex)
                {
                    return Message = ex.Message;
                }
                finally
                {
                    Finally(con);
                }
            }
        }
        private void Finally(OracleConnection con)
        {
            con.Close();
            con.Dispose();
            if (Message != "OK")
                MessageBox.Show($"{Message}", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
