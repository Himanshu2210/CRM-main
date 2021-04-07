using CRM.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using CRM.Models;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.IO;

/*
        Author:Sagar Srivastava,
        Aim:Contains code for CRM CRUD operation,
        Change Log: code ,developer name ,task description ,date
       =========================================================================================
        #CC01 ,Chandan Singh ,update user list ,05-feb-2021
        #CC02 ,Chandan Singh , User search list no record found massage show,04-feb-2021
        #CC03 ,Chandan Singh , after dwonload excel field same as an user liat,04-feb-2021
        #CC04 ,Chandan Singh , use encrypt technique for user password ,04-feb-2021
 */
namespace CRM.Services
{
    public class AdminService
    {
        public UserDetail GetRoles(user_Model userData = null)
        {
            DataSet ds=null;
            UserDetail detail = new UserDetail();
            try
            {
                using (SqlConnection db= ConnectionHelper.getConnection())
                {
                    db.Open();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@role", userData.userType);
                    ds = SqlHelper.ExecuteDataset(db ,CommandType.StoredProcedure , "prcGetRoles" ,param);

                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                RoleModel obj = new RoleModel();
                                if (!string.IsNullOrEmpty(dr["RoleId"].ToString()))
                                {
                                    obj.roleId = Convert.ToInt32(dr["RoleId"].ToString());
                                }
                                else
                                {
                                    obj.roleId = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["role"].ToString()))
                                {
                                    obj.role = dr["role"].ToString();
                                }
                                else
                                {
                                    obj.role = "";
                                }

                                detail.roleList.list.Add(obj);
                            }
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {

            }
            return detail;
        }
        public user_List GetUserList(user_Model userData = null ,int mode = 0 ,string filter="" ,string datefilter="" ,int operationId = 0 ,int isExport=0 )
        {
            DataSet ds = null;
            user_List detail = new user_List();
            if (string.IsNullOrEmpty(datefilter))
            {
                datefilter = DateTime.UtcNow.ToString();
            }
            try
            {
                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    ds = new DataSet();
                    DateTime x = Convert.ToDateTime(datefilter);
                    SqlParameter[] param = new SqlParameter[6];
                    param[0] = new SqlParameter("@mode", mode);
                    param[1] = new SqlParameter("@filter" ,filter);
                    param[2] = new SqlParameter("@userid", userData.userId);
                    param[3] = new SqlParameter("@role", userData.userType);
                    param[4] = new SqlParameter("@datefilter", Convert.ToDateTime(datefilter));
                    param[5] = new SqlParameter("@isExport", isExport);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcGetUsers", param);
                    if (operationId == 1)
                    {
                        utils.ExcelHelper.ExporttoExcel(ds.Tables[0]);
                    }
                    detail.filter = filter;
                    detail.dateFilter = datefilter;
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    user_Model obj = new user_Model();
                                    if (!string.IsNullOrEmpty(dr["UserId"].ToString()))
                                    {
                                        obj.userId = Convert.ToInt32(dr["UserId"].ToString());
                                    }
                                    else
                                    {
                                        obj.userId = 0;
                                    }
                                    if (!string.IsNullOrEmpty(dr["UserName"].ToString()))
                                    {
                                        obj.username = dr["UserName"].ToString();
                                    }
                                    else
                                    {
                                        obj.username = "";
                                    }
                                    if (mode != 0)
                                    {
                                        if (!string.IsNullOrEmpty(dr["email"].ToString()))
                                        {
                                            obj.email = dr["email"].ToString();
                                        }
                                        else
                                        {
                                            obj.email = "";
                                        }
                                        if (!string.IsNullOrEmpty(dr["phNo"].ToString()))
                                        {
                                            obj.phNo = Convert.ToUInt64(dr["phNo"].ToString());
                                        }
                                        else
                                        {
                                            obj.phNo = 0;
                                        }
                                        if (!string.IsNullOrEmpty(dr["whatsappPhNo"].ToString()))
                                        {
                                            obj.whatsappPhNo = Convert.ToUInt64(dr["whatsappPhNo"].ToString());
                                        }
                                        else
                                        {
                                            obj.whatsappPhNo = 0;
                                        }
                                        /*#CC03  added start*/
                                        //if (!string.IsNullOrEmpty(dr["roleid"].ToString()))
                                        //{
                                        //    obj.userType = Convert.ToInt32(dr["roleid"]);
                                        //}
                                        //else
                                        //{
                                        //    obj.userType = 0;
                                        //}
                                        /*#CC03  added start*/
                                        if (!string.IsNullOrEmpty(dr["userType"].ToString()))
                                        {
                                            obj.role = dr["userType"].ToString();
                                        }
                                        else
                                        {
                                            obj.role = "";
                                        }
                                        if (!string.IsNullOrEmpty(dr["skypeId"].ToString()))
                                        {
                                            obj.skypeId = dr["skypeId"].ToString();
                                        }
                                        else
                                        {
                                            obj.skypeId = "";
                                        }
                                        if (!string.IsNullOrEmpty(dr["status"].ToString()))
                                        {
                                            obj.status = dr["status"].ToString();
                                        }
                                        else
                                        {
                                            obj.status = "";
                                        }
                                    }

                                    detail.list.Add(obj);
                                }

                            }
                            else
                            {
                                detail.message = "No such record found.";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return detail;
        }

