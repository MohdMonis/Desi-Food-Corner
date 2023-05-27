using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModel;
using DAL;
using InnoAssignment1.ViewModel;
using Microsoft.Xrm.Sdk;
using System.Web.Mvc;

namespace InnoAssignment1.Controllers
{
    public class OrderController : Controller
    {
        CRMDAL dal = new CRMDAL();
        // GET: ViewOrders
        public ActionResult ViewOrdersForStoreKeeper()
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here

                ViewBag.OrdersList = dal.GetShopkeeperOrders();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        #region ActionsForCustomer
        public ActionResult ViewOrdersForCustomer()
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here
                ViewBag.User = userDm;
                ViewBag.OrdersList = dal.GetCustomerOrdersByCustomerGuid(userDm.Id);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CreateOrder()
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here
                ViewBag.Items = dal.GetItemMasterList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CancelOrder(Guid Id)
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here

                OrderDM order = dal.UpdateOrderStatusByOrderIdAndValue(Id, 180720012);
                return RedirectToAction("ViewOrdersForCustomer", "Order");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CreateOrderForCustomer()
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here
                OrderDM order = new OrderDM();
                order.Name = string.Format("Order For {0}", string.Concat(userDm.FirstName, userDm.LastName));
                order.CustomerId = userDm.Id;
                order.ContactNo = Request["mobilenumber"];
                order.Address = Request["address"];
                order.PaymentModeValue = Convert.ToInt32(Request["paymentMethod"]);
                order.Id = dal.CreateNewOrderForCustomer(order);
                Session["order"] = order;
                return RedirectToAction("ViewOrder", "Order", new { Id = order.Id });
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult AddItemsInCustomerOrder()
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                OrderDM orderDm = (OrderDM)Session["order"];
                ItemMasterDM item = null;
                List<ItemMasterDM> lim = new List<ItemMasterDM>();
                lim = dal.GetItemMasterList();
                OrderItemDM orderItemDM = new OrderItemDM();
                orderItemDM.Name = string.Format("Item For {0}", string.Concat(userDm.FirstName, userDm.LastName));
                orderItemDM.OrderId = orderDm.Id;
                orderItemDM.ItemMasterId = Guid.Parse(Request["item"]);
                orderItemDM.Quantity = Convert.ToInt32(Request["quantity"]);
                foreach (var dalt in lim)
                {
                    if (dalt.Id == orderItemDM.ItemMasterId)
                    {
                        item = new ItemMasterDM();
                        item = dalt;
                    }
                }
                orderItemDM.Price = item.Price;
                orderItemDM.Id = dal.AddItemInOrderByOrderItemModel(orderItemDM);
                return RedirectToAction("ViewOrder", "Order", new { Id = orderDm.Id });
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult DeleteOrderItem(Guid Id)
        {
            if (Session["userDm"] != null)
            {
                dal.DeleteRecordByNameAndId("mon_orderitem", Id);
                return RedirectToAction("ViewOrder", "Order", new { Id = ((OrderDM)Session["order"]).Id });
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult PlaceOrder()
        {
            if (Session["userDm"] != null)
            {
                dal.UpdateOrderStatusByOrderIdAndValue(((OrderDM)Session["order"]).Id, 180720001);
                return RedirectToAction("ViewOrdersForCustomer", "Order");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        #endregion

        #region Common For All Customer Types
        public ActionResult UpdateOrderStatus(Guid Id)
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here
                OrderDM order = new OrderDM();
                order = dal.GetOrderById(Id);
                if(order.StatusReasonValue == 180720001)
                {
                    order = dal.UpdateOrderStatusByOrderIdAndValue(Id, 180720002);
                }else if (order.StatusReasonValue == 180720002)
                {
                    order = dal.UpdateOrderStatusByOrderIdAndValue(Id, 180720003);
                }else if (order.StatusReasonValue == 180720003)
                {
                    order = dal.UpdateOrderStatusByOrderIdAndValue(Id, 180720004);
                }
                else if (order.StatusReasonValue == 180720004)
                {
                    order = dal.UpdateOrderStatusByOrderIdAndValue(Id, 180720011);
                }
                dal.UpdateOrderStatusValue(userDm, Id);
                if (userDm.ContactTypeValue == 180720001)
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
                return RedirectToAction("Login", "Login");
            }
        }

        #endregion
        public ActionResult ViewOrder(Guid Id)
        {
            if (Session["userDm"] != null)
            {
                ViewOrder_VM order_VM = new ViewOrder_VM();
                order_VM.Order = dal.GetOrderById(Id);
                Session["order"] = order_VM.Order;
                order_VM.OrderItems = dal.GetOrderItemsByOrderId(Id);
                order_VM.ItemMaster = dal.GetItemMasterList();
                return View(order_VM);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        #region ActionFroDeliveryBoy
        public ActionResult ViewOrdersForDeliveryBoy()
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here 

                ViewBag.OrdersList = dal.GetDeliveryBoyOrderByDeliveryBoyGuid(userDm.Id);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult UpdateStatusToUnDelivered(Guid Id)
        {
            if (Session["userDm"] != null)
            {
                ContactDM userDm = (ContactDM)Session["userDm"];
                //Add your code here
                OrderDM order = new OrderDM();
                order = dal.UpdateOrderStatusByOrderIdAndValue(Id, 180720013);
                dal.UpdateOrderStatusValue(userDm, Id);
                return RedirectToAction("ViewOrdersForDeliveryBoy", "Order");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        #endregion
    }
}