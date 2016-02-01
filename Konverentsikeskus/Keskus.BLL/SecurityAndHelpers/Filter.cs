using Keskus.BLL.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keskus.BLL
{
    /// <remarks>
    /// Class Filter handles the booking filtering by one or multiple criteria in BookingsConfirmed and BookingsUnconfirmed Views.
    /// </remarks>
    public class Filter
    {

        #region private fields
        private String _bookingID;
        private DateTime? _bookingDate;
        private String _customerName;
        private String _roomName;
        #endregion

        #region public properties;

        public String BookingID
        {
            get { return _bookingID; }
            set { _bookingID = value; }
        }

        public DateTime? BookingDate
        {
            get { return _bookingDate; }
            set { _bookingDate = value; }
        }

        
        public String CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        public String RoomName
        {
            get { return _roomName; }
            set { _roomName = value; }
        }
        #endregion

        public Filter(String id, DateTime? date, String customer, String room)
        {
 
            _bookingID = id;
            _bookingDate = date;
            _customerName = customer;
            _roomName = room;

        }

        public List<BookingBO> doFilter(bool confirmed)
        {
            List<BookingBO> result = new List<BookingBO>();
            BookingService bookingSrv = new BookingService();
            if (confirmed == true)
            {
                result = bookingSrv.findConfirmedBookingByFilters(_bookingID, _bookingDate, _customerName, _roomName);
            }
            else
            {
                result = bookingSrv.findUnconfirmedBookingByFilters(_bookingID, _bookingDate, _customerName, _roomName);
            }
            return result;
        }




    }
}
