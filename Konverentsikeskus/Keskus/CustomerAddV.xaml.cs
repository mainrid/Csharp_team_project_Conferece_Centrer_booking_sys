using Keskus.BLL.Customer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for CustomerAddV.xaml
    /// From this view user can add new Customer to DB.
    /// Input validation methods ensure correct information is stored in DB.
    /// </remarks>
    public partial class CustomerAddV : Page
    {
        

        public CustomerAddV()
        {
            InitializeComponent();
        }

        #region view buttons

        /// <remarks>
        /// returns to previous page without saving
        /// </remarks>
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
            //return to previous frame
            pageFrame.NavigationService.GoBack();
        }

        /// <remarks>
        /// Validates user input and if OK, stores new CustomerBO in DB
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
            int newCustomerId = customerSrv.addNew(txtCompanyNameToAdd, txtCompanyContactNameToAdd);

            if (0 == newCustomerId)
            {
                txtErrorCustomer.Content = "Klient ei ole loodud! Proovi uuesti!";
                return;
            }

            txtErrorCustomer.Content = "";

            //GoTo CustomerUpdateV with  freshly stored customer details
            CustomerBO customer = customerSrv.getCustomerById(newCustomerId);
            this.NavigationService.Navigate(new CustomerUpdateV(customer, true));
        }

        #endregion

    }
}
