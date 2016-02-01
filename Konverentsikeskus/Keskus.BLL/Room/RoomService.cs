using System;
using System.Collections.Generic;
using System.Linq;
using Keskus.DAL;
using Keskus.BLL.Security;

namespace Keskus.BLL.Room
{

    /// <remarks>
    /// Class contains services for Room business object
    /// </remarks>
    public class RoomService
    {
        //application logic events are logged into log.txt file
        private Logger _log;

        // user action events are logged into userlog.txt file
        private Logger _userlog;

        public RoomService()
        {
            _log = new Logger("RoomService()");
            _log.Trace("Init");
            _userlog = new Logger(true, "Rooms data is changed");
        }

        /// <remarks>
        /// gets all Room objects from DB table
        /// </remarks>
        /// <returns> list of rooms</returns>
        public List<RoomBO> getAllFromTable()
        {
            _log.Trace("in getAllFromTable()");
            using (var db = new Keskus_baasEntities())
            {
                List<RoomBO> tableContent = db.Rooms
                    .ToList()
                    .Select(x => new RoomBO(x))
                    .ToList();
                return tableContent;
            }
        }


        /// <remarks>
        /// gets room object from DB table by name
        /// </remarks>
        /// <param name="roomName"></param>
        /// <returns>room object</returns>
        public RoomBO getRoomByName(string roomName)
        {
            _log.Trace("in getRoomByName()");
            using (var db = new Keskus_baasEntities())
            {
                return new RoomBO(db.Rooms
                    .Where(x => x.Name == roomName)
                    .FirstOrDefault());
            }
        }


        /// <remarks>
        /// rewrites existing room object property valued with user input
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="active"></param>
        /// <param name="seats"></param>
        /// <param name="name"></param>
        public void updateRoom(int id, bool active, int seats, String name)
        {
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Room updated = (from x in db.Rooms
                                    where x.RoomID == id
                                    select x).FirstOrDefault();
                
                if (updated != null)
                {
                    // userlog registers old values before update
                    _userlog.Trace(string.Format("Room updated, from: {0}, {1}, {2}", updated.Name, updated.Seats, updated.Active));

                    updated.Active = active;
                    updated.Seats = seats;
                    updated.Name = name;
                }
 
                try
                {
                    db.SaveChanges();
                    _userlog.Trace(string.Format("Room updated, to: {0}, {1}, {2}", updated.Name, updated.Seats, updated.Active));
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: '{0}'", e);
                    throw;
                }


            }
        }
        /// <remarks>
        /// finds room indes in DB by room name as string
        /// </remarks>
        /// <param name="rooms"></param>
        /// <param name="roomName"></param>
        /// <returns> room index as integer</returns>
        public int getRoomIndexFromList(List<RoomBO> rooms, string roomName)
        {
            foreach (RoomBO room in rooms)
            {
                if (room.Name == roomName)
                {
                    return rooms.IndexOf(room);
                }
            }
            return -1;
        }


        /// <remarks>
        /// gets rooms with property Active == true from DB table
        /// </remarks>
        /// <returns>list of active rooms</returns>
        public List<RoomBO> getActiveRooms()
        {
            using (var db = new Keskus_baasEntities())
            {
                var activeRooms = db.Rooms
                    .Where(x => x.Active == true)
                    .ToList()
                    .Select(x => new RoomBO(x))
                    .ToList();
                if (activeRooms != null) return activeRooms;
                else return null;
                
            }
        }





        /// <remarks>
        /// adds new Room entry to DB
        /// Used for development of app, to ensure uniform data in DB on all dev stations
        /// </remarks>
        public void addNew(string name, int seats, bool active)
        {
            _log.Trace("in addNew()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                DAL.Room room = new DAL.Room
                {
                    Name = name,
                    Seats = seats,
                    Active = active
                };
                db.Rooms.Add(room);
                try
                {
                    db.SaveChanges();
                    //userlog registeres new db entry added event 
                    _userlog.Trace(string.Format("New room added: {0}, {1}, {2}", room.Name, room.Seats, room.Active));
                }
                catch (Exception ex)
                {
                    _log.Trace(string.Format("addNew() - An error occurred: '{0}'", ex));
                }
            }
        }

        /// <remarks>
        /// clears Room Table. 
        /// Used for development of app, to ensure uniform data in DB on all dev stations
        /// </remarks>
        public void emptyTable()
        {
            _log.Trace("in emptyTable()");
            using (Keskus_baasEntities db = new Keskus_baasEntities())
            {
                foreach (var row in db.Rooms)
                    db.Rooms.Remove(row);
                db.SaveChanges();
            }
        }


    }
}
