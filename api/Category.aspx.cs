using alwatnia.api;
using System;
using System.Collections.Generic;
using System.Data;

namespace alwatnia.Api_Code
{

    public partial class Category : System.Web.UI.Page
    {

        public DataAccess DA = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {

            int id = Int32.Parse(Request.QueryString["newsid"]);
            int page = Int32.Parse(Request.QueryString["Page"]);
            int Cat = Int32.Parse(Request.QueryString["Cat"]);
            int lang = Int32.Parse(Request.QueryString["lang"]);

            if (id != 0)
            {

                string a = GetNewsByID(id);

                Response.Write(a);

            }
            else
            {

                string a = ConvertDataTabletoString(10, page, Cat, lang);

                Response.Write(a);

            }



        }


        public string ConvertDataTabletoString(int rowsperpage, int page, int cat, int lang)
        {

            DataTable Dt = DataAccess.NewsPaggingAdmin(rowsperpage, page, cat, lang);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in Dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in Dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);

        }

        public string GetNewsByID(int id)
        {

            DataTable Dt = DataAccess.NewsById(id);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in Dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in Dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);

        }

    }

}
