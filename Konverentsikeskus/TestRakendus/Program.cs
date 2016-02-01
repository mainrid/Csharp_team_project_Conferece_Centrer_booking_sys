using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Keskus.DAL;
using Keskus.BLL.Room;
using Keskus.BLL.Booking;
using Keskus.BLL.Administrator;
using Keskus.BLL.ContactType;
using Keskus.BLL.Customer;
using Keskus.BLL.Contact;
using System.Globalization;

namespace TestRakendus
    
{
    class Program
    {


        static void Main(string[] args)
        {
            //TODO: Verify that all needed Data exists in Database
            //This method ensures that data exists, if not then full fill DataBase.


            //Setup step (includes verification)
            // Deletes old data from DB and full fill readable data to DB tables 
            cleanDB();
            fullFillDB();

            //TEST EXECUTION
            testContactTypesDataCorrectness();

            //Exit
            Console.WriteLine("Testimine edukalt sooritatud!");
            
        }



        private static void cleanDB()
        {
            //Please do not change the order. (Primary/Foreign key in DB are affected)
            emptyTableBooking();
            emptyTableContact();
            emptyTableContactType();
            emptyTableCustomer();

            emptyTableAdministrator();
            emptyTableRoom();
        }

        private static void emptyTableRoom()
        {
            RoomService roomSrv = new RoomService();
            roomSrv.emptyTable();
            List<RoomBO> bookings = roomSrv.getAllFromTable();
            Assert.AreEqual(0, bookings.Count(), string.Format("'Room' Table should be empty."));
        }

        private static void emptyTableBooking()
        {
            BookingService bookingSrv = new BookingService();
            bookingSrv.emptyTable();
            List<BookingBO> bookings = bookingSrv.getAllFromTable();
            Assert.AreEqual(0, bookings.Count(), string.Format("'Booking' Table should be empty."));
        }

        private static void emptyTableContact()
        {
            ContactService contactSrv = new ContactService();
            contactSrv.emptyTable();
            List<ContactBO> contacts = contactSrv.getAllFromTable();
            Assert.AreEqual(0, contacts.Count(), string.Format("'Contact' Table should be empty."));
        }
        private static void emptyTableContactType()
        {
            ContactTypeService contactTypeSrv = new ContactTypeService();
            contactTypeSrv.emptyTable();
            List<ContactTypeBO> contactTypes = contactTypeSrv.getAllFromTable();
            Assert.AreEqual(0, contactTypes.Count(), string.Format("'ContactType' Table should be empty."));
        }

        private static void emptyTableCustomer()
        {
            CustomerService customerSrv = new CustomerService();
            customerSrv.emptyTable();
            List<CustomerBO> customers = customerSrv.getAllFromTable();
            Assert.AreEqual(0, customers.Count(), string.Format("'Customer' Table should be empty."));
        }

        private static void emptyTableAdministrator()
        {
            AdministratorService adminSrv = new AdministratorService();
            adminSrv.emptyTable();
            List<AdministratorBO> admins = adminSrv.getAllFromTable();
            Assert.AreEqual(0, admins.Count(), string.Format("'Administrator' Table should be empty."));
        }



        private static void fullFillDB()
        {
            //Please do not change the order. (Primary/Foreign key in DB are affected)
            fullFillContactTypeTable();
            fullFillCustomerTable();
            fullFillContactTable();

            fullFillTableAdministrator();
            fullFillTableRoom();
            fullFillTableBooking();
            

        }

        private static void fullFillTableRoom()
        {
            RoomService roomSrv = new RoomService();
            roomSrv.addNew("Saturn", 130, true);//TODO: take first from list
            roomSrv.addNew("Marss", 30, false);
            roomSrv.addNew("Merkuur", 55, true);
            List<RoomBO> rooms = roomSrv.getAllFromTable();
            Assert.AreNotEqual(0, rooms.Count(), string.Format("There should be values in 'Rooms' Table."));
        }

        private static void fullFillTableBooking()
        {
            RoomService roomSrv = new RoomService();
            List<RoomBO> rooms = roomSrv.getAllFromTable();
            Assert.AreNotEqual(0, rooms.Count(), string.Format("There should be values in 'Rooms' Table."));
            int existingRoomID = rooms.ElementAt(0).RoomID;

            CustomerService customerSrv = new CustomerService();
            List<CustomerBO> customers = customerSrv.getAllFromTable();
            Assert.AreNotEqual(0, customers.Count(), string.Format("There should be values in 'Customer' Table."));
            int existingCustomerID = customers.ElementAt(0).CustomerID;

            AdministratorService adminSrv = new AdministratorService();
            List<AdministratorBO> admins = adminSrv.getAllFromTable();
            Assert.AreNotEqual(0, admins.Count(), string.Format("There should be values in 'Administrator' Table."));
            int existingAdminID = admins.ElementAt(0).AdminID;

            //Verify that booking cannot be created before Customer (CustomerID foreign key)
            //Verify that booking cannot be created before Room (RoomID foreign key)
            //Verify that booking cannot be created before Administrator (AdminID foreign key)
            BookingService bookingSrv = new BookingService();
            bookingSrv.addNew(DateTime.Now.AddDays(1), existingRoomID, existingCustomerID, 25, DateTime.Now, existingAdminID, "no smoking");// take first from list
            bookingSrv.addNew(DateTime.Now.AddDays(1), existingRoomID, existingCustomerID, 40, DateTime.Now, existingAdminID, "no pets allowed");
            bookingSrv.addNew(DateTime.Now.AddDays(10), existingRoomID, existingCustomerID, 77, DateTime.Now, existingAdminID, "reconfirm by phone");
            bookingSrv.addNew(DateTime.Now.AddDays(17), existingRoomID, existingCustomerID, 55, DateTime.Now, existingAdminID, "reconfirm by phone tomorrow");
            bookingSrv.addNew(DateTime.Now.AddDays(-5), existingRoomID, existingCustomerID, 10, DateTime.Now.AddDays(-8), existingAdminID, "catering ");
            bookingSrv.addNew(DateTime.Now.AddDays(-2), existingRoomID, existingCustomerID, 100, DateTime.Now.AddDays(-5), existingAdminID, "no catering");
            List<BookingBO> bookings = bookingSrv.getAllFromTable();
            Assert.AreNotEqual(0, bookings.Count(), string.Format("There should be values in 'Booking' Table."));
        }

