using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BDC25.Models
{
    public class CountPostFlowTag   // Dung de dem so luong bai viet trong the
    {
        public int IDTag { get; set; }
        public string Tagname { get; set; }
        public int Count { get; set; }
    }
}