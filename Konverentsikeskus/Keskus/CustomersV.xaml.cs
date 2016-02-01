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
    /// Contains interaction logic for CustomersV.xaml
    /// The ListView displays all existing customers. 
    /// New customer can be added by clicking "Lisa klient"
    /// Once any ListView row is chosen, customer data cam be modified by clicking "Muuda andmed".
    /// ListView in the bottom displays contacts of selected customer. 
    /// </remarks>
    public partial class CustomersV : Page
    {
        public CustomersV()
        {
            InitializeComponent();

            CustomerService customerSrv = new CustomerService();
            List<CustomerBO> customers = customerSrv.getAllFromTable();
            listCustomers.ItemsSource = customers;
        }


        #region navigation buttons 
        /// <remarks>
        /// Navigates to CustomerAddV to add new customer.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CustomerAddV());
        }


        /// <remarks>
        /// Navigates to CustomerUpdateV with selectedCustomer as parameter
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerBO selectedCustomer = (CustomerBO)listCustomers.SelectedItem;
            this.NavigationService.Navigate(new CustomerUpdateV(selectedCustomer, false));
        }
        #endregion


        #region UI user interaction related methods

        /// <remarks>
        /// Displays customer details and contacts if any customer is selected in table
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listCustomers.SelectedItem != null)
            {
                CustomerBO selectedCustomer = (CustomerBO)listCustomers.SelectedItem;
                lblCompanyName.Content = selectedCustomer.CompanyName;
                lblContactPerson.Content = selectedCustomer.ContactPerson;

                ContactService contactSrv = new ContactService();
                List<ContactBO> customerContacts = contactSrv.getAllFromTableByCustomerID(selectedCustomer.CustomerID);

                ContactTypeService contactTypeSrv = new ContactTypeService();

                //adding ContactType Name to ContactBO Obj. by ContactType ID
                foreach (ContactBO customerContact in customerContacts)
                {
                    customerContact.ContactTypeName = contactTypeSrv.getNameById(customerContact.ContactTypeID);
                }

                listContacts.ItemsSource = customerContacts;
            }
        }

        #endregion

        /// <remarks>
        /// reloads the view. useful for NavigationService.GoBack();
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateView(object sender, RoutedEventArgs e)
        {
            CustomerService customerSrv = new CustomerService();
            List<CustomerBO> customers = customerSrv.getAllFromTable();
            listCustomers.ItemsSource = customers;
            listContacts.ItemsSource = null;

            Frame pageFrame = null;
            DependencyObject currParent = VisualTreeHelper.GetParent(this);
            while (currParent != null && pageFrame == null)
            {
                pageFrame = currParent as Frame;
                currParent = VisualTreeHelper.GetParent(currParent);
            }
            // if current frame has any assigned tags, remove.
            if (pageFrame.Tag != null)
            {
                pageFrame.Tag = null;
            }
        }

    }
}
