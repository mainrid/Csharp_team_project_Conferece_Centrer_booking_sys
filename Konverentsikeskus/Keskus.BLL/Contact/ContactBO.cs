using System;

namespace Keskus.BLL.Contact
{
    /// <remarks>
    /// Class contains description of Contact business object
    /// </remarks>
    public class ContactBO
    {
        #region private fields
        private int _contactID;
        private int _customerID;
        private int _contactTypeID;
        private string _contactTypeName = "";
        private string _value;
        private DateTime? _created;
        #endregion

        #region public properties
        public int ContactID
        {
            get { return _contactID; }
            set { _contactID = value; }
        }
        public int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }

        public int ContactTypeID
        {
            get { return _contactTypeID; }
            set { _contactTypeID = value; }
        }
        public string ContactTypeName
        {
            get { return _contactTypeName; }
            set { _contactTypeName = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public DateTime? Created
        {
            get { return _created; }
            set { _created = value; }
        }
        #endregion

        #region constructors

        public ContactBO(Keskus.DAL.Contact contact)
        {
            this._contactID = contact.ContactID;
            this._customerID = contact.CustomerID;
            this._contactTypeID = contact.ContactTypeID;
            this._value = contact.Value;
            this._created = contact.Created;
        }

        #endregion
    }
}
