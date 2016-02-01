using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Keskus.BLL.Room;
using Keskus.BLL.Booking;
using System.Collections.Generic;
using Keskus.ViewHelpers;

namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for RoomUpdateV.xaml
    /// User can modify the amount of seats in the room and active/inactive status.
    /// </remarks>
    public partial class RoomUpdateV : Page

    {
        ViewHelper _helper = new ViewHelper();
        private RoomBO selectedRoom;
  
        public RoomUpdateV(RoomBO selectedRoom)
        {
            InitializeComponent();
  
            this.selectedRoom = selectedRoom;
            //fill the page with information of selected room.
            lblRoomName.Content = selectedRoom.Name.ToUpper();
            txtSeats.Text = selectedRoom.Seats.ToString();
            if (selectedRoom.Active == true)
            {
                active.IsChecked = true;
            }
            else
            {
                inactive.IsChecked = true;
            }
        }


        #region UI methods
        /// <remarks>
        /// input check - allows only numeric user input to textBox
        /// </remarks>
        private void CheckTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_helper.IsTextAllowed(e.Text);
        }

        #endregion

        #region Navigation

        ///<remarks> 
        /// Returns to previous page, discarding any changes
        /// </remarks>
        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Find the frame.
            Frame pageFrame = null;
            DependencyObject currParent = VisualTreeHelper.GetParent(this);
            while (currParent != null && pageFrame == null)
            {
                pageFrame = currParent as Frame;
                currParent = VisualTreeHelper.GetParent(currParent);
            }
            pageFrame.NavigationService.GoBack();
        }



        ///<remarks> 
        /// confirmation button saves the changes and returns to previous page
        /// </remarks>
        private void btnConfirmUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomService roomSrv = new RoomService();
            //saving data changes
            selectedRoom.Seats = int.Parse(txtSeats.Text);
            if (inactive.IsChecked== true)
            {
                selectedRoom.Active = false;
                // Warning against existing booking in deactivated room. If room is not active, booking row in 
                // Confirmed Booking abd Unconfirmed Booking views have red background and tooltip
                MessageBoxResult inactiveRoomWarning = MessageBox.Show("Kontrolli, kas sul on deaktiveeritud ruumis broneeringuid. Broneeringute nimekirjades need on märgitud punasega", "Ruum on deaktiveeritud", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
                else if (active.IsChecked == true)
            {
                selectedRoom.Active = true;
            }
  
            //updating object in database
            roomSrv.updateRoom(selectedRoom.RoomID, selectedRoom.Active, selectedRoom.Seats, selectedRoom.Name);

            //returning to previous page
            // Find the frame.
            Frame pageFrame = null;
            DependencyObject currParent = VisualTreeHelper.GetParent(this);
            while (currParent != null && pageFrame == null)
            {
                pageFrame = currParent as Frame;
                currParent = VisualTreeHelper.GetParent(currParent);
            }
            //goto previous page
            this.NavigationService.Navigate(new RoomsV());
        }
        #endregion

    }
}
