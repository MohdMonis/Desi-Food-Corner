using DataModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CRMDAL
    {
        public IOrganizationService _orgService;
        public CRMDAL()
        {
            string connectionString = ConfigurationManager.AppSettings["CRMConnectionString"];
            CrmServiceClient _crmConnection;

            _crmConnection = new CrmServiceClient(connectionString);

            _orgService = (IOrganizationService)_crmConnection.OrganizationWebProxyClient != null ? (IOrganizationService)_crmConnection.OrganizationWebProxyClient : (IOrganizationService)_crmConnection.OrganizationServiceProxy;
        }

        #region Signup
        public Guid SetContactByEmailAndPassword(ContactDM con)
        {
            Guid gid = Guid.Empty;
            try
            {
                Entity ecn = new Entity("contact");
                if (!string.IsNullOrWhiteSpace(con.FirstName))
                    ecn["firstname"] = con.FirstName;
                if (!string.IsNullOrWhiteSpace(con.LastName))
                    ecn["lastname"] = con.LastName;
                if (!string.IsNullOrWhiteSpace(con.MobilePhone))
                    ecn["mobilephone"] = con.MobilePhone;
                if (con.GenderValue > 0)
                    ecn["gendercode"] = new OptionSetValue(con.GenderValue);
                if (con.BirthDay != DateTime.MinValue)
                    ecn["birthdate"] = con.BirthDay;
                if (!string.IsNullOrWhiteSpace(con.Email))
                    ecn["emailaddress1"] = con.Email;
                if (!string.IsNullOrWhiteSpace(con.Password))
                    ecn["mon_password"] = con.Password;
                if (con.ContactTypeValue > 0)
                    ecn["mon_contacttype"] = new OptionSetValue(con.ContactTypeValue);
                gid = _orgService.Create(ecn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return gid;
        }
        #endregion

        #region Login
        public ContactDM GetContactByEmailAndPassword(string email, string password)
        {
            ContactDM contactDM = null;
            try
            {
                string qry = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='contact'>
                                <attribute name='firstname' />
                                <attribute name='lastname' />
                                <attribute name='contactid' />
                                <attribute name='emailaddress1' />
                                <attribute name='mon_contacttype' />//
                                <attribute name='mobilephone' />
                                <attribute name='address1_line1' />
                                <attribute name='address1_city' />
                                <attribute name='address1_stateorprovince' />
                                <order attribute='fullname' descending='false' />
                                <filter type='and'>
                                <condition attribute='emailaddress1' operator='eq' value='{0}' />
                                <condition attribute='mon_password' operator='eq' value='{1}' />
                                </filter>
                            </entity>
                            </fetch>";
                qry = string.Format(qry, email, password);
                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qry));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    Entity entity = entityCol.Entities[0];
                    contactDM = new ContactDM();
                    contactDM.Id = entity.Id;
                    if (entity.Contains("emailaddress1"))
                        contactDM.Email = (string)entity["emailaddress1"];
                    if (entity.Contains("firstname"))
                        contactDM.FirstName = (string)entity["firstname"];
                    if (entity.Contains("lastname"))
                        contactDM.LastName = (string)entity["lastname"];
                    if (entity.Contains("mon_contacttype"))
                    {
                        contactDM.ContactTypeValue = ((OptionSetValue)entity["mon_contacttype"]).Value;
                        contactDM.ContactTypeText = entity.FormattedValues["mon_contacttype"];
                    }
                    if (entity.Contains("mobilephone"))
                        contactDM.MobilePhone = (string)entity["mobilephone"];
                    if (entity.Contains("address1_line1"))
                        contactDM.AddressLine = (string)entity["address1_line1"];
                    if (entity.Contains("address1_city"))
                        contactDM.AddressCity = (string)entity["address1_city"];
                    if (entity.Contains("address1_stateorprovince"))
                        contactDM.AddressState = (string)entity["address1_stateorprovince"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return contactDM;
        }

        public ContactDM GetContactByEmail(string email)
        {
            ContactDM contactDM = null;
            try
            {
                string qry = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='contact'>
                                <attribute name='firstname' />
                                <attribute name='lastname' />
                                <attribute name='contactid' />
                                <attribute name='emailaddress1' />
                                <attribute name='mon_contacttype' />
                                <order attribute='fullname' descending='false' />
                                <filter type='and'>
                                <condition attribute='emailaddress1' operator='eq' value='{0}' />
                                </filter>
                            </entity>
                            </fetch>";
                qry = string.Format(qry, email);
                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qry));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    Entity entity = entityCol.Entities[0];
                    contactDM = new ContactDM();
                    contactDM.Id = entity.Id;
                    if (entity.Contains("emailaddress1"))
                        contactDM.Email = (string)entity["emailaddress1"];
                    if (entity.Contains("firstname"))
                        contactDM.FirstName = (string)entity["firstname"];
                    if (entity.Contains("lastname"))
                        contactDM.LastName = (string)entity["lastname"];
                    if (entity.Contains("mon_contacttype"))
                    {
                        contactDM.ContactTypeValue = ((OptionSetValue)entity["mon_contacttype"]).Value;
                        contactDM.ContactTypeText = entity.FormattedValues["mon_contacttype"];
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Opps something went wrong.." + ex); }
            return contactDM;
        }

        public Guid ChangePassword(ContactDM con)
        {
            Guid guid = con.Id;
            try
            {
                Entity ecn = new Entity("contact");
                ecn.Id = guid;
                if (!string.IsNullOrWhiteSpace(con.Password))
                    ecn["mon_password"] = con.Password;
                ecn["mon_forgotpassword"] = con.ForgotPassword;
                _orgService.Update(ecn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return guid;
        }

        public void ChangePasswordByIdAndPassword(Guid Id, string pass)
        {
            Entity oen = new Entity("contact");
            oen.Id = Id;
            if (!string.IsNullOrWhiteSpace(pass))
                oen["mon_password"] = pass;
            _orgService.Update(oen);
        }

        #endregion

        #region Order
        public List<OrderDM> GetCustomerOrdersByCustomerGuid(Guid Id)
        {
            OrderDM orderDM = null;
            List<OrderDM> orderList = new List<OrderDM>();
            try
            {
                string qry = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                 <entity name='mon_order'>
                                    <attribute name='mon_orderid' />
                                    <attribute name='mon_name' />
                                    <attribute name='mon_orderno' />
                                    <attribute name='mon_customer' />
                                    <attribute name='mon_contactno' />
                                    <attribute name='mon_address' />
                                    <attribute name='mon_dispatchedby' />
                                    <attribute name='mon_deliveredby' />
                                    <attribute name='statuscode' />
                                    <attribute name='mon_totalamount' />
                                   <order attribute='mon_orderno' descending='false' />
                                   <filter type='and'>
                                            <condition attribute='mon_customer' operator='eq' value='{0}' />
                                   </filter>
                                 </entity>
                               </fetch>";
                qry = string.Format(qry, Id);
                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qry));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    foreach (var entity in entityCol.Entities)
                    {
                        orderDM = new OrderDM();
                        orderDM.Id = entity.Id;
                        if (entity.Contains("mon_customer"))
                        {
                            orderDM.CustomerId = ((EntityReference)entity["mon_customer"]).Id;
                            orderDM.CustomerName = ((EntityReference)entity["mon_customer"]).Name;
                        }
                        if (entity.Contains("mon_orderno"))
                            orderDM.OrderNo = (string)entity["mon_orderno"];
                        if (entity.Contains("mon_dispatchedby"))
                        {
                            orderDM.DispatchedById = ((EntityReference)entity["mon_dispatchedby"]).Id;
                            orderDM.DispatchedByName = ((EntityReference)entity["mon_dispatchedby"]).Name;
                        }
                        if (entity.Contains("mon_deliveredby"))
                        {
                            orderDM.DeliveredById = ((EntityReference)entity["mon_deliveredby"]).Id;
                            orderDM.DeliveredByName = ((EntityReference)entity["mon_deliveredby"]).Name;
                        }
                        if (entity.Contains("mon_name"))
                            orderDM.Name = (string)entity["mon_name"];
                        if (entity.Contains("mon_contactno"))
                            orderDM.ContactNo = (string)entity["mon_contactno"];
                        if (entity.Contains("mon_address"))
                            orderDM.Address = (string)entity["mon_address"];
                        if (entity.Contains("statuscode"))
                        {
                            orderDM.StatusReasonValue = ((OptionSetValue)entity["statuscode"]).Value;
                            orderDM.StatusReasonText = entity.FormattedValues["statuscode"];
                        }
                        orderList.Add(orderDM);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Opps something went wrong.." + ex); }
            return orderList;
        }

        public List<OrderDM> GetShopkeeperOrders()
        {
            OrderDM orderDM = null;
            List<OrderDM> orderList = new List<OrderDM>();
            try
            {
                string qry = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                 <entity name='mon_order'>
                                    <attribute name='mon_orderid' />
                                    <attribute name='mon_name' />
                                    <attribute name='mon_orderno' />
                                    <attribute name='mon_customer' />
                                    <attribute name='mon_contactno' />
                                    <attribute name='mon_address' />
                                    <attribute name='mon_dispatchedby' />
                                    <attribute name='mon_deliveredby' />
                                    <attribute name='statuscode' />
                                    <attribute name='mon_totalamount' />
                                   <order attribute='mon_name' descending='false' />
                                 </entity>
                               </fetch>";
                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qry));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    foreach (var entity in entityCol.Entities)
                    {
                        orderDM = new OrderDM();
                        orderDM.Id = entity.Id;
                        if (entity.Contains("mon_customer"))
                        {
                            orderDM.CustomerId = ((EntityReference)entity["mon_customer"]).Id;
                            orderDM.CustomerName = ((EntityReference)entity["mon_customer"]).Name;
                        }
                        if (entity.Contains("mon_orderno"))
                            orderDM.OrderNo = (string)entity["mon_orderno"];
                        if (entity.Contains("mon_dispatchedby"))
                        {
                            orderDM.DispatchedById = ((EntityReference)entity["mon_dispatchedby"]).Id;
                            orderDM.DispatchedByName = ((EntityReference)entity["mon_dispatchedby"]).Name;
                        }
                        if (entity.Contains("mon_deliveredby"))
                        {
                            orderDM.DeliveredById = ((EntityReference)entity["mon_deliveredby"]).Id;
                            orderDM.DeliveredByName = ((EntityReference)entity["mon_deliveredby"]).Name;
                        }
                        if (entity.Contains("mon_name"))
                            orderDM.Name = (string)entity["mon_name"];
                        if (entity.Contains("mon_contactno"))
                            orderDM.ContactNo = (string)entity["mon_contactno"];
                        if (entity.Contains("mon_address"))
                            orderDM.Address = (string)entity["mon_address"];
                        if (entity.Contains("statuscode"))
                        {
                            orderDM.StatusReasonValue = ((OptionSetValue)entity["statuscode"]).Value;
                            orderDM.StatusReasonText = entity.FormattedValues["statuscode"];
                        }
                        orderList.Add(orderDM);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Opps something went wrong.." + ex); }
            return orderList;
        }

        public List<OrderDM> GetDeliveryBoyOrderByDeliveryBoyGuid(Guid Id)
        {
            OrderDM orderDM = null;
            List<OrderDM> orderList = new List<OrderDM>();
            try
            {
                string qry = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                 <entity name='mon_order'>
                                   <attribute name='mon_orderid' />
                                   <attribute name='mon_name' />
                                   <attribute name='statuscode' />
                                   <attribute name='mon_orderno' />
                                   <attribute name='mon_dispatchedby' />
                                   <attribute name='mon_deliveredby' />
                                   <attribute name='mon_customer' />
                                   <attribute name='mon_contactno' />
                                   <attribute name='mon_address' />
                                   <attribute name='mon_totalamount' />
                                   <order attribute='mon_name' descending='false' />
                                   <filter type='and'>
                                     <filter type='or'>
                                       <condition attribute='mon_deliveredby' operator='eq' value='{0}' />
                                       <condition attribute='mon_deliveredby' operator='null' />
                                     </filter>
                                     <condition attribute='statuscode' operator='in'>
                                       <value>180720002</value>
                                       <value>180720013</value>
                                       <value>180720003</value>
                                       <value>180720011</value>
                                       <value>180720004</value>
                                     </condition>
                                   </filter>
                                 </entity>
                               </fetch>";
                qry = string.Format(qry, Id);
                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qry));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    foreach (var entity in entityCol.Entities)
                    {
                        orderDM = new OrderDM();
                        orderDM.Id = entity.Id;
                        if (entity.Contains("mon_customer"))
                        {
                            orderDM.CustomerId = ((EntityReference)entity["mon_customer"]).Id;
                            orderDM.CustomerName = ((EntityReference)entity["mon_customer"]).Name;
                        }
                        if (entity.Contains("mon_orderno"))
                            orderDM.OrderNo = (string)entity["mon_orderno"];
                        if (entity.Contains("mon_dispatchedby"))
                        {
                            orderDM.DispatchedById = ((EntityReference)entity["mon_dispatchedby"]).Id;
                            orderDM.DispatchedByName = ((EntityReference)entity["mon_dispatchedby"]).Name;
                        }
                        if (entity.Contains("mon_deliveredby"))
                        {
                            orderDM.DeliveredById = ((EntityReference)entity["mon_deliveredby"]).Id;
                            orderDM.DeliveredByName = ((EntityReference)entity["mon_deliveredby"]).Name;
                        }
                        if (entity.Contains("mon_name"))
                            orderDM.Name = (string)entity["mon_name"];
                        if (entity.Contains("mon_contactno"))
                            orderDM.ContactNo = (string)entity["mon_contactno"];
                        if (entity.Contains("mon_address"))
                            orderDM.Address = (string)entity["mon_address"];
                        if (entity.Contains("statuscode"))
                        {
                            orderDM.StatusReasonValue = ((OptionSetValue)entity["statuscode"]).Value;
                            orderDM.StatusReasonText = entity.FormattedValues["statuscode"];
                        }
                        orderList.Add(orderDM);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Opps something went wrong.." + ex); }
            return orderList;
        }

        public ContactDM GetCustomerDetailsByGuid(Guid GuId)
        {
            ContactDM contactDM = null;
            try
            {
                string qry = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                 <entity name='contact'>
                                   <attribute name='contactid' />
                                   <attribute name='mobilephone' />
                                   <attribute name='lastname' />
                                   <attribute name='firstname' />
                                   <attribute name='address1_line1' />
                                   <attribute name='address1_stateorprovince' />
                                   <attribute name='address1_city' />
                                   <order attribute='mobilephone' descending='false' />
                                   <filter type='and'>
                                     <condition attribute='contactid' operator='eq' value='{0}' />
                                     <condition attribute='mon_contacttype' operator='eq' value='180720000' />
                                   </filter>
                                 </entity>
                               </fetch>";

                qry = string.Format(qry, GuId);

                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qry));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    Entity entity = entityCol.Entities[0];
                    contactDM = new ContactDM();
                    contactDM.Id = entity.Id;

                    if (entity.Contains("firstname"))
                        contactDM.FirstName = (string)entity["firstname"];
                    if (entity.Contains("lastname"))
                        contactDM.LastName = (string)entity["lastname"];
                    if (entity.Contains("mobilephone"))
                        contactDM.MobilePhone = (string)entity["mobilephone"];
                    if (entity.Contains("address1_line1"))
                        contactDM.AddressLine = (string)entity["address1_line1"];
                    if (entity.Contains("address1_city"))
                        contactDM.AddressCity = (string)entity["address1_city"];
                    if (entity.Contains("address1_stateorprovince"))
                        contactDM.AddressState = (string)entity["address1_stateorprovince"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return contactDM;
        }

        public OrderDM UpdateOrderStatusByOrderIdAndValue(Guid Orderid, int changeto)
        {
            OrderDM order = new OrderDM();
            Entity oen = new Entity("mon_order");
            oen.Id = Orderid;
            if (changeto > 0)
                oen["statuscode"] = new OptionSetValue(changeto);
            _orgService.Update(oen);
            return order;
        }
        
        public void UpdateOrderStatusValue(ContactDM user, Guid Id)
        {
            OrderDM order = new OrderDM();
            order = GetOrderById(Id);
            Entity oen = new Entity("mon_order");
            oen.Id = Id;
            if (order.StatusReasonValue == 180720004)
            {
                order.DispatchedById = user.Id;
                order.DispatchedOn = DateTime.Now;
                if (Guid.Empty != order.DispatchedById)
                    oen["mon_dispatchedby"] = new EntityReference("contact", order.DispatchedById);
                if (order.DispatchedOn != DateTime.MinValue)
                    oen["mon_dispatchedon"] = order.DispatchedOn;
            }
            else if (order.StatusReasonValue == 180720011 || order.StatusReasonValue == 180720013 || order.StatusReasonValue == 180720003)
            {
                order.DeliveredById = user.Id;
                order.DeliveredOn = DateTime.Now;
                if (Guid.Empty != order.DeliveredById)
                    oen["mon_deliveredby"] = new EntityReference("contact", order.DeliveredById);
                if (order.DeliveredOn != DateTime.MinValue)
                    oen["mon_deliveredon"] = order.DeliveredOn;
            }
            _orgService.Update(oen);
        }
        
        public List<ItemMasterDM> GetItemMasterList()
        {
            List<ItemMasterDM> itemlist = new List<ItemMasterDM>();
            try
            {
                string qry = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                 <entity name='mon_itemmaster'>
                                   <attribute name='mon_itemmasterid' />
                                   <attribute name='mon_name' />
                                   <attribute name='mon_type' />
                                   <attribute name='mon_price' />
                                   <attribute name='mon_description' />
                                   <attribute name='mon_category' />
                                   <order attribute='mon_price' descending='false' />
                                 </entity>
                               </fetch>";

                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qry));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    foreach (var entity in entityCol.Entities)
                    {
                        ItemMasterDM itemDM = new ItemMasterDM();
                        itemDM.Id = entity.Id;

                        if (entity.Contains("mon_name"))
                            itemDM.Name = (string)entity["mon_name"];
                        if (entity.Contains("mon_description"))
                            itemDM.Description = (string)entity["mon_description"];
                        if (entity.Contains("mon_type"))
                        {
                            itemDM.TypeValue = ((OptionSetValue)entity["mon_type"]).Value;
                            itemDM.TypeText = entity.FormattedValues["mon_type"];
                        }
                        if (entity.Contains("mon_price"))
                            itemDM.Price = decimal.Parse((((Money)entity["mon_price"]).Value).ToString("0.00"));
                        if (entity.Contains("mon_category"))
                        {
                            itemDM.CategoryId = ((EntityReference)entity["mon_category"]).Id;
                            itemDM.CategoryName = ((EntityReference)entity["mon_category"]).Name;
                        }
                        itemlist.Add(itemDM);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return itemlist;
        }
        
        public Guid CreateNewOrderForCustomer(OrderDM order)
        {
            Guid gid = Guid.Empty;
            try
            {
                Entity ecn = new Entity("mon_order");
                if (!string.IsNullOrWhiteSpace(order.Name))
                    ecn["mon_name"] = order.Name;
                if (Guid.Empty != order.CustomerId)
                    ecn["mon_customer"] = new EntityReference("contact", order.CustomerId);
                if (!string.IsNullOrWhiteSpace(order.ContactNo))
                    ecn["mon_contactno"] = order.ContactNo;
                if (order.PaymentModeValue > 0)
                    ecn["mon_paymentmode"] = new OptionSetValue(order.PaymentModeValue);
                if (!string.IsNullOrWhiteSpace(order.Address))
                    ecn["mon_address"] = order.Address;
                gid = _orgService.Create(ecn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return gid;
        }
        
        public Guid AddItemInOrderByOrderItemModel(OrderItemDM orderItem)
        {
            Guid guid = Guid.Empty;
            try
            {
                Entity ecn = new Entity("mon_orderitem");
                if (!string.IsNullOrWhiteSpace(orderItem.Name))
                    ecn["mon_name"] = orderItem.Name;
                if (Guid.Empty != orderItem.OrderId)
                    ecn["mon_order"] = new EntityReference("mon_order", orderItem.OrderId);
                if (Guid.Empty != orderItem.ItemMasterId)
                    ecn["mon_itemmaster"] = new EntityReference("mon_order", orderItem.ItemMasterId);
                if (!string.IsNullOrWhiteSpace(Convert.ToString(orderItem.Quantity)))
                    ecn["mon_quantity"] = orderItem.Quantity;
                if (orderItem.Price > 0)
                    ecn["mon_price"] = new Money(orderItem.Price);
                guid = _orgService.Create(ecn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return guid;
        }
        
        public OrderDM GetOrderById(Guid Id)
        {
            OrderDM itemDM = null;
            try
            {
                Entity entity = _orgService.Retrieve("mon_order", Id, new ColumnSet(true));
                if (entity != null)
                {
                    itemDM = new OrderDM();
                    itemDM.Id = entity.Id;
                    if (entity.Contains("mon_name"))
                        itemDM.Name = (string)entity["mon_name"];
                    if (entity.Contains("mon_orderno"))
                        itemDM.OrderNo = (string)entity["mon_orderno"];
                    if (entity.Contains("mon_address"))
                        itemDM.Address = (string)entity["mon_address"];
                    if (entity.Contains("mon_contactno"))
                        itemDM.ContactNo = (string)entity["mon_contactno"];
                    if (entity.Contains("statuscode"))
                    {
                        itemDM.StatusReasonValue = ((OptionSetValue)entity["statuscode"]).Value;
                        itemDM.StatusReasonText = entity.FormattedValues["statuscode"];
                    }
                    if (entity.Contains("mon_paymentmode"))
                    {
                        itemDM.PaymentModeValue = ((OptionSetValue)entity["mon_paymentmode"]).Value;
                        itemDM.PaymentModeText = entity.FormattedValues["mon_paymentmode"];
                    }
                    if (entity.Contains("mon_amount"))
                        itemDM.Amount = decimal.Parse((((Money)entity["mon_amount"]).Value).ToString("0.00"));
                    if (entity.Contains("mon_totalamount"))
                        itemDM.TotalAmount = decimal.Parse((((Money)entity["mon_totalamount"]).Value).ToString("0.00"));
                    return itemDM;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return itemDM;
        }
        
        public List<OrderItemDM> GetOrderItemsByOrderId(Guid Id)
        {
            OrderItemDM itemDM = null;
            List<OrderItemDM> itemList = new List<OrderItemDM>();
            try
            {
                string qrey = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='mon_orderitem'>
                                    <attribute name='mon_orderitemid' />
                                    <attribute name='mon_name' />
                                    <attribute name='mon_quantity' />
                                    <attribute name='mon_price' />
                                    <attribute name='mon_order' />
                                    <attribute name='mon_itemmaster' />
                                    <attribute name='mon_amount' />
                                    <order attribute='mon_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='mon_order' operator='eq' value='{0}' />
                                    </filter>
                                  </entity>
                                </fetch>";
                qrey = string.Format(qrey, Id);
                EntityCollection entityCol = _orgService.RetrieveMultiple(new FetchExpression(qrey));
                if (entityCol != null && entityCol.Entities.Count > 0)
                {
                    foreach (var entity in entityCol.Entities)
                    {
                        itemDM = new OrderItemDM();
                        itemDM.Id = entity.Id;
                        if (entity.Contains("mon_name"))
                            itemDM.Name = (string)entity["mon_name"];
                        if (entity.Contains("mon_quantity"))
                            itemDM.Quantity = (int)entity["mon_quantity"];
                        if (entity.Contains("mon_itemmaster"))
                        {
                            itemDM.ItemMasterId = ((EntityReference)entity["mon_itemmaster"]).Id;
                            itemDM.ItemMasterName = ((EntityReference)entity["mon_itemmaster"]).Name;
                        }
                        if (entity.Contains("mon_amount"))
                            itemDM.Amount = decimal.Parse((((Money)entity["mon_amount"]).Value).ToString("0.00"));
                        if (entity.Contains("mon_price"))
                            itemDM.Price = decimal.Parse((((Money)entity["mon_price"]).Value).ToString("0.00"));
                        itemList.Add(itemDM);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Opps something went wrong.." + ex);
            }
            return itemList;
        }
        
        public void DeleteRecordByNameAndId(string name, Guid Id)
        {
            _orgService.Delete(name, Id);
        }
        #endregion
    }
}