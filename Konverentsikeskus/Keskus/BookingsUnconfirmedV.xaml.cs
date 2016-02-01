using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Keskus.BLL.Booking;
using Keskus.BLL;
using System.Windows.Input;
using System;
using Keskus.BLL.Contact;
using Keskus.ViewHelpers;
using System.Globalization;

namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for BookingsUnconfirmedV.xaml
    /// From this view user can access the list of future unconfirmed bokings, starting from today.
    /// Filtering by ID, event date, customer name and room name is enabled.
    /// Clicking on single row (booking) displays booking details on bottom half of the page.
    /// Each booking may be modified by user with buttons "Kinnita" - confirm booking, "Muuda" - change/update booking details, and "Kustuta" - delete booking.
    /// Deleting booking requires additional confirmation by clicking OK in MessageBox.
    /// </remarks>
    public partial class BookingsUnconfirmedV : Page
    {

        BookingService _bookingSrv = new BookingService();
        List<BookingBO> _bookings;
        BookingBO _selectedBooking;
        ViewHelper _helper = new ViewHelper();

        // current admin, accessing the page;
        String _admin;
        
       
        //constructor, receives an admin name parameter from MainMenuV
        public BookingsUnconfirmedV( string adminName)
        {
            InitializeComponent();
            _admin = adminName;
            
            // fill DataGrid with unconfirmed bookings
            _bookings = _bookingSrv.getUnconfirmedFromTable();
            UnconfBookingsGrid.ItemsSource = _bookings;

            //allows to filter events only for today and future dates, as DataGrid shows only today and future bookings
            txtFilterDate.DisplayDateStart = DateTime.Today;          
        }

        #region booking action button methods

        /// <remarks>
        /// Confirms _selectedBooking booking on button click event
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            _selectedBooking = (BookingBO)UnconfBookingsGrid.SelectedItem;
            _bookingSrv.confirmBooking(_selectedBooking);
            // reloads page to remove newly confirmed booking from table
            updateWindow();
        }

        /// <remarks>
        /// Deletes (actually, archives) selected booking. 
        /// It is removed from current list, with assigned "archived" timestamp.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            _selectedBooking = (BookingBO)UnconfBookingsGrid.SelectedItem;
            // Messagebox appears to prevent accidental user events. OK button confirms user intention to delete
            MessageBoxResult dialogResult = MessageBox.Show("Kustutan valitud broneeringu?", "Oled sa kindel?", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.OK)
            {
                _bookingSrv.archiveBooking(_selectedBooking);
                //window is reloaded after booking is deleted
                updateWindow();
            } 
        }

        /// <remarks>
        /// Navigates to booking update view (BookingUpdateV) pre-filled with selected booking details
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            _selectedBooking = (BookingBO)UnconfBookingsGrid.SelectedItem;
            this.NavigationService.Navigate(new BookingUpdateV(_selectedBooking, _admin));
        }

        #endregion

        #region UI user interaction related methods

        /// <remarks>
        /// Fills the Booking detailed view on bottom of the page with the _selectedBooking info.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnconfBookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            CultureInfo culture = new CultureInfo("de-DE");
            _selectedBooking = (BookingBO)UnconfBookingsGrid.SelectedItem;

            // if there is a booking with active selection in the DataGrid, textBlocks are filled with booking info
            if (_selectedBooking != null)
            {
                txtID.Text = _selectedBooking.BookingID.ToString();
                txtDate.Text = _selectedBooking.Date.ToString("d", culture);
                txtRoom.Text = _selectedBooking.Room;
                txtParticipants.Text = _selectedBooking.Participants.ToString();
                txtCustomer.Text = _selectedBooking.CustomerCompany;
                txtCustomerPIC.Text = _selectedBooking.CustomerContactPerson;
                txtCreated.Text = _selectedBooking.Created.ToString("d", culture);
                txtAdmin.Text = _selectedBooking.Admin;
                txtAdditional.Text = _selectedBooking.AdditionalInfo;

                //in ideal world must never execute first condition
                if (_selectedBooking.Confirmed != null)
                {
                    txtConfirmed.Text = ((DateTime)_selectedBooking.Confirmed).ToString("g", culture);
                }
                else txtConfirmed.Text = "";// must always execute this condition, as this is Unconfirmed Booking View

                // displays first available contact value, if there is any in the table
                ContactService contactSrv = new ContactService();
                List<ContactBO> contacts = contactSrv.getAllFromTableByCustomerID(_selectedBooking.CustomerID);
                if (contacts.Count > 0)
                {
                    txtContact.Text = contacts[0].Value.ToString();
                }
                else txtContact.Text = "";
        
            }
        }


        /// <remarks>
        /// Reloads items from database and updates the view. 
        /// </remarks>
        private void updateWindow()
        {
            _bookings = _bookingSrv.getUnconfirmedFromTable();
            UnconfBookingsGrid.ItemsSource = _bookings;
        }


        /// <remarks>
        /// Removes text placeholder from DatePicker on load
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            // as this method is called every time the view is reloaded (even on NavigationService.GoBack() case),
            // this allows to refresh data on page after booking update & return from update page
            updateWindow();

            //removes placeholder
            _helper.clearDatePickerPlaceholder(sender);
        }

        /// <remarks>
        /// provides check against non-numerical input in textbox
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_helper.IsTextAllowed(e.Text);
        }

        #endregion


        #region filtering buttons

        /// <remarks>
        /// Sets all filter values to 0.
        /// View is reloaded to display all available confirmed bookings
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteFilter_Click(object sender, RoutedEventArgs e)
        {
            txtFilterId.Text = "";
            txtFilterDate.SelectedDate = null;
            txtFilterCustomer.Text = "";
            txtFilterRoom.Text = "";
            updateWindow();      
        }


        /// <remarks>
        /// Starts filtering by user defined criteria. 
        /// Uses all 4 filter fields: Booking ID, Date, Customer and Room
        /// As a result displays filtered items in view 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            Filter criteria = new Filter(txtFilterId.Text, txtFilterDate.SelectedDate, txtFilterCustomer.Text, txtFilterRoom.Text);
            List<BookingBO> filtered = criteria.doFilter(false);
            UnconfBookingsGrid.ItemsSource = filtered;
        }



        #endregion


    }
}
