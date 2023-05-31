using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model.EF;
using PagedList;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;


namespace Model.Dao
{
    //CHịu sự quản lý của admin dùng để khóa tài khoản
    public class UserDao
    {
        BDC25DbContext db = null;
        public UserDao()
        {
            db = new BDC25DbContext();
        }

        //Lấy toàn bộ danh sách trong DB
        public IEnumerable<USERWEB> ListALLCATE(string searchString, int page, int pageSize)
        {
            IQueryable<USERWEB> model = db.USERWEBs;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TaiKhoan.Contains(searchString) || x.Email.Contains(searchString) || x.DienThoai.Contains(searchString));
            }
            return model.OrderBy(x => x.IDUser).ToPagedList(page, pageSize);
        }

        //Dùng cho admin
        public USERWEB ViewDetail(int id)
        {
            IQueryable<USERWEB> listUser = db.USERWEBs;
            var list = listUser.Where(x => x.IDUser == id).FirstOrDefault();
            return list;
        }

        //Dùng cho admin
        public bool Delete(int id)
        {
            try
            {
                var user = db.USERWEBs.Find(id);
                db.USERWEBs.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        //Dùng cho admin
        //Dùng js để kích hoạt 
        public bool ChangeStatus(int id)
        {
            var user = db.USERWEBs.Find(id);
            user.AnHien = !user.AnHien;
            user.NgayKhoa = DateTime.Now;
            user.SoLanKhoa = user.SoLanKhoa + 1;
            db.SaveChanges();
            return user.AnHien;
        }

        //Dùng cho user
        public bool Update(USERWEB entity)
        {
            try
            {
                var idU = db.USERWEBs.Find(entity.IDUser);

                idU.HoTen = entity.HoTen;
                idU.DiaChi = entity.DiaChi;
                idU.NgaySua = DateTime.Now;
                idU.DienThoai = entity.DienThoai;
                idU.Email = entity.Email;
                idU.Image = entity.Image;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool UpdateNotImage(USERWEB entity)
        {
            try
            {
                var idU = db.USERWEBs.Find(entity.IDUser);

                idU.HoTen = entity.HoTen;
                idU.DiaChi = entity.DiaChi;
                idU.NgaySua = DateTime.Now;
                idU.DienThoai = entity.DienThoai;
                idU.Email = entity.Email;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        //Check tai khoan va email
        public bool CheckEmail(string email)
        {
            var ema = db.USERWEBs.FirstOrDefault(x => x.Email == email);
            if (ema == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckTK(string username)
        {
            var tk = db.USERWEBs.FirstOrDefault(x => x.TaiKhoan == username);
            if (tk == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Dùng cho user
        public long Register(USERWEB entity)
        {
            db.USERWEBs.Add(entity);
            db.SaveChanges();
            return entity.IDUser;
        }
        //Dùng cho user
        public int Login(string userName, string passW)
        {
            var result = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.AnHien == false)
                {
                    return -1;  //Kiểm tra trong CSDL nếu status = 0 thì bị khóa và trả về -1
                }
                else
                {
                    if (result.MatKhau == passW)
                    {
                        return 1;   //Kiểm tra trong CSDL nếu password đúng trả về 1
                    }
                    else
                    {
                        return 2;   //Kiểm tra trong CSDL nếu password sai trả về 2
                    }
                }
            }
        }

        //Dùng cho user
        public USERWEB GetByID(string userN)
        {
            //C2
            return db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userN);
        }

        public bool CheckPass(int IDUSer,string PassW)
        {
            var result = db.USERWEBs.SingleOrDefault(x => x.IDUser == IDUSer);
            if (result != null)
            {
                if (result.MatKhau == PassW)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        //Kiểm tra email
        public bool IsEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }
        //Kiểm tra mật khẩu
        public bool IsPass(string passwd)
        {
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();

            if (passwd.Length < 8 || passwd.Length > 14 && passwd.Contains(" ") && !passwd.Any(char.IsLower))
                return false;
            foreach (char ch in specialChArray)
            {
                if (passwd.Contains(ch))
                    return true;
            }
            return true;
        }

        //Kiểm tra số điện thoại
        public bool Isphone(string phone)
        {
            if (phone.Length != 10)
            {
                return false;
            }
            if (phone[0] != '0')
            {
                return false;
            }
            for (int i = 1; i < phone.Length; i++)
            {
                if (!char.IsDigit(phone[i]))
                {
                    return false;
                }
            }
            return true;
        }

        //Khóa tài khoản
        public bool GetStatus(string userN)
        {
            var result = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userN);
            if (result.AnHien == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ ngày khóa đến hiện tại xong trả về số ngày bằng ngày hiện tại - ngày khóa = totalDay
        /// </summary>
        public int CountLock(string userN)
        {
            var result = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userN);
            int count = result.SoLanKhoa;
            return count;
        }

        /// <summary>
        /// Lấy ngày mở khóa của tài khoản từ ngày khóa + thêm 3 ngày ra đc ngày mở khóa
        /// </summary>
        public DateTime GetDayUnLock(string userN)
        {
            CultureInfo cul = new CultureInfo("es-ES");
            var result = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userN);
            DateTime day = (DateTime)result.NgayKhoa;
            string dayLock = day.ToString("dd/MM/yyyy");
            DateTime getDayLock = DateTime.Parse(dayLock, cul);
            DateTime getUnLock = getDayLock.AddDays(3);
            return getUnLock;
        }

        /// <summary>
        /// Lấy dữ liệu từ ngày khóa đến hiện tại xong trả về số ngày bằng ngày hiện tại - ngày khóa = totalDay
        /// </summary>
        public int GetTotalDayLock(string userN)
        {
            var result = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userN);
            CultureInfo cul = new CultureInfo("es-ES");
            DateTime day = (DateTime)result.NgayKhoa;
            string dayLock = day.ToString("dd/MM/yyyy");
            DateTime getDayLock = DateTime.Parse(dayLock, cul);
            DateTime end = DateTime.Today;
            TimeSpan timesp = end.Subtract(getDayLock);  //Khảng thời gian từ ngày lock đến hiện tại
            int totalDay = timesp.Days;
            return totalDay;
        }

        /// <summary>
        /// Thực hiện mở khóa tài khoản
        /// </summary>
        public bool UnLock(string userName)
        {
            var result = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userName);
            if (result != null)
            {
                CultureInfo cul = new CultureInfo("es-ES");
                DateTime day = (DateTime)result.NgayKhoa;
                string dayLock = day.ToString("dd/MM/yyyy");
                DateTime getDayLock = DateTime.Parse(dayLock, cul);
                DateTime end = DateTime.Today;
                TimeSpan timesp = end.Subtract(getDayLock);  //Khảng thời gian từ ngày lock đến hiện tại
                int totalDay = timesp.Days;

                //Nếu tổng ngày khóa -> ngày hiện tại phải lớn hơn 3 ngày thì ms tiến hành mở khóa
                if (totalDay > 3)
                {
                    result.AnHien = true;
                    result.NgayMoKhoa = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string RandomPassword(int number)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var password = string.Empty;
            var random = new Random();
            for (var i = 0; i < number; i++)
            {
                var x = random.Next(1, chars.Length);
                if (!password.Contains(chars.GetValue(x).ToString()))
                {
                    password += chars.GetValue(x);
                }
                else
                {
                    i--;
                }
            }

            return password;
        }

        public bool UpdatePass(string userN,string pass)
        {
            try
            {
                var result = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == userN);
                if (result == null)
                {
                    return false;
                }
                else
                {
                    result.MatKhau = pass;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
