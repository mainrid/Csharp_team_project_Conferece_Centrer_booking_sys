namespace Keskus.BLL.Administrator
{
    /// <remarks>
    /// Class contains description of Admnistrator business object
    /// </remarks>
    public class AdministratorBO
    {
        #region private fields
        private int _adminId;
        private string _name;
        private bool _changeAllowed;
        private string _userName;
        private string _password;
        #endregion

        #region public properties
        public int AdminID
        {
            get { return _adminId; }
            set { _adminId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool ChangeAllowed
        {
            get { return _changeAllowed; }
            set { _changeAllowed = value; }
        }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        #endregion

        #region constructors   
        public AdministratorBO(Keskus.DAL.Administrator admin)
        {
            this.AdminID = admin.AdminID;
            this.Name = admin.Name;
            this.ChangeAllowed = admin.CanChange;
            this.Password = admin.Password;
            this.UserName = admin.Username;
        }
        #endregion

    }
}
