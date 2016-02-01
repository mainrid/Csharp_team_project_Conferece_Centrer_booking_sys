using Keskus.BLL.Security;
using Keskus.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Keskus.BLL.Contact
{
    /// <remarks>
    /// Class contains services for Contact business object
    /// </remarks>
    public class ContactService
    {
        //application logic events are logged into log.txt file
        private Logger _log;

        // user action events are logged into userlog.txt file
        private Logger _userlog;

        public ContactService()
        {
            _log = new Logger("ContactService()");
            _log.Trace("Init");
            _userlog = new Logger(true, "Contacts data is changed");
        }

        /// <remarks>
        ///Gets all entries from Contacts table
        /// </remarks>
        /// <returns>list of contacts</returns>
        public List<ContactBO> getAllFromTable()
        {
            _log.Trace("in getAllFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<ContactBO> tableContent = db.Contacts
                    .ToList()
                    .Select(x => new ContactBO(x))
                    .ToList();
                return tableContent;
            }
        }

        /// <remarks>
        /// adds new entry to Contacts table
        /// </remarks>
        /// <param name="customerID"></param>
        /// <param name="contactTypeID"></param>
        /// <param name="value"></param>
        /// <param name="created"></param>
        public void addNew (int customerID, int contactTypeID, string value, DateTime created)
        {
            _log.Trace("in addNew()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Contact contact = new DAL.Contact
                {
                    CustomerID = customerID,
                    ContactTypeID = contactTypeID,
                    Value = value,
                    Created = created
                };
                db.Contacts.Add(contact);
               
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Contact was added on {0} for customer {1}: type {2} - value {3}", contact.Created, contact.CustomerID, contact.ContactTypeID, contact.Value ));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("addNew() - An error occurred: '{0}'", ex));
                }
            }
        }

        /// <remarks>
        /// gets all customer contacts from table by customer ID
        /// </remarks>
        /// <param name="customerId"></param>
        /// <returns>list of contacts</returns>
        public List<ContactBO> getAllFromTableByCustomerID(int customerId)
        {
            _log.Trace("in getAllFromTableByCustomerID()");
            using (var db = new Keskus_baasEntities())
            {
                List<ContactBO> tableContent = db.Contacts
                    .ToList()
                    .Select(x => new ContactBO(x))
                    .Where(x => x.CustomerID == customerId)
                    .ToList();
                return tableContent;
            }
        }

        /// <remarks>
        /// updates contact entry in DB with data provided by user
        /// </remarks>
        /// <param name="contactID"></param>
        /// <param name="customerID"></param>
        /// <param name="contactTypeID"></param>
        /// <param name="value"></param>
        /// <param name="created"></param>
        public void UpdateById(int contactID, int customerID, int contactTypeID, string value, DateTime created)
        {
            _log.Trace("in UpdateById()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Contact toUpdate = (from x in db.Contacts
                                    where x.ContactID == contactID
                                        select x).FirstOrDefault();
                

                _userlog.Trace(string.Format("Contact changed from: {0}, {1}, {2}", toUpdate.CustomerID, toUpdate.ContactTypeID, toUpdate.Value));

                toUpdate.CustomerID = customerID;
                toUpdate.ContactTypeID = contactTypeID;
                toUpdate.Value = value;
                toUpdate.Created = created;
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Contact changed to: {0}, {1}, {2}", toUpdate.CustomerID, toUpdate.ContactTypeID, toUpdate.Value));

                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("UpdateById() - An error occurred: '{0}'", ex));
                }
            }
        }



        /// <remarks>
        /// clears Contacts Table. 
        /// Used for development of app, to ensure uniform data in DB on all dev stations
        /// </remarks>
        public void emptyTable()
        {
            _log.Trace("in emptyTable()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                foreach (var row in db.Contacts)
                    db.Contacts.Remove(row);
                db.SaveChanges();
            }
        }


    }
}
