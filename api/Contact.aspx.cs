using alwatnia.api;
using System;
using System.Collections.Generic;
using System.Data;

public partial class Contact : System.Web.UI.Page
{

    public DataAccess DA = new DataAccess();

    protected void Page_Load(object sender, EventArgs e)
    {

        string type = Request.QueryString["type"];
        string name = Request.QueryString["name"];
        string email = Request.QueryString["email"];
        string msg = Request.QueryString["msg"];
        string mobile = Request.QueryString["mobile"];


        string a = GetNewsByID(type, name, email, msg, mobile);

        Response.Write(a);


    }



    public string GetNewsByID(string type, string name, string email, string msg, string mobile)
    {

        DataTable Dt = new DataTable();

        int a = DA.AddContact(type, name, email, msg, mobile);

        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;

        row = new Dictionary<string, object>();

        row.Add("Message", "تمت الاضافة بنجاح");

        rows.Add(row);

        return serializer.Serialize(rows);

    }

}