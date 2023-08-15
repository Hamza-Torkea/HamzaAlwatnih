using alwatnia.api;
using System;
using System.Collections.Generic;
using System.Data;

public partial class Events : System.Web.UI.Page
{

    public DataAccess DA = new DataAccess();

    protected void Page_Load(object sender, EventArgs e)
    {


        int id = Int32.Parse(Request.QueryString["id"]);
        int lang = Int32.Parse(Request.QueryString["lang"]);

        if (id != -1)
        {
            string a = GetNewsByID(id, lang);

            Response.Write(a);

        }
        else
        {

            string a = GetNewsByID(-1, lang);

            Response.Write(a);

        }


    }



    public string GetNewsByID(int id, int lang)
    {

        DataTable Dt = DataAccess.GetActivity(id, lang);

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