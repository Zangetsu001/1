using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using EduMaster.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduMaster.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Login() => RedirectToAction("Index");
        public IActionResult Register() => RedirectToAction("Index");

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // --- Основные страницы ---
        public IActionResult Index() => View();
        public IActionResult AboutUs() => View();
        public IActionResult Services() => View();
        public IActionResult Contacts() => View();
        public IActionResult SiteInformation() => View();
        public IActionResult Privacy() => View();

        // ===========================================================
        //                     ОТПРАВКА ПИСЬМА (AJAX)
        // ===========================================================
        [HttpPost]
        public IActionResult SendMessage([FromBody] ContactMessage dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Message))
            {
                return Json(new { success = false, message = "Заполните все поля." });
            }

            try
            {
                // Твой Gmail
                string yourEmail = "rty842891@gmail.com";

                // Пароль приложения Google (не обычный пароль!)
                string yourPassword = "ufyt rwsl lgly hgbl";

                var from = new MailAddress(yourEmail, "EduMaster");
                var to = new MailAddress(yourEmail); // письмо придёт тебе

                string subject =
                    string.IsNullOrWhiteSpace(dto.Subject)
                    ? $"Сообщение от {dto.Name}"
                    : dto.Subject;

                string body =
                    $"Имя: {dto.Name}\n" +
                    $"Email: {dto.Email}\n" +
                    $"Тема: {dto.Subject}\n\n" +
                    $"Сообщение:\n{dto.Message}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(yourEmail, yourPassword)
                };

                using (var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                return Json(new { success = true, message = "Сообщение отправлено!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка отправки email");
                return Json(new { success = false, message = "Ошибка отправки письма." });
            }
        }

        // --- Ошибка ---
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
