using alwatnia.api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace alwatnia.Api_Code
{
    public partial class Album : Page
    {

        public DataAccess DA = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {


            string type = Request.QueryString["type"];
            string a = GetNewsByID(type);

            Response.Write(a);


        }



        public string GetNewsByID(string id)
        {

            DataTable Dt = DataAccess.GetAlbum(id);

            JavaScriptSerializer serializer =
                new JavaScriptSerializer();
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

