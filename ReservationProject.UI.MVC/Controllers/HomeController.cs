using System.Web.Mvc;
using ReservationProject.UI.MVC.Models;
using System.Net.Mail;
using System.Net;
using System;

namespace ReservationProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string message = $"You have received an email from {cvm.Name} with a subject of {cvm.Subject}. Please" +
                $"respond to {cvm.Email} with your response to the following message: <br/> {cvm.Message} ";

            MailMessage mm = new MailMessage("admin@scottkwalters.com", "scottkwalters@outlook.com", cvm.Subject, message);

            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            mm.ReplyToList.Add(cvm.Email);

            SmtpClient client = new SmtpClient("mail.scottkwalters.com");
            client.Credentials = new NetworkCredential("admin@scottkwalters.com", "P@ssw0rd");

            try
            {
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage =
                    $"We're sorry your message could not be sent at this time." +
                    $"Please try again later or contact us by phone. <br/>Error Message: <br/>{ex.StackTrace}";
                return View(cvm);
            }
            return View("EmailConfirmation", cvm);


        }
    }
}
