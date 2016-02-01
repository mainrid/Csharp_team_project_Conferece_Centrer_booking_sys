using System;
using System.Collections.Generic;
using System.Linq;
using Keskus.DAL;
using System.Security.Cryptography;
using Keskus.BLL.Security;

namespace Keskus.BLL.Administrator
{


    /// <remarks>
    /// Class contains services for Administrator business object
    /// </remarks>
    public class AdministratorService
    {
        //application logic events are logged into log.txt file
        private Logger _log;

        // user action events are logged into userlog.txt file
        private Logger _userlog;

        //constructor
        public AdministratorService()
        {
            _log = new Logger("AdministratorService()");
            _log.Trace("Init");
            _userlog = new Logger( true, "Administrators data is changed");
        }

        /// <remarks>
        /// get all Administrators from Administrators table
        /// </remarks>
        /// <returns> list of admins</returns>
        public List<AdministratorBO> getAllFromTable()
        {
            _log.Trace("in getAllFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<AdministratorBO> admins = db.Administrators
                    .ToList()
                    .Select(x => new AdministratorBO(x))
                    .ToList();
                return admins;
            }
        }


        /// <remarks>
        /// Add new Administrator entry
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="changeAllowed"></param>
        public void addNew (string name, string userName, string password, bool changeAllowed)
        {
            
            _log.Trace("in addNew()");
            //encrypt password to hash
            string hPassword = EncryptPassword.computeHash(password, new MD5CryptoServiceProvider());

            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Administrator newAdmin = new DAL.Administrator
                {
                    Name = name,
                    CanChange = changeAllowed,
                    Password = hPassword,
                    Username = userName
                };
                db.Administrators.Add(newAdmin);
                try
                {
                    db.SaveChanges();
                    //userlog registeres new db entry added event 
                    _userlog.Trace(string.Format("New admin added: {0}, {1}, {2}", newAdmin.Name, newAdmin.Username, newAdmin.CanChange));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("addNew() - An error occurred: '{0}'", ex));
                }
            }
        }

        /// <remarks>
        /// searches administrator entry in DB table by ID
        /// </remarks>
        /// <param name="adminId"></param>
        /// <returns>Administrator object or null, if object not found</returns>
        public AdministratorBO findAdminByID (int adminId)
        {
            _log.Trace("in findAdminByID()");
            using (var db = new Keskus_baasEntities())
            {
                var admin = db.Administrators
                    .Where(x => x.AdminID == adminId)
                    .FirstOrDefault();
                if (admin != null)
                {
                    return new AdministratorBO(admin);
                }
                _log.Trace("in findAdminByID() - admin == null");
                return null;
            }
        }


        /// <remarks>
        /// searches admin entry in Administartors table by admin name
        /// </remarks>
        /// <param name="name"></param>
        /// <returns>Administrator object or null, if object not found</returns>
        public AdministratorBO getAdminByName(string name)
        {
            _log.Trace("in getAdminByName()");

            using (var db = new Keskus_baasEntities())
            {
                var admin = db.Administrators
                    .Where(x => x.Name == name)
                    .FirstOrDefault();
                if (admin != null)
                {
                    return new AdministratorBO(admin);
                }
                _log.Trace("in getAdminByName() - admin == null");
                return null;
            }
        }


        /// <remarks>
        /// clears Administrators Table. 
        /// Used for development of app, to ensure uniform data in DB on all dev stations
        /// </remarks>
        public void emptyTable()
        {
            _log.Trace("in emptyTable()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                foreach (var row in db.Administrators)
                    db.Administrators.Remove(row);
                db.SaveChanges();
            }
        }

    }
}
