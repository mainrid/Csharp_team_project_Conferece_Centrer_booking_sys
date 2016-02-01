namespace Keskus.BLL.Room
{


    /// <remarks>
    /// Class contains description of Room business object
    /// </remarks>
    public class RoomBO
    {
        #region properties
        public int RoomID { get; set; }
        public int Seats { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } = false;
        #endregion

        
        #region constructors
        public RoomBO(Keskus.DAL.Room room)
        {
            this.RoomID = room.RoomID;
            this.Name = room.Name;
            this.Seats = room.Seats;
            this.Active = room.Active;
               
        }
        #endregion

    }
}
