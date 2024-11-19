
using System.Runtime.InteropServices.ObjectiveC;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DbConnection
    {
        public SqlConnection con = new SqlConnection("Server=GABRIEL-PC\\SQLEXPRESS;Database=chatEngine;Integrated Security=True");

        public SqlConnection getCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();

            }
            return con;
        }

        public int ExeNonQuery(SqlCommand cmd)
        {
            cmd.Connection = getCon();
            int rowsaffected = -1;
            rowsaffected = cmd.ExecuteNonQuery();
            con.Close();
            return rowsaffected;
        }

        public object ExeScalar(SqlCommand cmd)
        {
            cmd.Connection = getCon();
            object obj = -1;
            obj = cmd.ExecuteScalar();
            con.Close();
            return obj;
        }

        public DataTable ExeReader(SqlCommand cmd)
        {
            cmd.Connection = getCon();
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            con.Close();
            return dt;
        }

    }

}

