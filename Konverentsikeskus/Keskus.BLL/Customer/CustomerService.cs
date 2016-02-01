using Keskus.BLL.Security;
using Keskus.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Keskus.BLL.Customer
{


    /// <remarks>
    /// Class contains services for Customer business object
    /// </remarks>
    public class CustomerService
    {
        //application logic events are logged into log.txt file
        private Logger _log;

        // user action events are logged into userlog.txt file
        private Logger _userlog;

        public CustomerService()
        {
            _log = new Logger("CustomerService()");
            _log.Trace("Init");
            _userlog = new Logger(true, "Customers data is changed");
        }


        /// <remarks>
        /// gets all entries from Customers table in DB 
        /// </remarks>
        /// <returns>list of customers</returns>
        public List<CustomerBO> getAllFromTable()
        {
            _log.Trace("in getAllFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<CustomerBO> tableContent = db.Customers
                    .ToList()
                    .Select(x => new CustomerBO(x))
                    .ToList();
                return tableContent;
            }
        }

        /// <remarks>
        /// adds new Customer ntry to DB
        /// </remarks>
        /// <param name="companyName"></param>
        /// <param name="contactPerson"></param>
        /// <returns> customer ID as integer </returns>
        public int addNew(string companyName, string contactPerson)
        {
            _log.Trace("in addNew()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Customer customer = new DAL.Customer
                {
                    CompanyName = companyName,
                    ContactPerson = contactPerson
                };
                db.Customers.Add(customer);
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("New customer added. ID {0}: {1}, {2}", customer.CustomertID, customer.CompanyName, customer.ContactPerson));
                    return customer.CustomertID;
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("addNew() - An error occurred: '{0}'", ex));
                }
            }
            return 0;
        }

        /// <remarks>
        /// gets customer from Customers table in DB by ID
        /// </remarks>
        /// <param name="customerId"></param>
        /// <returns> customer object</returns>
        public CustomerBO getCustomerById(int customerId)
        {
            _log.Trace("in getCustomerById()");
            using (var db = new Keskus_baasEntities())
            {
                return new CustomerBO(db.Customers
                    .Where(x => x.CustomertID == customerId)
                    .FirstOrDefault());
            }
        }

        /// <remarks>
        /// gets first customer from Customers table in DB by customer company name
        /// </remarks>
        /// <param name="customerId"></param>
        /// <returns> customer object</returns>
        public CustomerBO getCustomerByName(string customerName)
        {
            _log.Trace("in getCustomerByName()");
            using (var db = new Keskus_baasEntities())
            {
                return new CustomerBO(db.Customers
                    .Where(x => x.CompanyName == customerName)
                    .FirstOrDefault());
            }
        }

        /// <remarks>
        /// gets all customers from Customers table in DB by matching part of customer company name
        /// </remarks>
        /// <param name="customerId"></param>
        /// <returns> list of customers</returns>
        public List<CustomerBO> searchCustomerByCompany(string searchCompany)
        {
            _log.Trace("in searchCustomerByCompany()");
            string searchCompToLower = searchCompany.ToLower();
            List<CustomerBO> foundCustomers = new List<CustomerBO>();

            using (var db = new Keskus_baasEntities())
            {
                foreach (var customer in db.Customers)
                {
                    if (customer.CompanyName.ToLower().Contains(searchCompToLower))
                    {
                        foundCustomers.Add(new CustomerBO(customer));
                    }
                }
            }
            return foundCustomers;

        }


        /// <remarks>
        /// updates the existing customer entry, with data provided by user
        /// </remarks>
        /// <param name="customerID"></param>
        /// <param name="companyName"></param>
        /// <param name="contactPerson"></param>
        public void UpdateById(int customerID, string companyName, string contactPerson)
        {
            _log.Trace("in UpdateById()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Customer toUpdate = (from x in db.Customers
                                         where x.CustomertID == customerID
                                         select x).FirstOrDefault();

                
                _userlog.Trace(string.Format("Customer updated, from: {0}, {1}", toUpdate.CompanyName, toUpdate.ContactPerson));
                toUpdate.CustomertID = customerID;
                toUpdate.CompanyName = companyName;
                toUpdate.ContactPerson = contactPerson;

                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Customer updated, to: {0}, {1}", toUpdate.CompanyName, toUpdate.ContactPerson));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("UpdateById() - An error occurred: '{0}'", ex));
                }
            }
        }


        /// <remarks>
        /// clears Customer Table. 
        /// Used for development of app, to ensure uniform data in DB on all dev stations
        /// </remarks>
        public void emptyTable()
        {
            _log.Trace("in emptyTable()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                foreach (var row in db.Customers)
                    db.Customers.Remove(row);
                db.SaveChanges();
            }
        }
    }
}
