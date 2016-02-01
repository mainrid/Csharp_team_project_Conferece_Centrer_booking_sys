using Keskus.BLL.Booking;
using Keskus.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using System.Reflection;
using Keskus.BLL.Security;

namespace Keskus.BLL.Report
{
    /// <remarks>
    /// Class contains services for Report business object
    /// </remarks>
    public class ReportService
    {
        //application logic events are logged into log.txt file
        private Logger _log;

        // user action events are logged into userlog.txt file
        private Logger _userlog;


        public ReportService()
        {
            _log = new Logger("ReportService()");
            _log.Trace("Init");
            _userlog = new Logger(true, "Reports were accessed");
        }
        /// <remarks>
        /// generates report by Customer and date range from DB
        /// </remarks>
        /// <param name="report"></param>
        /// <returns></returns>
        public List<BookingBO> getReport(CustomerReportBO report)
        {
            _log.Trace("in getReport(Customer)");

           
            using (var db = new Keskus_baasEntities())
            {
                _userlog.Trace(string.Format("Report was requested for customer ID: {0},  {1}, for period {2} - {3}", report.Customer.CustomerID, report.Customer.CompanyName, report.StartDate, report.EndDate));
                List<BookingBO> bookings = db.Bookings
                    .Where(x => (x.CustomerID == report.Customer.CustomerID &&
                                 (x.Date.CompareTo(report.EndDate) <= 0) &&
                                 (x.Date.CompareTo(report.StartDate) >= 0)))
                    //add Status.Arctived == true!
                    .ToList()
                    .Select(x => new BookingBO(x))
                    .ToList();
                return bookings;
                
            }
        }

        /// <remarks>
        /// generates report by Room and date range from DB
        /// </remarks>
        /// <param name="report"></param>
        /// <returns></returns>
        public List<BookingBO> getReport(RoomReportBO report)
        {
            _log.Trace("in getReport(Room)");


            using (var db = new Keskus_baasEntities())
            {
                _userlog.Trace(string.Format("Report was requested for room ID: {0},  {1}, for period {2} - {3}", report.Room.RoomID, report.Room.Name, report.StartDate, report.EndDate));
                List<BookingBO> bookings = db.Bookings
                    .Where(x => (x.RoomID == report.Room.RoomID &&
                                 (x.Date.CompareTo(report.EndDate) <= 0) &&
                                 (x.Date.CompareTo(report.StartDate) >= 0)))
                                 //add Status.Arctived == true!
                    .ToList()
                    .Select(x => new BookingBO(x))
                    .ToList();
                return bookings;
            }
        }

        /// <remarks>
        /// generates  full report by date range from DB
        /// </remarks>
        /// <param name="report"></param>
        /// <returns></returns>
        public List<BookingBO> getReport(ReportBO report)
        {
            _log.Trace("in getReport(Full Report)");

            
            using (var db = new Keskus_baasEntities())
            {
                _userlog.Trace(string.Format("Full Report was requested for period: {0} - {1}", report.StartDate, report.EndDate));
                List<BookingBO> bookings = db.Bookings
                    .Where(x => ((x.Date.CompareTo(report.EndDate) <= 0) &&
                                 (x.Date.CompareTo(report.StartDate) >= 0)))
                    //add Status.Arctived == true!
                    .ToList()
                    .Select(x => new BookingBO(x))
                    .ToList();
                return bookings;
            }
        }

        /// <remarks>
        /// exports report to excel file
        /// solution http://stackoverflow.com/questions/11811143/export-datatable-to-excel-with-open-xml-sdk-in-c-sharp
        /// </remarks>
        /// <param name="ds"></param>
        /// <param name="destination"></param>
        public void ExportToExcel(DataSet ds, string destination)

        {
            _log.Trace("in ExportToExcel()");
            _userlog.Trace("Report export to file is requested");

            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                foreach (System.Data.DataTable table in ds.Tables)
                {
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = "Report" };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }


                    sheetData.AppendChild(headerRow);

                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }

                }
            }
            _userlog.Trace("Successful export to file.");
        }

        /// <remarks>
        /// converts List to DataTable for exporting purposes
        /// solution http://stackoverflow.com/questions/18100783/how-to-convert-a-list-into-data-table
        /// </remarks>
        /// <typeparam name="BookingBO"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public DataTable ToDataTable<ExportBO>(List<ExportBO> items)
        {
            _log.Trace("in ToDataTable()");
            DataTable dataTable = new DataTable(typeof(ExportBO).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(ExportBO).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
               
               
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                
            }
            foreach (ExportBO item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    
                    //inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }


}

