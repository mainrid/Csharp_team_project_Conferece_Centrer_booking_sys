using Keskus.BLL.Contact;
using Keskus.BLL.ContactType;
using Keskus.BLL.Customer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for CustomerUpdatedV.xaml
    /// From this view user can modify and rewrite customer details.
    /// Input validation methods ensure correct information is stored in DB.
    /// </remarks>
    public partial class CustomerUpdateV : Page
    {
        bool _addingNewCustomer;
        CustomerBO _selectedCustomer;

        //constructor creating page with pre-filled data from selected customer details
        public CustomerUpdateV(CustomerBO selectedCustomer, bool newCustomer)
        {
            InitializeComponent();

            //determine, whether user want to add new customer, or update existing customer
            this._addingNewCustomer = newCustomer;
            this._selectedCustomer = selectedCustomer;

            //top part of the window is the same asd CustomerAddV, so the information is transferred
            txtCompanyName.Text = selectedCustomer.CompanyName;
            txtCompanyContactName.Text = selectedCustomer.ContactPerson;

            // filling ContactTypes combobox with respective values
            ContactTypeService contactTypeSrv = new ContactTypeService();
            List<ContactTypeBO> contactTypes = contactTypeSrv.getAllFromTable();
            cboxContacts.ItemsSource = contactTypes;

            // filling Contact list with respective values
            updateListContacts();
        }


        #region navigation buttons

        /// <summary>
        /// Navigates back to previous page without data saving
        /// </summary>
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

            //in case of adding new customer, user came  bu following scenario 
            //"origin page" -> CustomerAddV -> CustomerUpdateV
            //to return to origin page, remove CustomerAddV from navigation stack
            if (_addingNewCustomer) {
                pageFrame.NavigationService.RemoveBackEntry();
            }
            // go back to origin page
           pageFrame.NavigationService.GoBack();

            

            //this.NavigationService.Navigate(new CustomersV());

            //pass _selectedCustomer as frame tag to origin page with GoBack() method
            pageFrame.Tag = _selectedCustomer;

        }

        #endregion


        #region contact controls

        /// <remarks>
        /// updates existing contact entry with updated information from contactType combobox and value textbox
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeContact_Click(object sender, RoutedEventArgs e)
        {
            // check if contactType is selected
            txtError.Content = "";
            ContactTypeBO cboxContact = (ContactTypeBO)cboxContacts.SelectedItem;
            if (null == cboxContact)
            {
                txtError.Content = "palun valige liik.";
                return;
            }
            int sellectedContactTypeID = cboxContact.ContactTypeID;

            int thisCustomerID = _selectedCustomer.CustomerID;
            string valueToAdd = txtContactTypeValue.Text;

            //  check if contactType is selected
            ContactBO listContact = (ContactBO)listContacts.SelectedItem;
            if (null == listContact)
            {
                txtError.Content = "palun valige kontakt.";
                return;
            }
            int thisContactID = listContact.ContactID;

            // if new information is provided by customer, update the existing BO with new information, else, display warning
            if (!String.IsNullOrEmpty(valueToAdd))
            {
                ContactService contactSrv = new ContactService();
                contactSrv.UpdateById(thisContactID, thisCustomerID, sellectedContactTypeID, valueToAdd, DateTime.Now);

                updateListContacts();
                txtContactTypeValue.Text = "";
            }
            else
            {
                txtError.Content = "Väärtus on tühi!";
            }
        }

        /// <remarks>
        /// Adds new contact  entry with data provided by customer.
        /// Validates presence of user input before saving.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
            txtError.Content = "";
            ContactTypeBO cboxContact = (ContactTypeBO)cboxContacts.SelectedItem;
            

            if (null == cboxContact)
            {
                txtError.Content = "palun valige liik.";
                return;
            }
            
            int sellectedContactTypeID = cboxContact.ContactTypeID;


            int thisCustomerID = _selectedCustomer.CustomerID;
            string valueToAdd = txtContactTypeValue.Text;

            if (!String.IsNullOrEmpty(valueToAdd))
            {
                ContactService contactSrv = new ContactService();
                contactSrv.addNew(thisCustomerID, sellectedContactTypeID, valueToAdd, DateTime.Now);

                updateListContacts();
                txtContactTypeValue.Text = "";
            }
            else
            {
                txtError.Content = "Väärtus on tühi!";
            }
        }


        /// <remarks>
        /// updates Customer Contact list after adding/updating contacts.
        /// </remarks>
        private void updateListContacts()
        {
            ContactTypeService contactTypeSrv = new ContactTypeService();
            ContactService contactSrv = new ContactService();
            List<ContactBO> customerContacts = contactSrv.getAllFromTableByCustomerID(_selectedCustomer.CustomerID);

            //adding ContactType Name to ContactBO Obj. by ContactType ID
            foreach (ContactBO customerContact in customerContacts)
            {
                customerContact.ContactTypeName = contactTypeSrv.getNameById(customerContact.ContactTypeID);
            }

            listContacts.ItemsSource = customerContacts;
        }


        /// <remarks>
        /// if row selection in Customer contact table is changed, recpective data is stored to editable boxes
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtError.Content = "";
            ContactBO listContact = (ContactBO)listContacts.SelectedItem;
            ContactTypeService contactTypeSrv = new ContactTypeService();
            //It is called on add/modify contact on db also (not on user activity only)
            //So after new list update noone is selected.
            if (null != listContact)
            {
                // TODO: add selected ContactType to Combobox as "preselected"
                txtContactTypeValue.Text = listContact.Value;
               
            }
            
        }

        #endregion


        #region customer update button

        /// <remarks>
        /// Updates existing customer entry with data provided by customer.
        /// Validates presence of user input before saving.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            txtErrorCustomer.Content = "";
            string txtCompanyNameToAdd = txtCompanyName.Text;
            string txtCompanyContactNameToAdd = txtCompanyContactName.Text;

            if (String.IsNullOrEmpty(txtCompanyNameToAdd))
            {
                txtErrorCustomer.Content = "'Organisatsioon:' Väärtus on tühi!";
                return;
            }
            if (String.IsNullOrEmpty(txtCompanyContactNameToAdd))
            {
                txtErrorCustomer.Content = "'Kontaktisik:' Väärtus on tühi!";
                return;
            }
            CustomerService customerSrv = new CustomerService();
         
            customerSrv.UpdateById(_selectedCustomer.CustomerID, txtCompanyNameToAdd, txtCompanyContactNameToAdd);

            txtErrorCustomer.Content = "";
        }

        #endregion


    }
}
