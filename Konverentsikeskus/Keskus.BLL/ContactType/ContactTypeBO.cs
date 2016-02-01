namespace Keskus.BLL.ContactType
{

    /// <remarks>
    /// Class contains description of Contact Type business object
    /// </remarks>
    public class ContactTypeBO
    {
        #region private fields
        private int _contactTypeID;
        private string _name;
        private bool _compulsory;
        #endregion

        #region public properties
        public int ContactTypeID
        {
            get { return _contactTypeID; }
            set { _contactTypeID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public bool Compulsory
        {
            get { return _compulsory; }
            set { _compulsory = value; }
        }
        #endregion

        #region constructors

        public ContactTypeBO(Keskus.DAL.ContactType contactType)
        {
            this._contactTypeID = contactType.ContactTypeID;
            this._name = contactType.Name;
            this._compulsory = contactType.Compulsory;
        }

        #endregion
    }
}
