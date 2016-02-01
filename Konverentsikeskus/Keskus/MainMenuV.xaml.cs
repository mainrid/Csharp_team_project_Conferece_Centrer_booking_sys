using Keskus.BLL;
using System;
using System.Windows;



namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for MainManuV.xaml
    /// Allows user navigate between different pages of application by using menu buttons
    /// opens new page in a frame.
    /// </remarks>
    public partial class MainMenuV : Window
    {
        private Logger _userlog;
        private String _currentAdmin;
       

        // constructor creating " session" for user as admin
        public MainMenuV(string adminName)
        {
            InitializeComponent();
            _userlog = new Logger(true, "Logout attempt: ");
            _currentAdmin = adminName;
            txtAdminName.Text = adminName;
            frame.Navigate(new BookingsConfirmedV(_currentAdmin));

        }

        /// <remarks>
        /// logout button navigates user back to login screen and finalizes UserLog for current admin
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
                LoginV loginV = new LoginV();
                loginV.Show();
                this.Close();
            _userlog.Trace(string.Format("-------------------------------- successful at {0}. Admin: {1} -------------------------------", DateTime.Now, _currentAdmin));
        }


        /// <remarks>
        /// navigates to "new booking" page
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewBooking_Click(object sender, RoutedEventArgs e)
        {
            // Change the page of the frame.
            frame.Navigate(new BookingUpdateV(_currentAdmin)); 
        }


        /// <remarks>
        /// navigates to "customers" page
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            // Change the page of the frame.
            frame.Navigate(new CustomersV());
            
        }

        /// <remarks>
        /// navigates to "rooms" page
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRooms_Click(object sender, RoutedEventArgs e)
        {
            // Change the page of the frame.
            frame.Navigate(new RoomsV());
        }

        /// <remarks>
        /// navigates to "Confirmed Bookings" page
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfBookings_Click(object sender, RoutedEventArgs e)
        {
            // Change the page of the frame.
            frame.Navigate(new BookingsConfirmedV(_currentAdmin));
        }

        /// <remarks>
        /// navigates to "Unconfirmed Bookings" page
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnconfBookings_Click(object sender, RoutedEventArgs e)
        {
            // Change the page of the frame.
            frame.Navigate(new BookingsUnconfirmedV(_currentAdmin));
        }


        /// <remarks>
        /// navigates to "Reports" page
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            // Change the page of the frame.
            frame.Navigate(new ReportV(_currentAdmin));
           
        }
    
    }
}
