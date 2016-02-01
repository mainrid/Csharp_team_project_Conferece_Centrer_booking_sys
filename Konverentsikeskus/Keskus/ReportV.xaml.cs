using Keskus.BLL.Administrator;
using Keskus.BLL.Booking;
using Keskus.BLL.Customer;
using Keskus.BLL.Report;
using Keskus.BLL.Room;
using Keskus.ViewHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;



namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for ReportV.xaml
    /// Allows user to generate see and export to .xslx file reports on center activity by different criteria and period.
    /// Only one criteria per report is supporter. 
    /// Report can be generated for period in the past only.
    /// </remarks>
    public partial class ReportV : Page
    {

        ViewHelper _helper = new ViewHelper();
        public CustomerBO _selectedCustomer;
        public RoomBO _selectedRoom;
        public AdministratorBO _selectedAdmin;
        public DateTime _startDate = DateTime.Today.AddDays(-1);
        public DateTime _endDate = DateTime.Today.AddDays(-1);
        List<BookingBO> _output;

        //default constructor;
        public ReportV()
        {
            InitializeComponent();
        }


        // constructor for current logged in admin
        public ReportV(String adminName)
        {

            InitializeComponent();
            // determine last available date for period date pickers
            endDate.DisplayDateEnd = DateTime.Today.AddDays(-1);
            startDate.DisplayDateEnd = DateTime.Today.AddDays(-1);
            if (startDate.SelectedDate == null)
            {
                startDate.DisplayDate = DateTime.Today.AddDays(-1);
            }
            if (endDate.SelectedDate == null)
            {
                endDate.DisplayDate = DateTime.Today.AddDays(-1);
            }


            RoomService roomSrv = new RoomService();
            CustomerService customerSrv = new CustomerService();
            AdministratorService adminSrv = new AdministratorService();

                      
            //populate list of Customers with Customer Names from database. 
            List<CustomerBO> customers = customerSrv.getAllFromTable();
            List<String> customerNames = new List<String>();
            foreach (CustomerBO customer in customers)
            {
                String customerName = customer.CompanyName;
                customerNames.Add(customerName);
            }
            listCustomers.ItemsSource = customerNames;

            //populate comboBox of Rooms with room names from database
            List<RoomBO> rooms = roomSrv.getAllFromTable();
            List<String> roomNames = new List<String>();
            foreach (RoomBO room in rooms)
            {
                String roomName = room.Name;
                roomNames.Add(roomName);
            }
            cboxRooms.ItemsSource = roomNames;
            
        }


        #region  UI handling methods




        /// <remarks>
        /// on Customer selection prepare CustomerBO object by chosen customer name.
        /// </remarks>
        private void CustomerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedCompanyName = (String)listCustomers.SelectedItem;
            CustomerService customerSrv = new CustomerService();
            _selectedCustomer = (CustomerBO)customerSrv. getCustomerByName(selectedCompanyName);
        }

        /// <remarks>
        /// on Room selection prepare CustomerBO object by chosen customer name.
        /// </remarks>
        private void RoomSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selectedRoomName = (String)cboxRooms.SelectedItem;
            RoomService roomSrv = new RoomService();
            _selectedRoom = (RoomBO)roomSrv.getRoomByName(selectedRoomName);
        }

        #endregion

        #region report generating and export buttons

        /// <remarks>
        /// checks if all neccessary criteria is selected and generates report by selected criteria.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            // set warning labels and report view to empty prior to checking
            lblError.Content = "";
            txtReport.ItemsSource = null;
            
            ReportService reportSrv = new ReportService();
            
            //check if both start and end date are selected, start date is not in the future and end date is later than start date
            // if not compliant, display warning
            if (startDate.SelectedDate != null && endDate.SelectedDate != null)
            {
                _startDate = (DateTime)startDate.SelectedDate;
                _endDate = (DateTime)endDate.SelectedDate;

                if (_startDate > DateTime.Now)
                {
                    lblError.Content = "Alguskuupäev ei tohi olla tulevikus";
                }
                else if (_endDate < _startDate)
                {
                    lblError.Content = "Perioodi lõpp ei tohi olla varem kui algus";
                }
                else if (_startDate <= _endDate)
                {
                    // if Radio Button Customer Report is checked, generate Report by Customer for selected period
                    if (rbtnCustomerReport.IsChecked == true && _selectedCustomer !=null)
                    {
                        CustomerReportBO currentReport = new CustomerReportBO((DateTime)_startDate, (DateTime)_endDate, _selectedCustomer);
                        _output = reportSrv.getReport(currentReport);          
                    }

                    // if Radio Button Room Report is checked, generate Report by Room for selected period
                    else if (rbtnRoomReport.IsChecked == true && _selectedRoom != null)
                    {
                        RoomReportBO currentReport = new RoomReportBO((DateTime)_startDate, (DateTime)_endDate, _selectedRoom);
                        _output = reportSrv.getReport(currentReport);
                    }

                    // if Radio Button Full Report is checked, generate Report by all bookings for selected period
                    else if (rbtnFullReport.IsChecked == true)
                    {
                        ReportBO currentReport = new ReportBO((DateTime)_startDate, (DateTime)_endDate);
                        _output = reportSrv.getReport(currentReport);
                    }  
                       
                    
                    // if no criteria is selected
                    else
                    {
                        _output = null;
                        btnExport.IsEnabled = false;
                        lblError.Content = "Vali aruande kriteeriumid";
                    }
                    
                    // if there are rows to display, show in txtReport ListView
                    if (_output != null && _output.Count != 0)
                    {
                        txtReport.ItemsSource = _output;
                        btnExport.IsEnabled = true;
                    }

                    // if no rows to display, give a warning.
                    else if (_output != null && _output.Count == 0)
                    {
                        btnExport.IsEnabled = false;
                        lblError.Content = "Päringule vastavaid kirjeid ei leitud";
                    }
                    else
                    {
                        btnExport.IsEnabled = false;
                        lblError.Content = "Vali aruande kriteeriumid";
                    }
                }
            }
            else
            {
                lblError.Content = "Vali periood";
            }
        }

        /// <remarks>
        /// Handles report export to .xlsx file on button click
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ReportService reportSrv = new ReportService();
            List<ExportBO> exportReportList = new List<ExportBO>();
            
            //converts List<BookingBO> to List <ExportBO>
            if (_output != null && _output.Count != 0)
            {
                foreach (BookingBO booking in _output)
                {
                    ExportBO item = new ExportBO(booking);
                    exportReportList.Add(item);
                }
            }

            //converts List to DataTable and adds DataTable to DataSet
            System.Data.DataTable dtReport = reportSrv.ToDataTable<ExportBO>(exportReportList);
            DataSet dsReport = new DataSet();
            dsReport.Tables.Add(dtReport);

            //determines path for report file
            string reportFileName = "report.xlsx";
            string currentExecutionDirPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string reportFilePath = string.Format("{0}\\{1}{2}", currentExecutionDirPath, DateTime.Now.ToString("MMMMdd_yyyy_H_mm_ss"), reportFileName);

            //saving report to excel
            reportSrv.ExportToExcel(dsReport, reportFilePath);

            // check if file exists and open, if exists.
            FileInfo fi = new FileInfo(reportFilePath);
            if (fi.Exists)
            {
                System.Diagnostics.Process.Start(reportFilePath);
            }
            else
            {
                //file doesn't exist
            }
        }

        #endregion

    }
}