using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using RDLCDesign.Service;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Sabri_Logistics_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration Configuration;

        [Obsolete]
        public ReportController(IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = config;
        }

        //[Route("GetUser")]
        //[HttpGet]
        //[Obsolete]
        //public ActionResult GetUserReport(decimal UserId)
        //{
        //    string mintype = "";
        //    int extension = 1;
        //    string webRootPath = _hostingEnvironment.WebRootPath;
        //    string ReportPath = "Reports\\Report1.rdlc";
        //    string ReportPathSave = "ReportPdf\\testeReport1.pdf";
        //    //var path = $"{_webHostEnviroment.WebRootPath}\\Reports\\Report1.rdlc";
        //    string path = Path.Combine(webRootPath, ReportPath);
        //    string pathSave = Path.Combine(webRootPath, ReportPathSave);

        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("UserID", UserId.ToString());
        //    LocalReport localReport = new LocalReport(path);

        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    ProductFunction fmr = new ProductFunction();
        //    dt = fmr.GetData("EXEC GetUser " + UserId + "");
        //    localReport.AddDataSource("DataSet1", dt);
        //    dt = Connection.ExecuteDataTable("SELECT * FROM RoleMaster");
        //    localReport.AddDataSource("DataSet2", dt);

        //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
        //    byte[] file = result.MainStream;
        //    System.IO.Stream stream = new System.IO.MemoryStream(file);

        //    var fileStream = new FileStream(pathSave, FileMode.Create, FileAccess.Write);
        //    stream.CopyTo(fileStream);
        //    fileStream.Dispose();

        //    var msg = new
        //    {
        //        FileUrl = "http://sabriapi.bancplus.in/ReportPdf/testeReport1.pdf"
        //    };
        //    return Ok(msg);

        //}

        //[Route("GetUser1")]
        //[HttpGet]
        //[Obsolete]
        //public ActionResult GetUserReport1(decimal UserId)
        //{
        //    string mintype = "";
        //    int extension = 1;
        //    string webRootPath = _hostingEnvironment.WebRootPath;
        //    string ReportPath = "Reports\\Report1.rdlc";
        //    string ReportPathSave = "ReportPdf\\testeReport1.pdf";
        //    //var path = $"{_webHostEnviroment.WebRootPath}\\Reports\\Report1.rdlc";
        //    string path = Path.Combine(webRootPath, ReportPath);
        //    string pathSave = Path.Combine(webRootPath, ReportPathSave);

        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("UserID", UserId.ToString());
        //    LocalReport localReport = new LocalReport(path);

        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    ProductFunction fmr = new ProductFunction();
        //    dt = fmr.GetData("EXEC GetUser " + UserId + "");
        //    localReport.AddDataSource("DataSet1", dt);
        //    dt = Connection.ExecuteDataTable("SELECT * FROM RoleMaster");
        //    localReport.AddDataSource("DataSet2", dt);

        //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
        //    byte[] file = result.MainStream;
        //    System.IO.Stream stream = new System.IO.MemoryStream(file);
        //    return File(stream, "application/pdf", "testeReport.pdf");

        //}

        [Route("GetVoucherBook")]
        [HttpGet]
        [Obsolete]
        public ActionResult GetVoucherBookReport(DateTime FrmDt, DateTime ToDt, string VRType)
        {
            string mintype = "";
            int extension = 1;
            string ReportPath = "", ReportPathSave = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            //string ReportPath = "Reports\\CashBook.rdlc";
            //string ReportPathSub1 = "Reports\\CashBookReceipt.rdlc";
            //string ReportPathSub2 = "Reports\\CashBookPayment.rdlc";
            if (VRType == "C")
            {
                ReportPath = "Reports\\CashBook.rdlc";
                ReportPathSave = "ReportPdf\\CashBook.pdf";
            }
            else if (VRType == "J")
            {
                ReportPath = "Reports\\JournalBook.rdlc";
                ReportPathSave = "ReportPdf\\JournalBook.pdf";
            }
            //var path = $"{_webHostEnviroment.WebRootPath}\\Reports\\Report1.rdlc";
            string path = Path.Combine(webRootPath, ReportPath);
            //string pathSub1 = Path.Combine(webRootPath, ReportPathSub1);
            //string pathSub2 = Path.Combine(webRootPath, ReportPathSub2);
            string pathSave = Path.Combine(webRootPath, ReportPathSave);
            LocalReport localReport = new LocalReport(path);
            //LocalReport localReportSub1 = new LocalReport(pathSub1);
            //LocalReport localReportSub2 = new LocalReport(pathSub2);

            System.Data.DataTable dt = new System.Data.DataTable();
            //System.Data.DataSet ds = new System.Data.DataSet();
            //ds = GetCashBookList(FrmDt, ToDt);
            //localReport.AddDataSource("DataSet1", ds.Tables[0]);
            //localReportSub1.AddDataSource("DataSet1", ds.Tables[1]);
            //localReportSub2.AddDataSource("DataSet1", ds.Tables[2]);
            ProductFunction fmr = new ProductFunction();
            //dt = fmr.GetData("EXEC pr_CashBook '" + FrmDt + "', '" + ToDt + "'");
            //localReport.AddDataSource("DataSet1", dt);
            //dt = fmr.GetData("EXEC pr_CashBook_RecPay '" + FrmDt + "', '" + ToDt + "'");
            //localReportSub1.AddDataSource("DataSet1", dt);
            //localReport.AddDataSource("DataSet2", dt);

            if (VRType == "C")
            {
                dt = fmr.GetData("EXEC pr_CashBook_RecPay '" + FrmDt + "', '" + ToDt + "'");
                localReport.AddDataSource("DataSet1", dt);
            }
            else if (VRType == "J") {
                dt = fmr.GetData("EXEC pr_JournalBook '" + FrmDt + "', '" + ToDt + "'");
                localReport.AddDataSource("DataSet1", dt);
            }
                //dt = fmr.GetData("EXEC pr_CashBook_RecPay '" + FrmDt + "', '" + ToDt + "'");
                //localReportSub2.AddDataSource("DataSet1", dt);
                //dt = Connection.ExecuteDataTable("SELECT * FROM RoleMaster");
                //localReport.AddDataSource("DataSet2", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("FrmDt", FrmDt.ToString());
            parameters.Add("ToDt", ToDt.ToString());

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
            byte[] file = result.MainStream;
            System.IO.Stream stream = new System.IO.MemoryStream(file);
            return File(stream, "application/pdf",  (VRType == "C" ? "CashBook.pdf" : "JournalBook.pdf"));
        }

        [Route("GetTrialBalance")]
        [HttpGet]
        [Obsolete]
        public ActionResult GetTrialBalanceReport(DateTime FrmDt, DateTime ToDt, decimal GrpID, string Mode)
        {
            string mintype = "";
            int extension = 1;
            string ReportPath = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            System.Data.DataTable dt = new System.Data.DataTable();
            ProductFunction fmr = new ProductFunction();
            if (Mode == "D")
            {
                ReportPath = "Reports\\TrialBalance.rdlc";
                dt = fmr.GetData("EXEC pr_TrialBalance '" + FrmDt + "', '" + ToDt + "', " + GrpID + "");
            }
            else if (Mode == "S")
            {
                ReportPath = "Reports\\TrialBalanceSummary.rdlc";
                dt = fmr.GetData("EXEC pr_TrialBalanceSummary '" + FrmDt + "', '" + ToDt + "', " + GrpID + "");
            }
            string path = Path.Combine(webRootPath, ReportPath);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("FrmDt", FrmDt.ToString());
            parameters.Add("ToDt", ToDt.ToString());
            parameters.Add("GrpID", GrpID.ToString());

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
            byte[] file = result.MainStream;
            System.IO.Stream stream = new System.IO.MemoryStream(file);
            return File(stream, "application/pdf", (Mode == "D" ? "TrialBalance.pdf" : "TrialBalanceSummary.pdf"));
        }

        [Route("GetProfitAndLoss")]
        [HttpGet]
        [Obsolete]
        public ActionResult GetProfitAndLossReport(DateTime AsOnDt)
        {
            string mintype = "";
            int extension = 1;
            string ReportPath = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            System.Data.DataTable dt = new System.Data.DataTable();
            ProductFunction fmr = new ProductFunction();
            ReportPath = "Reports\\ProfitAndLoss.rdlc";
            dt = fmr.GetData("EXEC pr_ProfitAndLoss '" + AsOnDt + "'");
            string path = Path.Combine(webRootPath, ReportPath);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("AsOnDate", AsOnDt.ToString());

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
            byte[] file = result.MainStream;
            System.IO.Stream stream = new System.IO.MemoryStream(file);
            return File(stream, "application/pdf", "ProfitAndLoss.pdf");
        }

        [Route("GetAcStatement")]
        [HttpGet]
        [Obsolete]
        public ActionResult GetAcStatementReport(DateTime FrmDt, DateTime ToDt, decimal AcID)
        {
            string mintype = "";
            int extension = 1;
            string ReportPath = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            System.Data.DataTable dt = new System.Data.DataTable();
            ProductFunction fmr = new ProductFunction();
            ReportPath = "Reports\\AcStatement.rdlc";
            dt = fmr.GetData("EXEC pr_AcStatement '" + FrmDt + "', '" + ToDt + "', " + AcID + "");
            string path = Path.Combine(webRootPath, ReportPath);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("FrmDt", FrmDt.ToString());
            parameters.Add("ToDt", ToDt.ToString());
            parameters.Add("AcId", AcID.ToString());

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
            byte[] file = result.MainStream;
            System.IO.Stream stream = new System.IO.MemoryStream(file);
            return File(stream, "application/pdf", "AcStatement.pdf");
        }

        [Route("GetDayBook")]
        [HttpGet]
        [Obsolete]
        public ActionResult GetDayBookReport(DateTime FrmDt, DateTime ToDt)
        {
            string mintype = "";
            int extension = 1;
            string ReportPath = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            System.Data.DataTable dt = new System.Data.DataTable();
            ProductFunction fmr = new ProductFunction();
            ReportPath = "Reports\\DayBook.rdlc";
            dt = fmr.GetData("EXEC pr_DayBook '" + FrmDt + "', '" + ToDt + "'");
            string path = Path.Combine(webRootPath, ReportPath);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("FrmDt", FrmDt.ToString());
            parameters.Add("ToDt", ToDt.ToString());

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mintype);
            byte[] file = result.MainStream;
            System.IO.Stream stream = new System.IO.MemoryStream(file);
            return File(stream, "application/pdf", "DayBook.pdf");
        }




        //public System.Data.DataSet GetCashBookList(DateTime FrmDt, DateTime ToDt)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    System.Data.DataSet dtArray = new System.Data.DataSet();
        //    ProductFunction fmr = new ProductFunction();
        //    dt = fmr.GetData("EXEC pr_CashBook '" + FrmDt + "', '" + ToDt + "'");
        //    //dtArray.Tables[0] = new DataTable("DataTable0");
        //    //dtArray[1] = new DataTable("DataTable1");
        //    //dtArray.DataSetName = new System.Data.DataTable("CashBook");
        //    dtArray.Tables.Add(dt);
        //    dt = fmr.GetData("EXEC pr_CashBook_RecPay '" + FrmDt + "', '" + ToDt + "', 'R'");
        //    dtArray.Tables.Add(dt);
        //    dt = fmr.GetData("EXEC pr_CashBook_RecPay '" + FrmDt + "', '" + ToDt + "', 'P'");
        //    dtArray.Tables.Add(dt);

        //    //dtArray[0].Columns.Add("EmpId", typeof(Int32));
        //    //dtArray[0].Columns.Add("EmpName");
        //    //dtArray[0].Columns.Add("EmpDepart");
        //    //dtArray[0].Columns.Add("EmpDesignation");

        //    //dtArray[1].Columns.Add("EmpContId", typeof(Int32));
        //    //dtArray[1].Columns.Add("EmpId", typeof(Int32));
        //    //dtArray[1].Columns.Add("EmpContMobile");
        //    //dtArray[1].Columns.Add("EmpContEmail");
        //    //dtArray[1].Columns.Add("EmpContAddress");
        //    //dtArray[1].Columns.Add("EmpContCity");

        //    //DataRow row;
        //    //DataRow rowTwo;

        //    //for (int i = 0; i < 10; i++)
        //    //{
        //    //    row = dtArray[0].NewRow();
        //    //    row["EmpId"] = i;
        //    //    row["EmpName"] = "Taha Suliman Ramadan" + i.ToString();
        //    //    row["EmpDepart"] = "Accounting DeprtMemt" + i.ToString();
        //    //    row["EmpDesignation"] = "SoftWare Engineer" + i.ToString();
        //    //    dtArray[0].Rows.Add(row);
        //    //    //------------------------------  
        //    //    for (int j = 0; j < 5; j++)
        //    //    {
        //    //        int x = i * 5 + j;
        //    //        rowTwo = dtArray[1].NewRow();
        //    //        rowTwo["EmpContId"] = x;
        //    //        rowTwo["EmpId"] = i;
        //    //        rowTwo["EmpContMobile"] = "00971523215230";
        //    //        rowTwo["EmpContEmail"] = "taha@gmail.com" + j.ToString();
        //    //        rowTwo["EmpContAddress"] = "Sharjah Gasimiya Mahata" + j.ToString();
        //    //        rowTwo["EmpContCity"] = "United Emarate Sharjah" + j.ToString();
        //    //        dtArray[1].Rows.Add(rowTwo);
        //    //    }

        //    //}
        //    return dtArray;
        //}
    }
}
