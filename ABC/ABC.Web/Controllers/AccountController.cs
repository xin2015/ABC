using ABC.BLL.CryptoTransverters;
using ABC.BLL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABC.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            string salt = new Random().Next(1000000).ToString().PadLeft(6, '0');
            AsymmetricEncryption ae = new AsymmetricEncryption();
            Session["salt"] = salt;
            Session["AE"] = ae;
            ViewBag.Salt = salt;
            ViewBag.PublicRSAKey = ae.PublicKeyBase64String;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userName, string password)
        {
            string salt = Session["salt"] as string;
            AsymmetricEncryption ae = Session["AE"] as AsymmetricEncryption;
            password = ae.Decrypt(password);
            return View();
        }
    }
}