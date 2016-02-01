using Keskus.BLL.Administrator;
using Keskus.BLL.Booking;
using Keskus.BLL.Customer;
using Keskus.BLL.Room;
using System;
using System.Collections.Generic;

namespace Keskus.BLL.Report
{
    /// <remarks>
    /// Class contains description of Report business object and handles generating full report objects of the programm.
    /// </remarks>
    public class ReportBO
    {
        #region private fields
        private DateTime _startDate;
        private DateTime _endDate;
        private List<BookingBO> _report;

        #endregion

        #region public properties

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public List<BookingBO> Report
        {
            get { return _report; }
            set { _report = value; }
        }

        #endregion

        public ReportBO(DateTime start, DateTime end)
        {
            this._startDate = start;
            this._endDate = end;
           
        }

    }



    /// <remarks>
    /// Subclass of Report BO handles reports by Customer
    /// </remarks>
    public class CustomerReportBO : ReportBO
    {
        private CustomerBO _customer;

        public CustomerBO Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public CustomerReportBO(DateTime start, DateTime end, CustomerBO customer) : base(start, end)
        {
            this._customer = customer;
        }



    }

    /// <remarks>
    /// Subclass of Report BO handles reports by Room
    /// </remarks>
    public class RoomReportBO : ReportBO
    {
        private RoomBO _room;

        public RoomBO Room
        {
            get { return _room; }
            set { _room = value; }
        }

        public RoomReportBO(DateTime start, DateTime end, RoomBO room) : base(start, end)
        {
            this._room = room;
        }
    }

    
    /// <remarks>
    /// Class  creates objects for exported reports
    /// </remarks>
    public class ExportBO 
    {

        #region properties

        public int BookingID { get; set; }
        public DateTime Date { get; set; }
        public string CustomerCompany { get; set; } // org. nimetus
        public int Participants { get; set; }
        public string Room { get; set; }
        public string Admin { get; set; }

        #endregion

        #region constructor
        public ExportBO(BookingBO booking)
        {
            this.BookingID = booking.BookingID;
            this.Date = booking.Date;
            this.Participants = booking.Participants;
            this.CustomerCompany = booking.CustomerCompany;
            this.Room = booking.Room;
            this.Admin = booking.Admin;

            
         }
        #endregion
    }

}

