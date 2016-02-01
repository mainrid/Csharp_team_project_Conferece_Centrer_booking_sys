using System.Collections.Generic;
using System.Windows;
using Keskus.BLL.Administrator;
using Keskus.BLL.Security;
using System;
using System.Security.Cryptography;
using Keskus.BLL;

namespace Keskus
{
    /// <remarks>
    /// Contains interaction logic for LoginV.xaml
    /// Allows user with active username/password to enter application with.
    /// Stores user info as admin for current working session.
    /// Navigates to MainMenuV, if username/passwird match the database. 
    /// starts logging user actions to UserLog.
    /// </remarks>
    public partial class LoginV : Window
    {
        private AdministratorService _adminSrv;
        private List<AdministratorBO> _admins;
        private Logger _userlog;

        public LoginV()
        {
            InitializeComponent();
            _adminSrv = new AdministratorService();
            _admins = _adminSrv.getAllFromTable();
            _userlog = new Logger(true, "Login attempt: ");
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {


            //Below commented code checks if inserted username/pw is according to DB data
            string hPassword = EncryptPassword.computeHash(this.passwordBox.Password, new MD5CryptoServiceProvider());

            if (txtUserName.Text == "" || String.IsNullOrWhiteSpace(passwordBox.Password)){

                MessageBox.Show("Sisesta kasutajanimi ja parool");

            }
            else
            {


                foreach (AdministratorBO _admin in _admins)
                {
                    if (_admin.UserName == this.txtUserName.Text && _admin.Password == hPassword)
                    {
                        MainMenuV mainMenu = new MainMenuV(_admin.Name);
                        mainMenu.Show();
                        this.Close();
                        _userlog.Trace(string.Format("--------------- successful at {0}. Admin: {1} ------------------------------", DateTime.Now, _admin.Name));
                        return;
                    }
                }

                MessageBoxResult messageResult = MessageBox.Show("Vale kasutajanimi või parool", "", MessageBoxButton.OK);

                if (messageResult == MessageBoxResult.OK)
                {
                    passwordBox.Clear();
                }
                _userlog.Trace(string.Format("*unsuccessful*"));

            }
        }

    }
}






/*! \mainpage Codezilla thanks you for trying our ITK C# group project
 *
 * \section intro_sec Introduction
 * Our app is a simple booking system allowing user to maintain customers and their booking in a small conference centre.
 * App is used by conference centre administrator/receptionist (admin).
 *
 * From admin point of view app allows to:
 * - add new bookings
 * - modify existing bookings
 * - confirm or delete bookings
 * - view existing booking details
 * - add/ modify customers and their contacts
 * - modify rooms configuration and availability
 * - see list of all confirmed anf unconfirmed bookings and filter output by multiple criteria
 * - generate report on centre activities by customer, by room or full report by date range 
 * 
 *
 * \section install_sec Installation
 *  to enjoy the full experience of our app prototype please follow the below steps.
 *
 * \subsection step1 Step 1: Create local database
 *  Execute the DBScript from KonverentsProject\Konverentsikeskus\ folder
 * \subsection step2 Step 2: Open the project in Visual Studio
 *  1. Set project TestRakendus as "StartUp project"
 *  2. Start solution. It will fill the database with dummy entries, for easier functionality testing
 *  3. Set project Keskus as "StartUp project".
 *  4. Start solution. 
 *  5. Enter username: test and password: test
 *  6. Enjoy!
 *  
 * 
 */
