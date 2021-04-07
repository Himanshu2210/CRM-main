using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
	public class user_Model
	{
		public string password { get; set; }
		public int userId { get; set; }
		public int isActive { get; set; }
		public int loginAttempt { get; set; }

		public string username { get; set; }
		public string email { get; set; }
		public ulong phNo { get; set; }
		public ulong whatsappPhNo { get; set; }
		public int userType { get; set; }
		public string role { get; set; }
		public string skypeId { get; set; }
		public string status { get; set; }
        public DateTime createdDate { get; set; }

    }

	public class Login_Model
	{
		[Key]
		public int id { get; set; }
		public string email { get; set; }

		public string isActive { get; set; }

		public int loginAttempt { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email/User Name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required]
		[Display(Name = "Organization")]
		public string Organization { get; set; }
		//public string __RequestVerificationToken { get; set; }
		//public string returnUrl { get; set; }
		//public string controller { get; set; }
	}

	public class user_List
	{
		public user_List()
		{
			this.list = new List<user_Model>();
		}
		public List<user_Model> list { get; set; }
		public string filter { get; set; }
		public string dateFilter { get; set; }
        public string message { get; set; }
    }

	public class UserDropDown_Model
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
	}

	public class UserDropDown_List
	{
		
		public UserDropDown_List()
		{
			this.list = new List<UserDropDown_Model>();
			
		}
		public List<UserDropDown_Model> list { get; set; }
		

	}
	public class UserDetail
	{
		public UserDetail()
		{
			this.roleList = new userTypeList();
		}

		public user_Model user { get; set; }
		public request_Model request { get; set; }
		public userTypeList roleList { get; set; }
	}

	public class RecordsView_Model
	{

		public string clientname { get; set; }
		public string agentname { get; set; }

		public string email { get; set; }
		public string skypeid { get; set; }

		public ulong phNo { get; set; }
		public ulong whatsappPhNo { get; set; }
		public string userRole { get; set; }
		public ulong orderNo { get; set; }
		public ulong orderamount { get; set; }	
		public string remarks { get; set; }
		public string createdDate { get; set; }
		public DateTime lastUpdatedDate { get; set; }
		public int leadId { get; set; }

	}

	public class RecordsView_List
	{
		public RecordsView_List()
		{
			this.list = new List<RecordsView_Model>();
		}
		public List<RecordsView_Model> list { get; set; }
		public string datefilter { get; set; }
		public string filter { get; set; }

	}

	public class RecordsEdit_List
	{
		public RecordsEdit_List()
		{
			this.userList = new UserDropDown_List();
			this.list = new RecordsView_List();
		}
		public RecordsView_List list { get; set; }
		public UserDropDown_List userList { get; set; }
	}

	public class RoleModel
	{
	
		public int roleId { get; set; }
		public string role { get; set; }
	}

	public class userTypeList
	{
		public userTypeList()
		{
			this.list = new List<RoleModel>();

		}
		public List<RoleModel> list { get; set; }
	}

	public class PerformanceDetail
	{
		public PerformanceDetail()
		{
			this.userDetail = new user_Model();
			this.requsetList = new RequestList();
		}
		public user_Model userDetail { get; set; }
		public RequestList requsetList { get; set; }
		public string message { get; set; }
	}

	public class UserEditModel
	{
		public UserEditModel()
		{
			this.list = new List<RoleModel>();
			this.userDetail = new user_Model();

		}
		public List<RoleModel> list { get; set; }
		public user_Model userDetail { get; set; }
		public string message { get; set; }

	}

	public class userUpdate_Response
    {
        public int userId { get; set; }
        public string statusMessage { get; set; }
    }
}

