using System;
using System.Collections.Generic;
using System.Linq;
using Keskus.DAL;
using Keskus.BLL.Security;

namespace Keskus.BLL.Booking
{
    /// <remarks>
    /// Class contains services for Booking business object
    /// </remarks>
    public class BookingService
    {
        //application logic events are logged into log.txt file
        private Logger _log;

        // user action events are logged into userlog.txt file
        private Logger _userlog;


        public BookingService()
        {
            _log = new Logger("BookingService()");
            _log.Trace("Init");
            _userlog = new Logger(true, "Bookings data is changed");
        }

        /// <remarks>
        /// gets all existing Booking database entries
        /// </remarks>
        /// <returns> list of bookings</returns>
        public List<BookingBO> getAllFromTable()
        {
            _log.Trace("in getAllFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<BookingBO> tableContent = db.Bookings
                    .ToList()
                    .Select(x => new BookingBO(x))
                    .ToList();
                return tableContent;
            }
        }

        /// <remarks>
        /// gets all existing unconfirmed Booking database entries, that are not archived and will take plase today or later
        /// </remarks>
        /// <returns> list of bookings</returns>
        public List<BookingBO> getUnconfirmedFromTable()
        {
            _log.Trace("in getUnconfirmedFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<BookingBO> tableContent = db.Bookings
                    
                    .Where(x => x.Confirmed == null && x.Archived==null && DateTime.Compare(x.Date, DateTime.Today) >= 0  )
                    .ToList()
                    .Select(x => new BookingBO(x))
                    .ToList();

                return tableContent;
            }
        }

        /// <remarks>
        /// gets all existing confirmed Booking database entries, that are not archived and will take plase today or later
        /// </remarks>
        /// <returns> list of bookings</returns>
        public List<BookingBO> getConfirmedFromTable()
        {
            _log.Trace("in getConfirmedFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<BookingBO> tableContent = db.Bookings
                    .Where(x => x.Confirmed != null && x.Archived==null && DateTime.Compare(x.Date, DateTime.Today) >= 0)
                    .ToList()
                    .Select(x => new BookingBO(x))
                    .ToList();

                return tableContent;
            }
        }




        /// <remarks>
        /// adds new Booking entry to database
        /// </remarks>
        /// <param name="date"></param>
        /// <param name="roomID"></param>
        /// <param name="customerID"></param>
        /// <param name="participants"></param>
        /// <param name="created"></param>
        /// <param name="adminID"></param>
        /// <param name="addInfo"></param>
        public void addNew(DateTime date, int roomID, int customerID, int participants, DateTime created, int adminID, string addInfo )
        {
            _log.Trace("in addNew()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Booking booking = new DAL.Booking
                {
                    Date = date,
                    RoomID = roomID,
                    CustomerID = customerID,
                    Participants = participants,              
                    Created = created,
                    AdminID = adminID,
                    AdditionalInfo = addInfo,
                    //by default Archived and Confirmed values are null
                    Archived = null,
                    Confirmed = null

                };
                db.Bookings.Add(booking);
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Added new booking {0} by {1} on {2}: date {3}, room:{4}, customer:{5}, participants: {6}, additional: {7} ", booking.BookingID, booking.Administrator, booking.Created, booking.Date, booking.Room, booking.Customer, booking.Participants, booking.AdditionalInfo));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("addNew() - An error occurred: '{0}'", ex));
                }
            }
        }