        private static void fullFillContactTable()
        {
            ContactTypeService contactTypeSrv = new ContactTypeService();
            List<ContactTypeBO> contactTypes = contactTypeSrv.getAllFromTable();
            Assert.AreNotEqual(0, contactTypes.Count(), string.Format("There should be values in 'ContactType' Table."));
            int existingContactTypeID = contactTypes.ElementAt(0).ContactTypeID;
            
            CustomerService customerSrv = new CustomerService();
            List<CustomerBO> customers = customerSrv.getAllFromTable();
            Assert.AreNotEqual(0, customers.Count(), string.Format("There should be values in 'Customer' Table."));
            int existingCustomerID = customers.ElementAt(0).CustomerID;

            //TODO: Verify that contact cannot be created before Customer (CustomerID foreign key)
            //TODO: Verify that contact cannot be created before ContactType (ContactTypeID foreign key)
            ContactService contactSrv = new ContactService();
            contactSrv.addNew(existingCustomerID, existingContactTypeID, "val_skype", DateTime.Now);//TODO: take first from list
            contactSrv.addNew(existingCustomerID, existingContactTypeID, "val_mail", DateTime.Now);
            contactSrv.addNew(existingCustomerID, existingContactTypeID, "val_phone", DateTime.Now);
            List<ContactBO> contacts = contactSrv.getAllFromTable();
            Assert.AreNotEqual(0, contacts.Count(), string.Format("There should be values in 'Contact' Table."));
        }

        private static void fullFillContactTypeTable()
        {
            ContactTypeService contactTypeSrv = new ContactTypeService();
            contactTypeSrv.addNew("skype", false);//TODO: take first from list
            contactTypeSrv.addNew("phone", true);
            contactTypeSrv.addNew("email", false);
            List<ContactTypeBO> contactTypes = contactTypeSrv.getAllFromTable();
            Assert.AreNotEqual(0, contactTypes.Count(), string.Format("There should be values in 'ContactType' Table."));
        }

        private static void fullFillCustomerTable()
        {
            CustomerService customerSrv = new CustomerService();
            customerSrv.addNew("ABC OÜ", "Anne");
            customerSrv.addNew("XYZ AS", "Daria");
            List<CustomerBO> customers = customerSrv.getAllFromTable();
            Assert.AreNotEqual(0, customers.Count(), string.Format("There should be values in 'Customer' Table."));
        }

        private static void fullFillTableAdministrator()
        {
            AdministratorService adminSrv = new AdministratorService();
            adminSrv.addNew("Alisa", "al_usr", "parol", true);
            adminSrv.addNew("Anna", "an_usr", "parol", true);
            adminSrv.addNew("Sergei", "se_usr", "parol", true);
            adminSrv.addNew("Test Admin", "test", "test", true);
            List<AdministratorBO> admins = adminSrv.getAllFromTable();
            Assert.AreNotEqual(0, admins.Count(), string.Format("There should be values in 'Administrator' Table.")); 

        }


        /// <summary>
        /// This method tests ContactType Table
        /// We test that minimum expected rows are exist
        /// and their values are correct
        /// </summary>
        public static void testContactTypesDataCorrectness()
        {

            ContactTypeService contactTypeSrv = new ContactTypeService();
            List<ContactTypeBO> contactTypes = contactTypeSrv.getAllFromTable();

            String expValueName1 = "skype";
            bool isVerifiedValue1 = false;

            String expValueName2 = "phone";
            bool isVerifiedValue2 = false;

            String expValueName3 = "email";
            bool isVerifiedValue3 = false;

            foreach (var contactType in contactTypes)
            {
                if (expValueName1 == contactType.Name)
                {
                    isVerifiedValue1 = true;
                }
                if (expValueName2 == contactType.Name)
                {
                    isVerifiedValue2 = true;
                }
                if (expValueName3 == contactType.Name)
                {
                    isVerifiedValue3 = true;
                }
                //debug info
                //string valjund = string.Format("{0} {1} ", contactType.ContactTypeID, contactType.Name);
                //Console.WriteLine(valjund);
            }

            Assert.AreEqual(true, isVerifiedValue1, string.Format("'{0}' Verification failed", expValueName1));
            Assert.AreEqual(true, isVerifiedValue2, string.Format("'{0}' Verification failed", expValueName2));
            Assert.AreEqual(true, isVerifiedValue3, string.Format("'{0}' Verification failed", expValueName3));
        }


    }
}

