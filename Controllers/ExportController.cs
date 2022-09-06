using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using WepApiWithToken.Authentication;
using WepApiWithToken.Interface;

namespace WepApiWithToken.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExportController : Controller
    {
        private readonly IStudent _studRep;
        private readonly IMaintenance _maintenanceRep;
        private readonly ICleaning _cleaningRep;
        private readonly IServiceManager _servRep;

        public ExportController(IStudent studRep, IMaintenance maintenanceRep, ICleaning cleaningRep, IServiceManager servRep)
        {
            _studRep = studRep;
            _maintenanceRep = maintenanceRep;
            _cleaningRep = cleaningRep;
            _servRep = servRep;
        }


        [HttpGet("Student/Excel"),Authorize(Roles ="Admin")]
        public IActionResult CreateExcel()
        {
            // This methood exports data in Excel
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9]
            {
                new DataColumn("Name"),
                new DataColumn("Email"),
                new DataColumn("Last Name"),
                new DataColumn("Phone Number"),
                new DataColumn("Age"),
                new DataColumn("Date of Birth"),
                new DataColumn("Gender"),
                new DataColumn("Room"),
                new DataColumn("Floor"),
            });

            var students = _studRep.GetAll();

            foreach (var student in students)
            {
                dt.Rows.Add(student.Name, student.Email, student.LastName,student.PhoneNumber, student.Age, student.BirthDate, student.GenderId, student.RoomId, student.RoomId);
            }

            

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.ColumnWidth = 18.71;
                wb.Worksheets.Add(dt);
                
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Student List.xlsx");
                }
            }
        }

        [HttpGet("Student/Pdf"),Authorize(Roles = "Admin")]
        public IActionResult StudentPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("StudentDetails_Pdf" /*+ dTime.ToString("yyyyMMdd") + "-" +*/ +".pdf");
            Document doc = new Document();
            doc.SetMargins(0, 0, 0, 0);
            PdfPTable tableLayout = new PdfPTable(9);
            doc.SetMargins(10, 10, 10, 0);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontInvoice = new Font(bf, 20, Font.NORMAL);
            Paragraph paragraph = new Paragraph("Students Detail List", fontInvoice);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);
            Paragraph p3 = new Paragraph();
            p3.SpacingAfter = 8;
            doc.Add(p3);
            doc.Add(Add_Content_To_PDF(tableLayout,"Student"));
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        [HttpGet("Maintenance/Excel"),Authorize(Roles = "Admin,Service Management")]
        public IActionResult MaintenanceExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[8]
            {
                new DataColumn("Maitenenance Id"),
                new DataColumn("Name"),
                new DataColumn("Contact"),
                new DataColumn("Status"),
                new DataColumn("Maintenance Type"),
                new DataColumn("Room Number"),
                new DataColumn("Description"),               
                new DataColumn("Maintenance Date"),
            });
            
            
            var maintenanceList = _maintenanceRep.GetAll();
               

            foreach (var model in maintenanceList)
            {
                dt.Rows.Add(model.MaintenanceId,model.Student.Name,model.Student.PhoneNumber,model.Status.Name, model.MaintenanceType.Name, model.Student.Room.RoomNum,model.Description,model.MaintenanceDate);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Maintenance List.xlsx");
                }
            }
        }

        [HttpGet("Cleaning/Excel"), Authorize(Roles = "Admin,Service Management")]
        public IActionResult CleaningExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("Cleaning Id"),
                new DataColumn("Name"),
                new DataColumn("Contact"),
                new DataColumn("Cleaning Type"),
                new DataColumn("Room Number"),
                new DataColumn("Description"),
                new DataColumn("Maintenance Date"),
            });


            var cleanList = _cleaningRep.GetAll();


            foreach (var model in cleanList)
            {
                dt.Rows.Add(model.CleaningId, model.Student.Name, model.Student.PhoneNumber, model.CleaningType.Name, model.Student.Room.RoomNum, model.Description, model.CleaningDate);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Cleaning List.xlsx");
                }
            }
        }

        [HttpGet("Maintenance/Pdf"), Authorize(Roles = "Admin,Service Management")]
        public IActionResult MaintenancePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("RAS_MaintenanceList"/* + dTime.ToString("yyyyMMdd") + "-" +*/ +".pdf");
            Document doc = new Document();
            doc.SetMargins(0, 0, 0, 0);
            PdfPTable tableLayout = new PdfPTable(8);
            doc.SetMargins(10, 10, 10, 0);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontInvoice = new Font(bf, 20, Font.NORMAL);
            Paragraph paragraph = new Paragraph("Maintenance List", fontInvoice);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);
            Paragraph p3 = new Paragraph();
            p3.SpacingAfter = 8;
            doc.Add(p3);
            doc.Add(Add_Content_To_PDF(tableLayout, "Maintenance"));
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        [HttpGet("Cleaning/Pdf"), Authorize(Roles = "Admin,Service Management")]
        public IActionResult CleaningPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("RAS_CleaningList" /*+ dTime.ToString("yyyyMMdd") + "-" +*/ +".pdf");
            Document doc = new Document();
            doc.SetMargins(0, 0, 0, 0);
            PdfPTable tableLayout = new PdfPTable(7);
            doc.SetMargins(10, 10, 10, 0);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontInvoice = new Font(bf, 20, Font.NORMAL);
            Paragraph paragraph = new Paragraph("Cleaning List", fontInvoice);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);
            Paragraph p3 = new Paragraph();
            p3.SpacingAfter = 8;
            doc.Add(p3);
            doc.Add(Add_Content_To_PDF(tableLayout, "Cleaning"));
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout,string model)
        {
            float[] headers;
            if (model == "Student")
            {
                float[] Studentheaders = { 26, 27, 29, 26,34, 29, 34, 34,34,};
                headers = Studentheaders;
            }
            else if(model == "Maintenance")
            {
                float[] Maintenanceheaders = { 26, 27, 29, 26, 34, 29, 34, 34 };
                headers = Maintenanceheaders;
            }
            else
            {
                float[]Cleaningheaders = { 26, 27, 29, 26, 34, 29, 34};
                headers = Cleaningheaders;
            }
          /*  headers = { 26, 27, 29, 26, 34, 29, 34, 34,34 };*/ //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            var count = 1;

            //Add header  
      
             if(model == "Student")
            {
                AddCellToHeader(tableLayout, "Name");
                AddCellToHeader(tableLayout, "Last Name");
                AddCellToHeader(tableLayout, "Email");
                AddCellToHeader(tableLayout, "PhoneNumber");
                AddCellToHeader(tableLayout, "Age");
                AddCellToHeader(tableLayout, "BirthDate");
                AddCellToHeader(tableLayout, "Gender");
                AddCellToHeader(tableLayout, "Room");
                AddCellToHeader(tableLayout, "Floor ");

                foreach (var stud in _studRep.GetAll())
                {
                    if (count >= 1)
                    {
                        //Add body  
                        //AddCellToBody(tableLayout, stud.Id.ToString(), count);
                        AddCellToBody(tableLayout, stud.Email.ToString(), count);
                        AddCellToBody(tableLayout, stud.PhoneNumber.ToString(), count);
                        AddCellToBody(tableLayout, stud.Name.ToString(), count);
                        AddCellToBody(tableLayout, stud.LastName.ToString(), count);
                        AddCellToBody(tableLayout, stud.Age.ToString(), count);
                        AddCellToBody(tableLayout, stud.BirthDate.ToString(), count);
                        AddCellToBody(tableLayout, stud.GenderId.ToString(), count);
                        AddCellToBody(tableLayout, stud.RoomId.ToString(), count);
                        AddCellToBody(tableLayout, stud.RoomId.ToString(), count);

                        count++;
                    }
                }
            }
            else if(model == "Maintenance")
            {
                AddCellToHeader(tableLayout,"Id");
                AddCellToHeader(tableLayout,"Name"); 
                AddCellToHeader(tableLayout,"Contact");       
                AddCellToHeader(tableLayout,"Status");
                AddCellToHeader(tableLayout,"Maintenance Type");
                AddCellToHeader(tableLayout, "Room Number");
                AddCellToHeader(tableLayout,"Description");
                AddCellToHeader(tableLayout, "Maintenance Date");

                foreach (var maintenance in _maintenanceRep.GetAll())
                {
                    if (count >= 1)
                    {
                        //Add body  
                        //AddCellToBody(tableLayout, stud.Id.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.MaintenanceId.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.Student.Name.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.Student.PhoneNumber.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.Status.Name.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.MaintenanceType.Name.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.Student.Room.RoomNum.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.Description.ToString(), count);
                        AddCellToBody(tableLayout, maintenance.MaintenanceDate.ToString(), count);
                  

                        count++;
                    }
                }
            }
            else
            {
                AddCellToHeader(tableLayout, "Id");
                AddCellToHeader(tableLayout, "Name");
                AddCellToHeader(tableLayout, "Contact");
                AddCellToHeader(tableLayout, "Cleaning Type");
                AddCellToHeader(tableLayout, "Room Number");
                AddCellToHeader(tableLayout, "Description");
                AddCellToHeader(tableLayout, "Maintenance Date");
          

                foreach (var stud in _cleaningRep.GetAll())
                {
                    if (count >= 1)
                    {
                        //Add body  
                       
                        AddCellToBody(tableLayout, stud.CleaningId.ToString(), count);
                        AddCellToBody(tableLayout, stud.Student.Name.ToString(), count);
                        AddCellToBody(tableLayout, stud.Student.PhoneNumber.ToString(), count);
                        AddCellToBody(tableLayout, stud.CleaningType.Name.ToString(), count);
                        AddCellToBody(tableLayout, stud.Student.Room.RoomNum.ToString(), count);
                        AddCellToBody(tableLayout, stud.Description.ToString(), count);
                        AddCellToBody(tableLayout, stud.CleaningDate.ToString(), count);
                      

                        count++;
                    }
                }
            }
       
            return tableLayout;
        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8,
                BackgroundColor = new BaseColor(255, 255, 255)
            });
        }
        private static void AddCellToBody(PdfPTable tableLayout, string cellText, int count)
        {
            if (count % 2 == 0)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new BaseColor(255, 255, 255)
                });
            }
            else
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211)
                });
            }
        }

        //[HttpGet("Cleaning/Excel")]
        //public IActionResult CleaningExcel()
        //{

        //}

    }
}