        public UserEditModel GetUsers(user_Model userData=null ,int mode=0 )
        {
            DataSet ds = null;
            UserEditModel detail = new UserEditModel();
            try
            {
                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@mode" ,mode);
                    param[1] = new SqlParameter("@userid", userData.userId);
                    param[2] = new SqlParameter("@role", userData.userType);

                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcGetUsers" ,param);

                    if (ds.Tables.Count > 0)
                    {
                   
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                //detail obj = new user_Model();
                                if (!string.IsNullOrEmpty(dr["userid"].ToString()))
                                {
                                    detail.userDetail.userId = Convert.ToInt32(dr["userid"].ToString());
                                }
                                else
                                {
                                    detail.userDetail.userId = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["username"].ToString()))
                                {
                                    detail.userDetail.username = dr["username"].ToString();
                                }
                                else
                                {
                                    detail.userDetail.username = "";
                                }
                            if (!string.IsNullOrEmpty(dr["email"].ToString()))
                            {
                                detail.userDetail.email = dr["email"].ToString();
                            }
                            else
                            {
                                detail.userDetail.email = "";
                            }
                            if (!string.IsNullOrEmpty(dr["phoneno"].ToString()))
                            {
                                detail.userDetail.phNo = Convert.ToUInt64(dr["phoneno"].ToString());
                            }
                            else
                            {
                                detail.userDetail.phNo = 0;
                            }
                            if (!string.IsNullOrEmpty(dr["whatsappPhNo"].ToString()))
                            {
                                detail.userDetail.whatsappPhNo = Convert.ToUInt64(dr["whatsappPhNo"].ToString());
                            }
                            else
                            {
                                detail.userDetail.whatsappPhNo = 0;
                            }
                            if (!string.IsNullOrEmpty(dr["userType"].ToString()))
                            {
                                detail.userDetail.userType = Convert.ToInt32(dr["userType"].ToString());
                            }
                            else
                            {
                                detail.userDetail.userType = 0;
                            }
                            if (!string.IsNullOrEmpty(dr["skypeId"].ToString()))
                            {
                                detail.userDetail.skypeId = dr["skypeId"].ToString();
                            }
                            else
                            {
                                detail.userDetail.skypeId = "";
                            }
                            if (!string.IsNullOrEmpty(dr["password"].ToString()))
                            {
                                detail.userDetail.password = DecodeFrom64(dr["password"].ToString());
                            }
                            else
                            {
                                detail.userDetail.password = "";
                            }
                           
                          }
                        if (mode == 3)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                RoleModel obj = new RoleModel();
                                if (!string.IsNullOrEmpty(dr["roleId"].ToString()))
                                {
                                    obj.roleId = Convert.ToInt32(dr["roleId"].ToString());
                                }
                                else
                                {
                                    obj.roleId = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["role"].ToString()))
                                {
                                    obj.role = dr["role"].ToString();
                                }
                                else
                                {
                                    obj.role = "";
                                }

