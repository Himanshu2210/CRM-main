using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class request_Model
    {
        public int orderId { get; set; }
        public int orderNo { get; set; }
		public string remarks { get; set; }
		public DateTime createdDate { get; set; }
    }

    public class RequestList
    {
        public RequestList()
        {
            this.requestList = new List<request_Model>();
            this.message = "";
        }
        public List<request_Model> requestList { get; set; }
        public string message { get; set; }
    }

    public class RequestEdit_Model
    {
        public RequestEdit_Model() 
        {
            this.userList = new UserDropDown_List();
            this.record = new request_Model();
        }
        public request_Model record { get; set; }
        public string userName { get; set; }
        public UserDropDown_List userList { get; set; }
    }

    public class DeleteModel
    {
        public string recordId { get; set; }
    }
}