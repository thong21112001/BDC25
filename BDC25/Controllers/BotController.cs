using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDC25.Controllers
{
    public class BotController : Controller
    {
        BDC25DbContext db = new BDC25DbContext();
        // GET: Bot
        [HttpPost]
        public ActionResult Messages(string userMessage)
        {
            // Xử lý nội dung tin nhắn và tạo phản hồi
            var responseMessage = ProcessMessage(userMessage);

            // Trả về phản hồi dưới dạng chuỗi
            return Content(responseMessage);
        }

        private string ProcessMessage(string message)
        {
            if (message.ToLower().Contains("xin chào") || message.ToLower().Contains("hi") || message.ToLower().Contains("hello"))
            {
                return "Xin chào! Tôi là chatbot từ BDC25. Rất vui được gặp bạn!";
            }
            if (message.ToLower().Contains("bạn có rảnh không"))
            {
                return "Tôi hiện đang rất là rảnh nhé >.<";
            }
            if (message.ToLower().Contains("ủng hộ") || message.ToLower().Contains("tôi muốn ủng hộ cho nhóm"))
            {
                return "Bạn bấm vào thành bar và chọn mục ủng hộ nhé. Xin cảm ơn!";
            }
            if (message.ToLower().Contains("kinh nghiệm") || message.ToLower().Contains("tôi muốn biết kinh nghiệm chăm sóc chó mèo"))
            {
                return "Bạn bấm vào thành bar và chọn mục kiến thức hoặc chia sẻ hoặc cộng đồng nhé. Xin cảm ơn!";
            }
            if (message.ToLower().Contains("giải trí") || message.ToLower().Contains("tôi có thể giải trí không"))
            {
                return "Bạn bấm vào thành bar và chọn mục game nhé. Xin cảm ơn!";
            }
            if (message.ToLower().Contains("video") || message.ToLower().Contains("video hướng dẫn"))
            {
                return "Bạn bấm vào thành bar và chọn mục cẩm nang video nhé. Xin cảm ơn!";
            }
            if (message.ToLower().Contains("cộng đồng") || message.ToLower().Contains("cho tôi biết về cộng đồng BDC25"))
            {
                return "BDC thành lập từ 04/2023 và đang trong quá trình phát triển hơn nữa trong tương lai";
            }
            if (message.ToLower().Contains("tạm biệt") || message.ToLower().Contains("bye"))
            {
                return "Cảm ơn vì đã dành thời gian nhắn tin với bot BDC25.";
            }
            else
            {
                return "Xin lỗi, tôi đang trong quá trình phát triển và không thể tiếp nhận những câu hỏi khó >3<";
            }
        }
    }
}