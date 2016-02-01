using Keskus.BLL.Security;
using Keskus.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Keskus.BLL.ContactType
{
    /// <remarks>
    /// Class contains services for ContactType business object
    /// </remarks>
    public class ContactTypeService
    {
        //application logic events are logged into log.txt file
        private Logger _log;

        // user action events are logged into userlog.txt file
        private Logger _userlog;

        public ContactTypeService()
        {
            _log = new Logger("ContactTypeService()");
            _log.Trace("Init");
            _userlog = new Logger(true, "ContactType updated");
        }

        /// <remarks>
        /// gets all ContactType entries from DB
        /// </remarks>
        /// <returns>list of contactTypes</returns>
        public List<ContactTypeBO> getAllFromTable()
        {
            _log.Trace("in getAllFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<ContactTypeBO> tableContent = db.ContactTypes
                    .ToList()
                    .Select(x => new ContactTypeBO(x))
                    .ToList();
                return tableContent;
            }
        }


        /// <remarks>
        /// adds new ContactType entry to DB
        /// </remarks>
        /// 
        public void addNew(string name, bool compulsory)
        {
            _log.Trace("in addNew()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.ContactType contactType = new DAL.ContactType
                {
                    Name = name,
                    Compulsory = compulsory
                };
                db.ContactTypes.Add(contactType);
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("ContactType added: {0}, {1}", contactType.Name, contactType.Compulsory));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("addNew() - An error occurred: '{0}'", ex));
                }
            }
        }

        /// <remarks>
        /// gets ContactType name by ID from DB
        /// </remarks>
        /// <returns>name of contactType as string</returns>
        public string getNameById (int id)
        {
            _log.Trace("in getNameById()");
            using (var db = new Keskus_baasEntities())
            {
                DAL.ContactType details = db.ContactTypes
                    .Where(x => x.ContactTypeID == id)
                    .FirstOrDefault();
                if (details != null)
                {
                    return details.Name;
                }
                return "";
            }
        }


        /// <remarks>
        /// clears ContactTypes Table. 
        /// Used for development of app, to ensure uniform data in DB on all dev stations
        /// </remarks>
        public void emptyTable()
        {
            _log.Trace("in emptyTable()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                foreach (var row in db.ContactTypes)
                    db.ContactTypes.Remove(row);
                db.SaveChanges();
            }
        }

    }
}
