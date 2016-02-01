using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Keskus.BLL.Room;
using System.Collections.ObjectModel;

namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for RoomsV.xaml
    /// The ListView displays all existing rooms. 
    /// Rooms cannot be added/deleted by user, only modified.
    /// Once any ListView row is chosen, room data cam be modified by clicking "Muuda andmed".
    /// plan drawing in the right highlights the selected room and reflects room status active/inactive. Doesn't act as user control, only as picture.
    /// </remarks>
    public partial class RoomsV : Page
    {
     
        ObservableCollection<Button> _planButtons = new ObservableCollection<Button>();
                
        
        public RoomsV()
        {
            InitializeComponent();
        
            RoomService roomSrv = new RoomService();
            List<RoomBO> rooms =roomSrv.getAllFromTable();
            listRooms.ItemsSource = rooms;
            _planButtons.Add(btnPlan1);
            _planButtons.Add(btnPlan2);
            _planButtons.Add(btnPlan3);
            namePlan(rooms);
            

        }

        #region UI  methods
        //this method is not scalable currently, but adds to visual appeal of application. 
        //Currently is set to work with 3 rooms, if there is more rooms, no visual output is provided
        private void namePlan(List<RoomBO>rooms)
        {
            var bc = new BrushConverter();
            if (_planButtons.Count == rooms.Count && _planButtons!=null)
            {
                for (var i = 0; i < _planButtons.Count; i++)
                {
                   _planButtons[i].Content = rooms[i].Name; 
                    if(rooms[i].Active == false)
                    {
                        _planButtons[i].Foreground = (Brush)bc.ConvertFrom("#FFABABAB");
                    }
                }

            }
            
        }




        /// <remarks>
        /// Handles UI events for room row selection from list
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RoomBO selectedRoom = (RoomBO)listRooms.SelectedItem;

            // if on plan drawing there is matching room name, paint the room green
            BrushConverter bc = new BrushConverter();
            foreach (Button button in _planButtons)
            {
                if (button.Content.ToString() == selectedRoom.Name)
                {
                    
                    button.Background = (Brush)bc.ConvertFrom("#FF7DBA1D");
                }
                else button.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
        }
        #endregion


        #region Navigation methods
        /// <remarks>
        /// Navigates to pre-filled RoomUpdateV 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomBO selectedRoom = (RoomBO)listRooms.SelectedItem;
            this.NavigationService.Navigate(new RoomUpdateV(selectedRoom));      
        }
        #endregion


    }
}
