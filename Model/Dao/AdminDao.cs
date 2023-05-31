using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using static System.Collections.Specialized.BitVector32;

namespace Model.Dao
{
    public class AdminDao
    {
        BDC25DbContext db = null;

        public AdminDao()
        {
            db = new BDC25DbContext();
        }
        public ADMINWEB GetByID(string userN)
        {
            //C2
            return db.ADMINWEBs.SingleOrDefault(x => x.TaiKhoan == userN);
        }

        public int Login(string userName, string passW)
        {
            var result = db.ADMINWEBs.SingleOrDefault(x => x.TaiKhoan == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.MatKhau == passW)
                {
                    return 1;   //Kiểm tra trong CSDL nếu password đúng trả về 1
                }
                else
                {
                    return -1;   //Kiểm tra trong CSDL nếu password sai trả về -1
                }
            }
        }
    }
}
