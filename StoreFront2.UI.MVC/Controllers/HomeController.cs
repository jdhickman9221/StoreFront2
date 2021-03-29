using StoreFrontJordan.MVC.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace StoreFrontJordan.MVC.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Store()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
        public ActionResult Checkout()
        {


            return View();
        }

        public ActionResult Cart()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            #region Email Notes
            //Usings TYPICALLY required for sending mail.
            //System.Net - Access to provide credentials for the SMTP server (username password port)
            //System.Net.Mail - Access to Mail Message class and methods associated.
            //Models folder (where our viewModel lives)

            //information required to send the email:
            //SMTP Server Name - mail.domain.ext
            //Email UserName (created at smarterASP)
            //Email Password (if you keep the default with SmarterASP, it is the same as your acct login pw)
            //port numbers - only need to be defined if your attempt to send mail fails (because of access)
            //Default (if not declared) is 25, alternate (must define) is 8889.
            #endregion

            //before we attempt to do anything with the instance of our model class
            //we want make sure it passes model validation
            if (!ModelState.IsValid)
            //ModelState.IsValid == false)
            {
                //if the object doesnt pass validation, send back the form WITH the object
                //that will repopulate the form AND show the validation errors.
                return View(cvm);
            }

            //this only executes if the form/object passes model validation
            //build the email message (this is what YOU will see when a user sends an email
            //from YOUR website.
            string message = $"You have received an email from {cvm.Name} with a subject of " +
                $"{(cvm.Subject == null ? "Email From SITENAME" : cvm.Subject)}. Please respond to " +
                $"{cvm.EmailAddress} with your response to the following message:<br /><br />" +
                $"{cvm.Message}";

            //build the MailMesage Object
            //FQ ctor to use: FROM, To, Subject, Message
            MailMessage mm = new MailMessage("admin@JordanHickman.com", "jdhickman816@outlook.com", cvm.Subject,//update
                message);

            //MailMessage properties
            mm.IsBodyHtml = true;

            //priority is not required, if you are going to leave at normal or low, just omit.
            mm.Priority = MailPriority.High;

            //update the reply to go to the end user instead of your admin Account at the website
            mm.ReplyToList.Add(cvm.EmailAddress);


            //Send the email
            SmtpClient client = new SmtpClient("mail.JordanHickman.com");//update
            //port 25 is the default. If your email does not send within 4 hours
            //consider changing the port value to be 8889;
            //client.Port = 8889;

            //client credentials
            client.Credentials = new NetworkCredential("admin@JordanHickman.com", "HmtU7APYMus_");//update

            //attempt to send the email
            //it is possible that the mail server is unavailable, in that case we do want to use
            //exception handling to care for mail message sending failure
            try
            {
                //attempt to send email
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"We're sorry your request could not be completed at this time. " +
                    $"Please try again later. Error Message:<br /><br />{ex.StackTrace}.";
                //stack trace is more for admin (recreate the problem) and should be encapsulated
                //in the an if(user.isinrole("admin") in the view.
                return View(cvm);
            }

            //if all goes well, return a view that displays a confirmation message - optionally,
            //you could provide the email content that they provided.
            return View("EmailConfirmation", cvm);
            //return RedirectToAction("Index");
        }

    }
}