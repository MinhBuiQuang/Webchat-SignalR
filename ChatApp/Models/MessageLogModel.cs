using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class MessageLogModel
    {
        public int MessageID { get; set; }
        public Nullable<int> YeuCauID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public UserModel User { get; set; }
    }
}