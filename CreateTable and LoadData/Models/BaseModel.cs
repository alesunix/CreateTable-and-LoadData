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
        public DataTable GetTable(string query)
        {
            using (OracleConnection con = new OracleConnection(СonnString))
            {
                DataTable dt = new DataTable();
                try
                {
                    con.Open();
                    var cmd = new OracleCommand(query, con);
                    OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    dt.Load(dr);
                    Message = "OK";
                    return dt;
                }
                catch (OracleException ex)
                {
                    dt.Columns.Add("Error", typeof(string));
                    dt.Rows.Add(ex.Message);
                    Message = ex.Message;
                    return dt;
                }
                finally
                {
                    Finally(con);
                }
            }
        }
        public void SetQuery(string query)
        {
            using (OracleConnection con = new OracleConnection(СonnString))
            {
                try
                {
                    con.Open();
                    var cmd = new OracleCommand(query, con).ExecuteNonQuery();
                    Message = "OK";
                }
                catch (OracleException ex)
                {
                    Message = ex.Message;
                }
                finally
                {
                    Finally(con);
                }
            }
        }
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
