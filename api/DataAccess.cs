using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace alwatnia.api
{
    public class DataAccess
    {
        public DataAccess()
        {

        }

        private static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataModel"].ToString());
            conn.Open();
            return conn;
        }

        public int AddContact(string type, string name, string email, string msg, string mobile)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Close();
                SqlCommand cmd = new SqlCommand("AddContact", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@msg", msg);
                cmd.Parameters.AddWithValue("@mobile", mobile);
                cmd.CommandTimeout = 0;
                conn.Open();
                int Result = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                return Result;
            }
        }

        public static DataTable NewsPaggingAdmin(int RowsPerPage, int Page, int CategoryID, int Langid)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("NewsPaggingAdmin", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@RowsPerPage", RowsPerPage);
                sa.SelectCommand.Parameters.AddWithValue("@Page", Page);
                if (CategoryID != -1)
                    sa.SelectCommand.Parameters.AddWithValue("@CategoryID", CategoryID);
                sa.SelectCommand.Parameters.AddWithValue("@Langid", Langid);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetCompanyNews(int id, int lang)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetCompanyNews", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);
                sa.SelectCommand.Parameters.AddWithValue("@lang", lang);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetCompanyProducts(int id, int lang)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetCompanyProducts", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);
                sa.SelectCommand.Parameters.AddWithValue("@lang", lang);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetCompanyReports(int id, int lang)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetCompanyReports", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);
                sa.SelectCommand.Parameters.AddWithValue("@lang", lang);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetCompanyAll(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetCompany", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (id != -1)
                    sa.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetCompanyImage(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetCompanyImage", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable ExhibtionPagging(int RowsPerPage, int Page, int type)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("ExhibtionPagging", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@RowsPerPage", RowsPerPage);
                sa.SelectCommand.Parameters.AddWithValue("@Page", Page);
                sa.SelectCommand.Parameters.AddWithValue("@type", type);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetImagesByID(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetImagesByID", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GeImageByPageId(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GeImageByPageId", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetActivity(int id, int lang)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetActivity", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (id != -1)
                    sa.SelectCommand.Parameters.AddWithValue("@id", id);
                sa.SelectCommand.Parameters.AddWithValue("@lang", lang);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetStaff(int lang)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetStaff", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@lang", lang);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetReport(int lang)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GetReport", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@lang", lang);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GetAlbum(string type)
        {
            using (SqlConnection conn = GetConnection())
            {

                SqlDataAdapter sa = new SqlDataAdapter("GetAlbum", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@type", type);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable GeImageByTag(string tag, int lang)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("GeImageByTag", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@tag", tag);
                sa.SelectCommand.Parameters.AddWithValue("@lang", lang);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable NewsById(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("NewsById", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable UpdateNewsCounter(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter sa = new SqlDataAdapter("UpdateNewsCounter", conn);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable DT = new DataTable();
                sa.Fill(DT);
                return DT;
            }
        }

        public static DataTable ListTopic(int page, int pagesize, int Parent_id, int Category_ID, int DDL1_ID)
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlDataAdapter DA = new SqlDataAdapter("ListTopic", conn);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;

                DA.SelectCommand.Parameters.AddWithValue("@page", page);
                DA.SelectCommand.Parameters.AddWithValue("@pagesize", pagesize);
                DA.SelectCommand.Parameters.AddWithValue("@Parent_id", Parent_id);
                DA.SelectCommand.Parameters.AddWithValue("@Category_ID", Category_ID);
                DA.SelectCommand.Parameters.AddWithValue("@DDL1_ID", DDL1_ID);

                DataTable DT = new DataTable();
                DA.Fill(DT);
                return DT;
            }
        }

    }
}