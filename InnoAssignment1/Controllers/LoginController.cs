using DataModel;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace InnoAssignment1.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit()
        {
            CRMDAL crmdl = new CRMDAL();
            try
            {
                string email = Request["email"];
                string password = Request["password"];

                ContactDM contactDM = crmdl.GetContactByEmailAndPassword(email, password);
                if (contactDM != null)
                {
                    Session["userDM"] = contactDM;
                    if (contactDM.ContactTypeText == "Customer")
                    {
                        return RedirectToAction("ViewOrdersForCustomer", "Order");
                    }
                    else if (contactDM.ContactTypeText == "Store Keeper")
                    {
                        return RedirectToAction("ViewOrdersForStoreKeeper", "Order");
                    }
                    else
                    {
                        return RedirectToAction("ViewOrdersForDeliveryBoy", "Order");
                    }
                }
                else
                {
                    return Content("Inncorrect Email or password");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ForgotPass()
        {
            CRMDAL crmdl = new CRMDAL();
            try
            {
                string email = Request["email"];
                ContactDM contactDM = crmdl.GetContactByEmail(email);
                if (contactDM != null)
                {
                    // new password generator code starts
                    const string valid = "@#$%/?abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    StringBuilder res = new StringBuilder();
                    Random rnd = new Random();
                    int i = 0;
                    while (i < 10) { res.Append(valid[rnd.Next(valid.Length)]); i++; }
                    string newPassword = res.ToString();
                    // new password generator code ends
                    contactDM.Password = newPassword;
                    contactDM.ForgotPassword = true;
                    Guid guid = crmdl.ChangePassword(contactDM);
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    return Content("Inncorrect Email or password");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult PassChange()
        {
            return View();
        }

        public ActionResult ChangePassowrd()
        {
            CRMDAL crmdl = new CRMDAL();
            if (Session["userDm"] != null)
            {
                ContactDM contactDM = (ContactDM)Session["userDm"];

                if (Request["password"]!= Request["pass"])
                {
                    crmdl.ChangePasswordByIdAndPassword(contactDM.Id, Request["pass"]);
                    if (contactDM.ContactTypeValue == 180720000)
                    {
                        return RedirectToAction("ViewOrdersForCustomer", "Order");
                    }
                    else if (contactDM.ContactTypeValue == 180720001)
                    {
                        return RedirectToAction("ViewOrdersForStoreKeeper", "Order");
                    }
                    else
                    {
                        return RedirectToAction("ViewOrdersForDeliveryBoy", "Order");
                    }
                }
                else
                {
                    if(Request["password"] != Request["pass"])
                    {
                        return Content("Old password and New Password should not be the same.");
                    }
                    else
                    {
                        return Content("Your have to login first in order to change password!");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Logout()
        {
            Session["userDm"] = null;
            return RedirectToAction("Login", "Login");
        }
    }
}