                                detail.list.Add(obj);
                            }
                        }
                        detail.message = "success";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return detail;
        }

        public user_List GetUserById(int id ,int mode = 0)
        {
            DataSet ds = null;
            user_List detail = new user_List();
            try
            {
                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@mode", mode);
                    param[2] = new SqlParameter("@userid", id);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcGetUsers", param);

                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                user_Model obj = new user_Model();
                                if (!string.IsNullOrEmpty(dr["UserId"].ToString()))
                                {
                                    obj.userId = Convert.ToInt32(dr["UserId"].ToString());
                                }
                                else
                                {
                                    obj.userId = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["UserName"].ToString()))
                                {
                                    obj.username = dr["UserName"].ToString();
                                }
                                else
                                {
                                    obj.username = "";
                                }
                                if (mode != 0)
                                {
                                    if (!string.IsNullOrEmpty(dr["email"].ToString()))
                                    {
                                        obj.email = dr["email"].ToString();
                                    }
                                    else
                                    {
                                        obj.email = "";
                                    }
                                    if (!string.IsNullOrEmpty(dr["phNo"].ToString()))
                                    {
                                        obj.phNo = Convert.ToUInt64(dr["phNo"].ToString());
                                    }
                                    else
                                    {
                                        obj.phNo = 0;
                                    }
                                    if (!string.IsNullOrEmpty(dr["whatsappPhNo"].ToString()))
                                    {
                                        obj.whatsappPhNo = Convert.ToUInt64(dr["whatsappPhNo"].ToString());
                                    }
                                    else
                                    {
                                        obj.whatsappPhNo = 0;
                                    }
                                    if (!string.IsNullOrEmpty(dr["roleid"].ToString()))
                                    {
                                        obj.userType = Convert.ToInt32(dr["roleid"]);
                                    }
                                    else
                                    {
                                        obj.userType = 0;
                                    }
                                    if (!string.IsNullOrEmpty(dr["userType"].ToString()))
                                    {
                                        obj.role = dr["userType"].ToString();
                                    }
                                    else
                                    {
                                        obj.role = "";
                                    }
                                    if (!string.IsNullOrEmpty(dr["skypeId"].ToString()))
                                    {
                                        obj.skypeId = dr["skypeId"].ToString();
                                    }
                                    else
                                    {
                                        obj.skypeId = "";
                                    }
                                    if (!string.IsNullOrEmpty(dr["status"].ToString()))
                                    {
                                        obj.status = dr["status"].ToString();
                                    }
                                    else
                                    {
                                        obj.status = "";
                                    }
                                }

                                detail.list.Add(obj);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return detail;
        }

        public user_Model ValidateUsers(Login_Model model)
        {
            DataSet ds = null;
            user_Model obj = new user_Model();
            try
            {
                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    ds = new DataSet();
                    /*#CC04  added start*/
                    string strpass = encryptpass(model.Password);
                    //string strpassword = DecodeFrom64(model.Password);
                    /*#CC04  added end*/                   
                    SqlParameter[] param = new SqlParameter[3];                    
                    param[0] = new SqlParameter("@username", model.UserName);
                    param[1] = new SqlParameter("@password", strpass);
                    param[2] = new SqlParameter("@outparam", SqlDbType.Int);
                    param[2].Direction = ParameterDirection.Output;
                    //param[2] = new SqlParameter("@outparam", SqlDbType.Int).Direction = ParameterDirection.Output;
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcValidateUsers", param);

                    int result = Convert.ToInt32(param[2].Value);
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            if (dt.Rows.Count>0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {

                                    if (!string.IsNullOrEmpty(dr["UserId"].ToString()))
                                    {
                                        obj.userId = Convert.ToInt32(dr["UserId"].ToString());
                                    }
                                    else
                                    {
                                        obj.userId = 0;
                                    }
                                    if (!string.IsNullOrEmpty(dr["isActive"].ToString()))
                                    {
                                        obj.isActive = Convert.ToInt32(dr["isActive"].ToString());
                                    }
                                    else
                                    {
                                        obj.isActive = 0;
                                    }
                                    if (!string.IsNullOrEmpty(dr["UserName"].ToString()))
                                    {
                                        obj.username = dr["UserName"].ToString();
                                    }
                                    else
                                    {
                                        obj.username = "";
                                    }
                                    if (!string.IsNullOrEmpty(dr["userType"].ToString()))
                                    {
                                        obj.userType = Convert.ToInt32(dr["userType"]);
                                    }
                                    else
                                    {
                                        obj.userType = 0;
                                    }


                                }

                            }
                            else
                            {
                                obj = null;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public string DecodeFrom64(string Password)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(Password);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        private static IEnumerable<RoleModel> ConvertToRoleModel(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new RoleModel
                {
                    roleId = Convert.ToInt32(row["RoleId"]),
                    role = row["role"].ToString()
                    
                };
            }

        }

        public string AddUser(FormCollection model)
        {
            string status;
            try
            {
                DataTable records = new DataTable();
                records.Columns.Add("username");
                records.Columns.Add("email");
                records.Columns.Add("phNo");
                records.Columns.Add("whatsappPhNo");
                records.Columns.Add("userType");
                records.Columns.Add("skypeId");
                records.Columns.Add("createdDate");
                records.Columns["phNo"].DataType = typeof(Int64);
                records.Columns["userType"].DataType = typeof(Int32);
                records.Columns["whatsappPhNo"].DataType = typeof(Int64);
                records.Columns["createdDate"].DataType = typeof(DateTime);

                DataRow dr = records.NewRow();
                dr["username"] =model["username"] ;
                dr["email"] = model["email"];
                dr["phNo"] = 0;
                dr["skypeId"] = "";
                dr["whatsappPhNo"] = 0;
                dr["userType"] = model["roleid"];
                dr["createdDate"] =model["createdDate"];
                string strpass = encryptpass("abc");
                records.Rows.Add(dr);


                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@records" ,records);
                    param[1] = new SqlParameter("@responseCode", model["ResponseCode"]);
                    param[2] = new SqlParameter("@defaultPassword", strpass);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prccreateUser", param);
                    status = "success";
                }

            }
            catch (Exception ex)
            {
                status = "failed";
            }
            return status;
        }
        public string DeleteUserService(DeleteModel record)
        {
            string status;
            try
            {


                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@recordId", record.recordId);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcdeleteuserrecords", param);
                    status = "success";
                }

            }
            catch (Exception ex)
            {
                status = "failed";
            }
            return status;
        }

        public string UpdateUserStatus(string responseCode)
        {
            string status;
            try
            {


                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@responseCode", responseCode);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcupdateuserstatus", param);
                    status = "success";
                }

            }
            catch (Exception ex)
            {
                status = "failed";
            }
            return status;
        }

        public string ActivateUserService(DeleteModel record)
        {
            string status;
            try
            {


                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@recordId", record.recordId);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcactivateuserrecords", param);
                    status = "success";
                }

            }
            catch (Exception ex)
            {
                status = "failed";
            }
            return status;
        }


        public RecordsView_List GetRecords(user_Model userData = null ,string filter="" ,string datefilter="" ,int mode=0 ,int operationId =0)
        {
            DataSet ds = null;
            RecordsView_List records = new RecordsView_List();
            if (string.IsNullOrEmpty(datefilter))
            {
                datefilter = DateTime.UtcNow.ToString();
            }
            try
            {
                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@role", userData.userType);
                    param[1] = new SqlParameter("@userid", userData.userId); 
                    param[2] = new SqlParameter("@filter", filter);
                    param[3] = new SqlParameter("@mode", mode);
                    param[4] = new SqlParameter("@datefilter", Convert.ToDateTime(datefilter));

                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcGetRecords" ,param);
                    if (operationId==1)
                    {
                        utils.ExcelHelper.ExporttoExcel(ds.Tables[0]);
                    }
                    records.filter = filter;
                    records.datefilter = datefilter;
                    /*#CC02  added start*/
                    if (ds.Tables.Count > 0)
                    /*#CC02  added end*/
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                RecordsView_Model obj = new RecordsView_Model();
                                if (!string.IsNullOrEmpty(dr["agentname"].ToString()))
                                {
                                    obj.agentname = dr["agentname"].ToString();
                                }
                                else
                                {
                                    obj.agentname = "";
                                }
                                if (!string.IsNullOrEmpty(dr["clientname"].ToString()))
                                {
                                    obj.clientname = dr["clientname"].ToString();
                                }
                                else
                                {
                                    obj.clientname = "";
                                }
                                if (!string.IsNullOrEmpty(dr["email"].ToString()))
                                {
                                    obj.email = dr["email"].ToString();
                                }
                                else
                                {
                                    obj.email = "";
                                }
                                if (!string.IsNullOrEmpty(dr["skypeid"].ToString()))
                                {
                                    obj.skypeid = dr["skypeid"].ToString();
                                }
                                else
                                {
                                    obj.skypeid = "";
                                }
                                if (!string.IsNullOrEmpty(dr["whatsappPhNo"].ToString()))
                                {
                                    obj.whatsappPhNo = Convert.ToUInt64(dr["whatsappPhNo"]);
                                }
                                else
                                {
                                    obj.whatsappPhNo = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["phNo"].ToString()))
                                {
                                    obj.phNo = Convert.ToUInt64(dr["phNo"]);
                                }
                                else
                                {
                                    obj.phNo = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["leadid"].ToString()))
                                {
                                    obj.leadId = Convert.ToInt32(dr["leadid"]);
                                }
                                else
                                {
                                    obj.leadId = 0;
                                }

                                if (!string.IsNullOrEmpty(dr["orderNo"].ToString()))
                                {
                                    obj.orderNo = Convert.ToUInt64(dr["orderNo"]);
                                }
                                else
                                {
                                    obj.orderNo = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["createdDate"].ToString()))
                                {
                                    obj.createdDate = Convert.ToDateTime( dr["createdDate"]).ToString("dd MMMM yyyy");
                                   
                                }
                                else
                                {
                                    obj.createdDate = "";
                                }
                               
                                if (!string.IsNullOrEmpty(dr["remarks"].ToString()))
                                {
                                    obj.remarks = dr["remarks"].ToString();
                                }
                                else
                                {
                                    obj.remarks = "";
                                }
                                records.list.Add(obj);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return records;
        }

        public RecordsView_Model GetRecordsById(int recordId)
        {
            DataSet ds = null;
            RecordsView_Model obj = new RecordsView_Model();
            try
            {
                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@Recordid", recordId);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcGetRecords" ,param);

                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!string.IsNullOrEmpty(dr["agentname"].ToString()))
                                {
                                    obj.agentname = dr["agentname"].ToString();
                                }
                                else
                                {
                                    obj.agentname = "";
                                }
                                if (!string.IsNullOrEmpty(dr["clientname"].ToString()))
                                {
                                    obj.clientname = dr["clientname"].ToString();
                                }
                                else
                                {
                                    obj.clientname = "";
                                }
                                if (!string.IsNullOrEmpty(dr["email"].ToString()))
                                {
                                    obj.email = dr["email"].ToString();
                                }
                                else
                                {
                                    obj.email = "";
                                }
                                if (!string.IsNullOrEmpty(dr["skypeid"].ToString()))
                                {
                                    obj.skypeid = dr["skypeid"].ToString();
                                }
                                else
                                {
                                    obj.skypeid = "";
                                }
                                if (!string.IsNullOrEmpty(dr["whatsappPhNo"].ToString()))
                                {
                                    obj.whatsappPhNo = Convert.ToUInt64(dr["whatsappPhNo"]);
                                }
                                else
                                {
                                    obj.whatsappPhNo = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["phNo"].ToString()))
                                {
                                    obj.phNo = Convert.ToUInt64(dr["phNo"]);
                                }
                                else
                                {
                                    obj.phNo = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["leadid"].ToString()))
                                {
                                    obj.leadId = Convert.ToInt32(dr["leadid"]);
                                }
                                else
                                {
                                    obj.leadId = 0;
                                }

                                if (!string.IsNullOrEmpty(dr["orderNo"].ToString()))
                                {
                                    obj.orderNo = Convert.ToUInt64(dr["orderNo"]);
                                }
                                else
                                {
                                    obj.orderNo = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["orderamount"].ToString()))
                                {
                                    obj.orderamount = Convert.ToUInt64(dr["orderamount"]);
                                }
                                else
                                {
                                    obj.orderamount = 0;
                                }
                                if (!string.IsNullOrEmpty(dr["createdDate"].ToString()))
                                {
                                    obj.createdDate = Convert.ToDateTime(dr["createdDate"]).ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    obj.createdDate = "";
                                }

                                if (!string.IsNullOrEmpty(dr["remarks"].ToString()))
                                {
                                    obj.remarks = dr["remarks"].ToString();
                                }
                                else
                                {
                                    obj.remarks = "";
                                }
                                
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public string AddRecord(FormCollection model , user_Model userData)
        {
            string status;
            try
            {
                DataTable records = new DataTable();
                records.Columns.Add("userid");
                records.Columns.Add("clientname");
                records.Columns.Add("email");
                records.Columns.Add("phNo");
                records.Columns.Add("whatsappPhNo");
                records.Columns.Add("orderNo");
                records.Columns.Add("skypeId");
                records.Columns.Add("remarks");
                records.Columns.Add("createdDate");
                records.Columns.Add("orderAmount");
                records.Columns["orderNo"].DataType = typeof(Int32);
                records.Columns["orderAmount"].DataType = typeof(double);
                records.Columns["userid"].DataType = typeof(Int32);
                records.Columns["createdDate"].DataType = typeof(DateTime);
                records.Columns["phNo"].DataType = typeof(Int64);
                records.Columns["whatsappPhNo"].DataType = typeof(Int64);
               

                DataRow dr = records.NewRow();
                dr["userid"] = Convert.ToInt32(userData.userId);
                dr["clientname"] = model["clientname"];
                dr["email"] = model["email"];
                dr["phNo"] = Convert.ToInt64(model["phNo"]);
                dr["whatsappPhNo"] = Convert.ToInt64(model["whatsappPhNo"]);
                DateTime date = DateTime.UtcNow;
                int orderBase = Convert.ToInt32(date.ToString("yyMMdd"));
                dr["orderNo"] =orderBase;
                dr["skypeId"] = model["skypeId"];
                dr["remarks"] = model["remarks"];
                dr["orderAmount"] = model["orderAmount"];
                dr["createdDate"] = Convert.ToDateTime(model["createdDate"]);

                records.Rows.Add(dr);

                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@records", records);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prccreateRecords", param);
                    status = "success";
                }

            }
            catch (Exception ex)
            {
                status = "failed";
            }
            return status;
        }

        public string DeleteRecord(DeleteModel record)
        {
            string status;
            try
            {
                

                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@recordId", record.recordId);
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcdeleterecords", param);
                    status = "deleted";
                }

            }
            catch (Exception ex)
            {
                status = "failed";
            }
            return status;
        }

        public string UpdateRecord(FormCollection model, user_Model userData)
        {
            string status;
            try
            {
                //DataTable records = new DataTable();
                //records.Columns.Add("userid");
                //records.Columns.Add("orderNo");
                //records.Columns.Add("remarks");
                //records.Columns.Add("createdDate");
                //records.Columns["orderNo"].DataType = typeof(Int32);
                //records.Columns["userid"].DataType = typeof(Int32);
                //records.Columns["createdDate"].DataType = typeof(DateTime);

                //DataRow dr = records.NewRow();
                //dr["userid"] = model["userid"];
                //dr["orderNo"] = model["orderNo"];
                //dr["remarks"] = model["remarks"];
                //dr["createdDate"] = model["createdDate"];

                //records.Rows.Add(dr);
                DataTable records = new DataTable();
                records.Columns.Add("userid");
                records.Columns.Add("clientname");
                records.Columns.Add("email");
                records.Columns.Add("phNo");
                records.Columns.Add("whatsappPhNo");
                records.Columns.Add("orderamount");
                records.Columns.Add("skypeId");
                records.Columns.Add("remarks");
                records.Columns.Add("createdDate");
                records.Columns["orderamount"].DataType = typeof(Int32);
                records.Columns["userid"].DataType = typeof(Int32);
                records.Columns["createdDate"].DataType = typeof(DateTime);
                records.Columns["phNo"].DataType = typeof(Int64);
                records.Columns["whatsappPhNo"].DataType = typeof(Int64);

                DataRow dr = records.NewRow();
                dr["userid"] = Convert.ToInt32(userData.userId);
                dr["clientname"] = model["clientname"];
                dr["email"] = model["email"];
                dr["phNo"] = Convert.ToInt64(model["phNo"]);
                dr["whatsappPhNo"] = Convert.ToInt64(model["whatsappPhNo"]);
                dr["orderamount"] = Convert.ToInt32(model["orderamount"]);
                dr["skypeId"] = model["skypeId"];
                dr["remarks"] = model["remarks"];
                dr["createdDate"] = Convert.ToDateTime(model["createdDate"]);

                records.Rows.Add(dr);


                using (SqlConnection db = ConnectionHelper.getConnection())
                {
                    db.Open();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@records", records);
                    param[1] = new SqlParameter("@orderId",Convert.ToInt32(model["leadid"]) );
                    
                    ds = SqlHelper.ExecuteDataset(db, CommandType.StoredProcedure, "prcEditRecords", param);
                    status = "Editsuccess";
                }

            }
            catch (Exception ex)
            {
                status = "failed";
            }
            return status;
        }
        /*#CC01  added start*/
        public userUpdate_Response UpdateUserDetails(FormCollection userDetail)
        {
            userUpdate_Response response = new userUpdate_Response();
            try
            {
                using (SqlConnection db_con=ConnectionHelper.getConnection())
                {
                    string strpass = encryptpass(userDetail["password"]);
                    SqlParameter[] param = new SqlParameter[8];
                    param[0] = new SqlParameter("@userId", userDetail["userId"]);
                    param[1] = new SqlParameter("@phNo", userDetail["phNo"]);
                    param[2] = new SqlParameter("@username", userDetail["username"]);
                    param[3] = new SqlParameter("@whatsappPhNo", userDetail["whatsappPhNo"]);
                    param[4] = new SqlParameter("@skypeId", userDetail["skypeId"]);
                    param[5] = new SqlParameter("@password", strpass);
                    param[6] = new SqlParameter("@email", userDetail["email"]);
                    param[7] = new SqlParameter("@userType", userDetail["userType"]);
                    DataSet ds = SqlHelper.ExecuteDataset(db_con ,CommandType.StoredProcedure , "prcUpdateUserDetail", param);
                    response.statusMessage = "editsuccess";
                }
            }
            catch (Exception ex)
            {
                response.statusMessage = ex.Message;
            }
            return response;
        }
        /*#CC04  added start*/
        public string encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }
        /*#CC04  added start*/
    }
}