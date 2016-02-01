using System;

namespace Keskus.BLL.Customer
{
    /// <remarks>
    /// Class contains description of Customer business object
    /// </remarks>
    public class CustomerBO
    {
        #region private fields
        private int _customerID;
        private string _companyName;
        private string _contactPerson;
        #endregion

        #region public properties
        public int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public string ContactPerson
        {
            get { return _contactPerson; }
            set { _contactPerson = value; }
        }
        #endregion

        #region constructors

        public CustomerBO(Keskus.DAL.Customer customer)
        {
            this._customerID = customer.CustomertID;
            this._companyName = customer.CompanyName;
            this._contactPerson = customer.ContactPerson;
        }

      

        #endregion
    }
}
