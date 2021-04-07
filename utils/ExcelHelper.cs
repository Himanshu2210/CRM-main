using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM.utils
{
    public class ExcelHelper
    {
       
        public static void ExporttoExcel(DataTable table)
        {
            string guid = Guid.NewGuid().ToString()+".xls";
            string attachment = "attachment; filename="+guid;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in table.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n\n");
            int i;
            foreach (DataRow dr in table.Rows)
            {
                tab = "";
                for (i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }

        //public static void ImporttoExcell(string filename)
        //{
        //    string oleconnection ="Provider=Microsoft.ACE.OLEDB.12.0;
        //                            "Data Source='" + filename +
        //                            "';Extended Properties=\"Excel 12.0;HDR=YES;\"";
        //}
    }
}