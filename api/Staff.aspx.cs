using alwatnia.api;
using System;
using System.Collections.Generic;
using System.Data;

public partial class Staff : System.Web.UI.Page
{

    public DataAccess DA = new DataAccess();

    protected void Page_Load(object sender, EventArgs e)
    {

        int lang = Int32.Parse(Request.QueryString["lang"]);

        string a = GetNewsByID(lang);

        Response.Write(a);


    }



    public string GetNewsByID(int lang)
    {

        DataTable Dt = DataAccess.GetStaff(lang);

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