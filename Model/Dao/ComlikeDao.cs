using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ComlikeDao
    {
        public long? Getlikecounts(int id) // to count like  
        {
            using (var db = new BDC25DbContext())
            {
                var count = (from x in db.COMMUNITies where (x.IDCommunity == id && x.DanhGia != null) select x.DanhGia).FirstOrDefault();
                return count;
            }
        }

        // to get all users who have done like and dislike of the society  
        public List<COMLIKE> GetallUser(int id)
        {
            using (var db = new BDC25DbContext())
            {
                var count = (from x in db.COMLIKEs where x.IDCommunity == id select x).ToList();
                return count;
            }
        }
    }
}
