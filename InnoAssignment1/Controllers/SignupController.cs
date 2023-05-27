using DataModel;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InnoAssignment1.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Signup()
        {
            return View();
        }
        public ActionResult SignupForm()
        {
            CRMDAL crmdl = new CRMDAL();
            try
            {
                ContactDM con = new ContactDM();
                con.FirstName = Request["firstName"];
                con.LastName = Request["lastName"];
                con.GenderValue = Convert.ToInt32(Request["gender"]);
                con.MobilePhone = Request["mobile"];
                con.BirthDay = Convert.ToDateTime(Request["dob"]);
                con.Email = Request["email"];
                con.Password = Request["password"];
                con.ContactTypeValue = 180720000;

                Guid guid = crmdl.SetContactByEmailAndPassword(con);
                if (guid != Guid.Empty)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    return Content("account not created please try again");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}