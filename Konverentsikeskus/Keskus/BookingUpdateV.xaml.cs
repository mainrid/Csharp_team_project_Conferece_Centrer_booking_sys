using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Keskus.BLL.Booking;
using Keskus.BLL.Room;
using Keskus.BLL.Customer;
using System.Collections.Generic;
using Keskus.BLL.Administrator;
using Keskus.ViewHelpers;

namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for BookingUpdatedV.xaml
    /// From this view user can create new Booking or modify details for existing booking.
    /// In case of new booking the firlds are empty
    /// In case if booking update fields are pre-filled with booking data
    /// Customer can be either chosen from database, or created New.
    /// Several input validation methods ensure correct information is stored in DB
    /// </remarks>
    public partial class BookingUpdateV : Page
    {

        ViewHelper _helper = new ViewHelper();
        BookingService _bookingSrv = new BookingService();
        AdministratorService _adminSrv = new AdministratorService();
        RoomService _roomSrv = new RoomService();
        CustomerService _custSrv = new CustomerService();

        //booking details, accessible through entire class
        AdministratorBO _admin;
        CustomerBO _selectedCustomer;
        RoomBO _selectedRoom;
        DateTime _selectedDate;
        int _participants;
        String _additionalInfo = "";
        BookingBO _selectedBooking;

        List<RoomBO> _rooms;


        //constructor, is used to register new booking.
        public BookingUpdateV(string adminName)
        {
            InitializeComponent();

            //fill Room CombBox with active rooms from DB
            _rooms = _roomSrv.getActiveRooms();
            cboxRoom.ItemsSource = _rooms;
            cboxRoom.DisplayMemberPath = Name;

            //limit DatePicker choice to "starting from Today" only
            txtDate.DisplayDateStart = DateTime.Today;

           
            _admin = (AdministratorBO)_adminSrv.getAdminByName(adminName);
            listCustomers.ItemsSource = _custSrv.getAllFromTable();
        }

        ////constructor, is used to update existing booking details.
        public BookingUpdateV(BookingBO selectedBooking, string adminName)
        {
            InitializeComponent();

            this._selectedBooking = selectedBooking;

            // pre-fill text boxes with booking data
            txtCustomer.Text = selectedBooking.CustomerCompany;
            txtParticipants.Text = selectedBooking.Participants.ToString();
            txtAdditionalInfo.Text = selectedBooking.AdditionalInfo;
            txtDate.SelectedDate = selectedBooking.Date;

            //searches List<CustomersBO> by Customer Name
            listCustomers.ItemsSource = _custSrv.searchCustomerByCompany(txtCustomer.Text);
            listCustomers.SelectAll();

            //limit DatePicker choice to "starting from Today" only
            txtDate.DisplayDateStart = DateTime.Today;


            //fill combobox with active rooms from DB
            _rooms = _roomSrv.getActiveRooms();
            cboxRoom.ItemsSource = _rooms;

            // in case room is inactive leave combobox empty
            if (_roomSrv.getRoomIndexFromList(_rooms, selectedBooking.Room) != -1)
            {
                cboxRoom.SelectedValue = selectedBooking.Room;
                cboxRoom.SelectedIndex = _roomSrv.getRoomIndexFromList(_rooms, selectedBooking.Room);
            }

    
            _admin = (AdministratorBO)_adminSrv.getAdminByName(adminName);
        }

        #region Customer buttons (add/search)

        /// <remarks>
        /// if existng customer if not found froom database, user can input new customer by clicking buttom "Lisa klient".
        /// Navigates to CustomerAddV
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new CustomerAddV());
        }



        /// <remarks>
        /// Searches customer database by user input (customer name part)
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCustomer_Click(object sender, RoutedEventArgs e)
        {
            // display instructions
            lblInstructions.Content = "Vali nimekirjast:";

            //// if no user input defined, display all customers
            //if (txtCustomer.Text.Equals(""))
            //{
            //    listCustomers.ItemsSource = _custSrv.getAllFromTable();
            //}
            ////if there is user input, search by company name
            //else
            listCustomers.ItemsSource = _custSrv.searchCustomerByCompany(txtCustomer.Text);

            //if no customers found, display warning
            if (listCustomers.Items.Count == 0)
            {
                lblInstructions.Content = "Klienti ei leitud";
            }

        }

        #endregion


        #region UI user interaction related methods

        /// <remarks>
        /// fills Customer name TextBox with selected Customer Company Name from DB
        /// </remarks>
        private void listCustomers_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listCustomers.SelectedItem != null)
            {
                _selectedCustomer = (CustomerBO)listCustomers.SelectedItem;
                txtCustomer.Text = (String)_selectedCustomer.CompanyName;
            }
        }


        /// <remarks>
        /// Removes text placeholder from DatePicker on load
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {

            // this part is added to DatePicker_Loaded method because DatePicker_Loaded is called even on NavigationService.GoBack();
            // Find the frame sender is in.
            Frame pageFrame = null;
            DependencyObject currParent = VisualTreeHelper.GetParent(this);
            while (currParent != null && pageFrame == null)
            {
                pageFrame = currParent as Frame;
                currParent = VisualTreeHelper.GetParent(currParent);
            }
            // dirty workable solution to pass object as frame Tag while using NavigationService.GoBack();
            // passing added Customer, to be pre-filled in booking details
            if (pageFrame.Tag != null)
            {
                CustomerBO addedCustomer = (CustomerBO)pageFrame.Tag;
                pageFrame.Tag = null;
                List<CustomerBO> list = new List<CustomerBO>();
                list.Add(addedCustomer);
                listCustomers.ItemsSource = list;
                listCustomers.SelectedItem = addedCustomer;
                txtCustomer.Text = addedCustomer.CompanyName;
            }

            //removes placeholder text
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


        #region View navigation buttons

        /// <remarks>
        /// returns to previos page without saving any input/changes
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Find the frame.
            Frame pageFrame = null;
            DependencyObject currParent = VisualTreeHelper.GetParent(this);
            while (currParent != null && pageFrame == null)
            {
                pageFrame = currParent as Frame;
                currParent = VisualTreeHelper.GetParent(currParent);
            }
            // go back
            pageFrame.NavigationService.GoBack();
        }

        /// <remarks>
        /// Saves validates and saves the data in fields as new BookingBO and returns to previous page.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            lblWarningBook.Text = "";

            //check if date is selected, if not, display warning
            if (txtDate.SelectedDate == null)
                lblWarningBook.Text = "Palun vali kuupäev";
            
            else
            {
                //check, if date isn't in the past 
                if (DateTime.Compare((DateTime)txtDate.SelectedDate,DateTime.Today)<0)
                {
                    lblWarningBook.Text = "Valitud kuupäev on minevikus";
                }
                //if date is selected, convert to DateTime and assing
                _selectedDate = (DateTime)txtDate.SelectedDate;

                //check if Room is selected, of not, display warning
                if (cboxRoom.SelectedItem == null)
                    lblWarningBook.Text = "Palun vali ruum";
                else
                {
                    //if Room is selected, assign
                    _selectedRoom = (RoomBO)cboxRoom.SelectedItem;

                    //check if customer is selected, if not,  display warning
                    if ((CustomerBO)listCustomers.SelectedItem == null)
                    {
                        lblWarningBook.Text = "Palun vali klient";
                    }
                    else
                    {
                        //if customer is selected, assign. Clear warning lable
                        _selectedCustomer = (CustomerBO)listCustomers.SelectedItem;


                        if (int.TryParse(txtParticipants.Text, out _participants) == false)
                        {
                            lblWarningBook.Text = "Palun sisesta osalejate arv";
                            return;
                        }
                        
                            _participants = int.Parse(txtParticipants.Text);

                        // if number is negative or 0, display warning
                        if (_participants <= 0)
                        {
                            lblWarningBook.Text = "Palun sisesta osalejate arv";
                            return;
                        }


                        if (lblWarning.Text.Equals("") && lblWarning2.Text.Equals("") && lblWarningBook.Text.Equals(""))
                        {

                            //if all conditions filfilled and values assigned, assign optional values
                            if ((_participants > 0) && (_selectedRoom != null) && (_selectedDate != null) && (_selectedCustomer != null))
                            {

                                _additionalInfo = txtAdditionalInfo.Text;

                                // if user created new booking, add new booking to DB
                                if (_selectedBooking == null)
                                {
                                    _bookingSrv.addNew(_selectedDate, _selectedRoom.RoomID, _selectedCustomer.CustomerID, _participants, DateTime.Now, _admin.AdminID, _additionalInfo);
                                }
                                // if user updates existing booking, update booking by Id
                                else
                                {
                                    _bookingSrv.UpdateById(_selectedBooking.BookingID, _selectedDate, _selectedRoom.RoomID, _selectedCustomer.CustomerID, _participants, _additionalInfo);
                                }

                                //return to previous page
                                // Find the frame.
                                Frame pageFrame = null;
                                DependencyObject currParent = VisualTreeHelper.GetParent(this);
                                while (currParent != null && pageFrame == null)
                                {
                                    pageFrame = currParent as Frame;
                                    currParent = VisualTreeHelper.GetParent(currParent);
                                }

                                // gets us back to View we came from
                                pageFrame.NavigationService.GoBack();


                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region user input validation

        /// <remarks>
        /// Doesn't allow to input past dates by hand
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkDate(object sender, RoutedEventArgs e)
        {
            if (txtDate.SelectedDate != null)
            {
                int past = DateTime.Compare((DateTime)txtDate.SelectedDate, DateTime.Today);
                if (past < 0)
                {
                    txtDate.SelectedDate = DateTime.Today;
                    txtDate.Text = DateTime.Today.ToString();
                }
            }

        }

        /// <remarks>
        /// restrict pasting data to field 
        /// solution from http://stackoverflow.com/questions/938145/make-wpf-textbox-as-cut-copy-and-paste-restricted
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        /// <remarks>
        /// checks  room seats - participants pair. If participants value exeeds room seats, displays warning. 
        /// checks room-date pair. If there is a booking for this pair already, displays warning.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxRuum_selectionChanged(object sender, SelectionChangedEventArgs e)
        {

            _selectedRoom = (RoomBO)(sender as ComboBox).SelectedItem;

            // checks if any participants info is stores, that exeeds room seats value, if true, display warning
            if (_participants > 0 && _selectedRoom != null && _participants > _selectedRoom.Seats)
            {
                lblWarning2.Text = String.Format("Osalejate arv ületab valitud ruumi mahutavust ({0} kohta).",_selectedRoom.Seats.ToString());
            }
            else
            {
                lblWarning2.Text = "";
            }

            // checks, if there are already bookings for this date-room pair if true, display warning
            if (txtDate.SelectedDate != null)
            {
                List<BookingBO> allBookings = _bookingSrv.getAllFromTable();
                if (allBookings != null)
                {
                    foreach (BookingBO booking in allBookings)
                    {
                        if (DateTime.Compare(booking.Date, (DateTime)txtDate.SelectedDate) == 0 && booking.Room.ToString().Equals(_selectedRoom.Name.ToString()))
                        {

                            if (_selectedBooking != null && _selectedBooking.BookingID != booking.BookingID || _selectedBooking == null)
                            {
                                lblWarning.Text = string.Format("Selleks kuupäevaks ruum {0} on juba broneeritud. Vali muu kuupäev/ruum", _selectedRoom.Name.ToString());
                                return;
                            }
                            else
                            {
                                lblWarning.Text = "";
                            }
                        }
                        else
                        {
                            lblWarning.Text = "";
                        }

                    }
                }
            }

        }

        /// <remarks>
        /// checks room-date pair. If there is a booking for this pair already, displays warning.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DateTime)txtDate.SelectedDate == null)
            {
                lblWarningBook.Text = "Palun vali kuupäev";
                return;
            }
            _selectedDate = (DateTime)txtDate.SelectedDate;

            if (cboxRoom.SelectedItem != null)
            {
                _selectedRoom = (RoomBO)cboxRoom.SelectedItem;
                List<BookingBO> allBookings = _bookingSrv.getAllFromTable();
                foreach (BookingBO booking in allBookings)
                {
                    if (DateTime.Compare(booking.Date, _selectedDate) == 0 && booking.Room.ToString().Equals(_selectedRoom.Name.ToString()))
                    {
                        if (_selectedBooking == null || _selectedBooking.BookingID != booking.BookingID)
                        {
                            lblWarning.Text = string.Format("Selleks kuupäevaks ruum {0} on juba broneeritud. Vali muu kuupäev/ruum", _selectedRoom.Name.ToString());
                            return;
                        }
                        else
                        {
                            lblWarning.Text = "";
                        }
                    }
                    else
                    {
                        lblWarning.Text = "";
                    }
                }
            }
        }


        /// <remarks>
        /// checks  room seats - participants pair. If participants value exeeds room seats, displays warning. 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtParticipants_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtParticipants.Text, out _participants))
            {
                _participants = int.Parse(txtParticipants.Text);
            }
            if (_participants > 0 && _selectedRoom != null && _participants > _selectedRoom.Seats)
            {
                lblWarning2.Text = String.Format("Osalejate arv ületab valitud ruumi mahutavust ({0} kohta).", _selectedRoom.Seats.ToString());
            }
            else
            {
                lblWarning2.Text = "";
            }
        }
        #endregion

    }
}



