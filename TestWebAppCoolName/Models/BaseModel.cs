using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAppCoolName.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public bool Approved { get; set; }
        public string OwnerId { get; set; }
        public int Position { get; set; }
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
    }
}