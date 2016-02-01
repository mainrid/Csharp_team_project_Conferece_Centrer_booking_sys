using Keskus.DAL;
using System;
using System.Linq;


namespace Keskus.BLL.Booking
{
    /// <remarks>
    /// Class contains description of Booking business object
    /// </remarks>
    public class BookingBO
    {
        #region properties

        public int BookingID { get; set; }
        public DateTime Date { get; set; }
        public int RoomID { get; set; }
        public int CustomerID { get; set; }
        public int Participants { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Confirmed { get; set; }
        public int AdminID { get; set; }
        public string AdditionalInfo { get; set; }
        public string Room { get; set; }
        public bool RoomActive { get; set; }
        public string CustomerCompany { get; set; } // org. nimetus
        public string CustomerContactPerson { get; set; } // kontaktisik
        public string Admin { get; set; }
        public Nullable<System.DateTime> Archived { get; set; }

        #endregion

        #region constructors
        public BookingBO(Keskus.DAL.Booking booking)
        {
            this.BookingID = booking.BookingID;
            this.Date = booking.Date;
            this.RoomID = booking.RoomID;
            this.CustomerID = booking.CustomerID;
            this.Participants = booking.Participants;
            this.Created = booking.Created;
            this.Confirmed = booking.Confirmed;
            this.AdditionalInfo = booking.AdditionalInfo;
            this.Archived = booking.Archived;

            using (var db = new Keskus_baasEntities())
            {
                var getCustomer = db.Customers
                    .Where(x => x.CustomertID == booking.CustomerID)
                    .FirstOrDefault();
                //getting klient PIC name from Klients database
                this.CustomerContactPerson = getCustomer.ContactPerson;

                //getting klient ORG name from Klients database
                this.CustomerCompany = getCustomer.CompanyName;

                
                var getRoom = db.Rooms
                   .Where(x => x.RoomID == booking.RoomID)
                   .FirstOrDefault();

                //getting Room name from Ruums database
                this.Room = getRoom.Name;

                //getting Room status from Ruums database
                this.RoomActive = getRoom.Active;

                
                var getAdmin = db.Administrators
                   .Where(x => x.AdminID == booking.AdminID)
                   .FirstOrDefault();

                //getting Admin name from Administraators database
                this.Admin = getAdmin.Name;
            }

        }
        #endregion


    }

}

