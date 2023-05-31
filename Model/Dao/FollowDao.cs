using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class FollowDao
    {
        BDC25DbContext db = null;
        public FollowDao()
        {
            db = new BDC25DbContext();
        }

        public int GetallUser(int id)
        {
            int countFL = db.USERFOLLOWs.Where(x => x.IDUser == id && x.IsFollow == true).Count();
            return countFL;
        }
    }
}