        /// <remarks>
        /// updates existing booking entry data in DB by ID
        /// </remarks>
        /// <param name="bookingId"></param>
        /// <param name="date"></param>
        /// <param name="roomID"></param>
        /// <param name="customerID"></param>
        /// <param name="participants"></param>
        /// <param name="addInfo"></param>
        public void UpdateById(int bookingId, DateTime date, int roomID, int customerID, int participants, string addInfo)
        {
            _log.Trace("in UpdateById()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Booking toUpdate = (from x in db.Bookings
                                         where x.BookingID == bookingId
                                         select x).FirstOrDefault();

                _userlog.Trace(string.Format("Booking {0} of {1} updated, from: date {2}, room:{3},  participants: {4}, additional: {5} ", toUpdate.BookingID, toUpdate.CustomerID, toUpdate.Date, toUpdate.Room, toUpdate.Participants, toUpdate.AdditionalInfo));
                toUpdate.Date = date;
                toUpdate.RoomID = roomID;
                toUpdate.Participants = participants;
                toUpdate.CustomerID = customerID;
                toUpdate.AdditionalInfo = addInfo;

                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Booking {0} of {1} updated, to: date {2}, room:{3},  participants: {4}, additional: {5} ", toUpdate.BookingID, toUpdate.CustomerID, toUpdate.Date, toUpdate.Room, toUpdate.Participants, toUpdate.AdditionalInfo));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("UpdateById() - An error occurred: '{0}'", ex));
                }
            }
        }

        /// <remarks>
        /// Sets existing DB booking entry  property Confirmed to current DateTime value
        /// </remarks>
        /// <param name="booking"></param>
        public void confirmBooking(BookingBO booking)
        {
            _log.Trace("in confirmBooking()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Booking toConfirm = (from x in db.Bookings
                                         where x.BookingID == booking.BookingID
                                         select x).FirstOrDefault();

                toConfirm.Confirmed = DateTime.Now;
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Booking {0} is confirmed on {1} ", toConfirm.BookingID, toConfirm.Confirmed));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("confirmBooking() - An error occurred: '{0}'", ex));
                }
            }
        }


        /// <remarks>
        /// Sets existing DB booking entry property Archived to current DateTime value
        /// </remarks>
        /// <param name="booking"></param>
        public void archiveBooking(BookingBO booking)
        {
            _log.Trace("in archiveBooking()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Booking toArchive = (from x in db.Bookings
                                         where x.BookingID == booking.BookingID
                                         select x).FirstOrDefault();

                toArchive.Archived = DateTime.Now;
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Booking {0} is archived on {1} ", toArchive.BookingID, toArchive.Archived));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("archiveBooking() - An error occurred: '{0}'", ex));
                }
            }
        }


        ///<remarks>  
        ///  Filters through all unconfirmed bookings in database by user criteria
        ///  </remarks>
        /// <param name="customer"></param>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <param name="room"></param>
        /// <returns>list of Bookings</returns>          
        public List<BookingBO> findUnconfirmedBookingByFilters(String id, DateTime? date, String customer, String room )
        {
            // fisrtly filter out all past bookings and confirmed/archived bookings
            List<BookingBO> result = getAllFromTable();
            if (result != null)
            {
                result.RemoveAll(booking => DateTime.Compare(booking.Date.Date, DateTime.Now.Date) < 0 || booking.Confirmed != null || booking.Archived != null);
            }
            else return null;
                
            // filter out bookings, that do not contain id filter value
            if (!String.IsNullOrEmpty(id) && result !=null)
            {
                result.RemoveAll(booking => !booking.BookingID.ToString().Contains(id));         
            }
            else if (result == null)    return null;

            // filter out bookings, that do not match by date
            if (date != null && result != null)
            {
                result.RemoveAll(booking => DateTime.Compare(booking.Date.Date, ((DateTime)date).Date) != 0);
                
            } else if (result == null)  return null;

            // filter out ones that do not have mathing customer name part
            if (!String.IsNullOrEmpty(customer) && result != null)
            {
                result.RemoveAll(booking => !booking.CustomerCompany.ToLower().Contains(customer.ToLower()));
               
            }else if (result == null)   return null;
            
            // filter out by unmatching room name part
            if (!String.IsNullOrEmpty(room) && result != null)
            {
                result.RemoveAll(booking => !booking.Room.ToLower().Contains(room.ToLower()));
                
            } else if (result == null)  return null;
 
            return result;
           
        }


        ///<remarks>  
        ///  Filters through all confirmed bookings in database by user criteria
        ///  </remarks>
        /// <param name="customer"></param>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <param name="room"></param>
        /// <returns>list of Bookings</returns>    
        public List<BookingBO> findConfirmedBookingByFilters(String id, DateTime? date, String customer, String room)
        {
            // fisrtly filter out all past bookings and unconfirmed/archived bookings
            List<BookingBO> result = getAllFromTable();
            if (result != null)
            {
                result.RemoveAll(booking => DateTime.Compare(booking.Date.Date, DateTime.Now.Date) < 0 || booking.Confirmed == null || booking.Archived != null);
            }
            else return null;

            // filter out bookings, that do not contain id filter value
            if (!String.IsNullOrEmpty(id) && result != null)
            {
                result.RemoveAll(booking => !booking.BookingID.ToString().Contains(id));
            }
            else if (result == null) return null;

            // filter out bookings, that do not match by date
            if (date != null && result != null)
            {
                result.RemoveAll(booking => DateTime.Compare(booking.Date.Date, ((DateTime)date).Date) != 0);

            }
            else if (result == null) return null;

            // filter out ones that do not have mathing customer name part
            if (!String.IsNullOrEmpty(customer) && result != null)
            {
                result.RemoveAll(booking => !booking.CustomerCompany.ToLower().Contains(customer.ToLower()));

            }
            else if (result == null) return null;

            // filter out by unmatching room name part
            if (!String.IsNullOrEmpty(room) && result != null)
            {
                result.RemoveAll(booking => !booking.Room.ToLower().Contains(room.ToLower()));

            }
            else if (result == null) return null;

            return result;

        }


        /// <remarks>
        /// clears Bookings Table. 
        /// Used for development of app, to ensure uniform data in DB on all dev stations
        /// </remarks>
        public void emptyTable()
        {
            _log.Trace("in emptyTable()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                foreach (var row in db.Bookings)
                    db.Bookings.Remove(row);
                db.SaveChanges();
            }
        }

    }
}
