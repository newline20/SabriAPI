using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomModels;
using Domain.Interfaces;
using Domain.Entities;

namespace Sabri_Logistics_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallanController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        ChallanMasterSuperModel challanMasterModel = new ChallanMasterSuperModel();
        
        public ChallanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        [HttpGet]
        [Route("GetChallan")]
        public IActionResult GetChallan(string ChallanStatus, string FromDt = null, string ToDt = null, decimal? ConsignorAcId = 0, decimal? ConsigneeAcId = 0, decimal? Driverid = null, string ChallanNo = null, string LrNo = null, decimal? PartyAcid = 0)
        {
            try
            {
                if(string.IsNullOrEmpty(ChallanStatus))
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Challan Status Required";
                    return new JsonResult(challanMasterModel);
                }

                DateTime? fromDt = null;
                DateTime? toDt = null;
                if (!string.IsNullOrEmpty(FromDt))
                    fromDt = Convert.ToDateTime(FromDt);
                if (!string.IsNullOrEmpty(ToDt))
                    toDt = Convert.ToDateTime(ToDt);

                var lstChallan = _unitOfWork.RepoChallan.GetChallan(ChallanStatus, fromDt, toDt, ConsignorAcId, ConsigneeAcId, Driverid, ChallanNo, LrNo, PartyAcid);

                if (lstChallan != null && lstChallan.Count > 0)
                {
                    challanMasterModel.lstChallanMstr = lstChallan;

                    challanMasterModel.Status = StatusCodes.Status200OK;
                    challanMasterModel.Message = "All Challan Retrieved Successfully";
                    return new JsonResult(challanMasterModel);
                }
                else
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "No Records found";
                    return new JsonResult(challanMasterModel);
                }
            }
            catch (Exception ex)
            {
                challanMasterModel.Status = StatusCodes.Status500InternalServerError;
                challanMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(challanMasterModel);
            }
        }

        [HttpGet]
        [Route("GetChallanByID")]
        public IActionResult GetChallanByID(decimal Challanid)
        {
            try
            {
                if (Challanid == 0)
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Challan ID Required";
                    return new JsonResult(challanMasterModel);
                }

                var lstChallan = _unitOfWork.RepoChallan.GetChallanByID(Challanid);

                if (lstChallan != null)
                {
                    challanMasterModel.ChallanMstr = lstChallan;

                    challanMasterModel.Status = StatusCodes.Status200OK;
                    challanMasterModel.Message = "Challan Retrieved Successfully";
                    return new JsonResult(challanMasterModel);
                }
                else
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "No Records found";
                    return new JsonResult(challanMasterModel);
                }
            }
            catch (Exception ex)
            {
                challanMasterModel.Status = StatusCodes.Status500InternalServerError;
                challanMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(challanMasterModel);
            }
        }


        [HttpPost]
        [Route("AddEditChallan")]
        public IActionResult AddEditChallan([FromBody] InsertChallanMasterModel challanMstModel)
        {
            try
            {
                if (challanMstModel.PaidChallan != "E" && challanMstModel.PaidChallan != "P")
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Paid Challan Required in Entry(E)/Pass(P)";
                    return new JsonResult(challanMasterModel);
                }
                decimal chaln = challanMstModel.NChallanid;
                decimal challanId = _unitOfWork.RepoChallan.AddEditChallan(challanMstModel);

                if (challanId > 0)
                {
                    challanMasterModel.NChallanid = challanId;
                    challanMasterModel.Status = StatusCodes.Status200OK;

                    //if (challanMstModel.NChallanid == 0)
                    if (chaln == 0)
                        challanMasterModel.Message = "Challan saved successfully";
                    else
                        challanMasterModel.Message = "Challan updated successfully";

                    return new JsonResult(challanMasterModel);
                }
                else
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Challan not saved due to some error";
                    return new JsonResult(challanMasterModel);

                }
            }
            catch (Exception ex)
            {
                challanMasterModel.Status = StatusCodes.Status500InternalServerError;
                challanMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(challanMasterModel);
            }
        }

        //[HttpPut]
        [HttpPost]
        [Route("ChallanReleaseUpdate")]
        public IActionResult ChallanReleaseUpdate(decimal? ChallanId, string Remarks, decimal Userid, DateTime ReleaseDt)
        {
            try
            {
                if(ChallanId == null && ChallanId==0)
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Challan Id Required";
                    return new JsonResult(challanMasterModel);
                }
                else if(string.IsNullOrEmpty(Remarks))
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Remarks Required";
                    return new JsonResult(challanMasterModel);
                }

                _unitOfWork.RepoChallan.ChallanReleaseUpdate(ChallanId, Remarks, Userid, ReleaseDt);
                challanMasterModel.Status = StatusCodes.Status200OK;
                challanMasterModel.Message = "Challan updated successfully";
                return new JsonResult(challanMasterModel);
               
            }
            catch (Exception ex)
            {
                challanMasterModel.Status = StatusCodes.Status500InternalServerError;
                challanMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(challanMasterModel);
            }
        }

        [HttpGet]
        [Route("GetInvoiceData")]
        //public IActionResult GetInvoiceData(string ChallanStatus, decimal? ConsignorAcId = 0, string FromDt = null, string ToDt = null)
        public IActionResult GetInvoiceData(string ChallanStatus, decimal? PartyAcId = 0, string FromDt = null, string ToDt = null)
        {
            try
            {
                if (string.IsNullOrEmpty(ChallanStatus))
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Challan Status Required";
                    return new JsonResult(challanMasterModel);
                }

                DateTime? fromDt = null;
                DateTime? toDt = null;
                if (!string.IsNullOrEmpty(FromDt))
                    fromDt = Convert.ToDateTime(FromDt);
                if (!string.IsNullOrEmpty(ToDt))
                    toDt = Convert.ToDateTime(ToDt);

                //var lstChallan = _unitOfWork.RepoChallan.GetInvoiceData(ChallanStatus, ConsignorAcId, fromDt, toDt);
                var lstChallan = _unitOfWork.RepoChallan.GetInvoiceWithLrData(ChallanStatus, PartyAcId, fromDt, toDt);

                if (lstChallan != null && lstChallan.Count > 0)
                {
                    challanMasterModel.lstChallanMstrWithLr = lstChallan;

                    challanMasterModel.Status = StatusCodes.Status200OK;
                    challanMasterModel.Message = "All Invoice Retrieved Successfully";
                    return new JsonResult(challanMasterModel);
                }
                else
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "No Records found";
                    return new JsonResult(challanMasterModel);
                }
            }
            catch (Exception ex)
            {
                challanMasterModel.Status = StatusCodes.Status500InternalServerError;
                challanMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(challanMasterModel);
            }
        }

        [HttpPost]
        [Route("GenerateInvoice")]
        //public IActionResult GenerateInvoice(decimal? ConsignorAcId, string InvoiceDate, decimal TotalAmount, decimal GSTRate, string InvNo, List<LrIds> lrIds)
        public IActionResult GenerateInvoice(decimal? ConsignorAcId, string InvoiceDate, decimal Userid, decimal GSTRate, string InvNo, List<LrIds> lrIds)
        {
            try
            {
                if (string.IsNullOrEmpty(InvoiceDate))
                {
                    challanMasterModel.Status = StatusCodes.Status400BadRequest;
                    challanMasterModel.Message = "Invoice Date Is Required";
                    return new JsonResult(challanMasterModel);
                }

                DateTime invoiceDt = Convert.ToDateTime(InvoiceDate);

                decimal LrIID = _unitOfWork.RepoChallan.GenerateInvoice(ConsignorAcId, invoiceDt, Userid, GSTRate, InvNo, lrIds);
                challanMasterModel.NChallanid = LrIID;
                challanMasterModel.Status = StatusCodes.Status200OK;

                challanMasterModel.Message = "Generate Invoice successfully";

                return new JsonResult(challanMasterModel);
            }
            catch (Exception ex)
            {
                challanMasterModel.Status = StatusCodes.Status500InternalServerError;
                challanMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(challanMasterModel);
            }
        }

        [Route("~/api/VoucherEntry")]
        [HttpPost]
        public async Task<ActionResult> PostData(List<VoucherEntry> request)
        {
            return Ok(await _unitOfWork.RepoChallan.InsertVoucherEntry(request));
        }

        [Route("~/api/VoucherDelete")]
        [HttpPost]
        public async Task<ActionResult> VoucherDelete(TransRecord request)
        {
            return Ok(await _unitOfWork.RepoChallan.DeleteVoucherEntry(request));
        }

        [Route("~/api/GetChallanforPayment")]
        [HttpGet]
        public async Task<ActionResult> GetChallanforPayment(decimal PartyAcId)
        {
            return Ok(await _unitOfWork.RepoChallan.GetChallanforPayment(PartyAcId));
        }

        [Route("~/api/InsertChallanPayment")]
        [HttpPost]
        public async Task<ActionResult> InsertChallanPayment(List<InsertChallanPayment> request)
        {
            return Ok(await _unitOfWork.RepoChallan.InsertChallanPayment(request));
        }

        [Route("~/api/DeleteChallanPayment")]
        [HttpPost]
        public async Task<ActionResult> DeleteChallanPayment(decimal PVID, decimal OptId)
        {
            return Ok(await _unitOfWork.RepoChallan.DeleteChallanPayment(PVID, OptId));
        }

        [Route("~/api/DeleteChallan")]
        [HttpPost]
        public async Task<ActionResult> DeleteChallan(decimal ChallanID, decimal OptId)
        {
            return Ok(await _unitOfWork.RepoChallan.DeleteChallan(ChallanID, OptId));
        }

        [Route("~/api/GetPaymentVoucher")]
        [HttpGet]
        public async Task<ActionResult> GetPaymentVoucher(decimal? PartyAcid = 0, DateTime? FromDt = null, DateTime? ToDt = null)
        {
            return Ok(await _unitOfWork.RepoChallan.GetPaymentVoucher(PartyAcid, FromDt, ToDt));
        }

        [Route("~/api/GetPaymentVoucherById")]
        [HttpGet]
        public async Task<ActionResult> GetPaymentVoucherById(decimal PVID)
        {
            return Ok(await _unitOfWork.RepoChallan.GetPaymentVoucherById(PVID));
        }

        [Route("GetChallanRegister")]
        [HttpGet]
        public async Task<ActionResult> GetChallanRegister(DateTime FromDt, DateTime ToDt)
        {
            return Ok(await _unitOfWork.RepoChallan.GetChallanRegister(FromDt, ToDt));
        }

        [Route("GetLrInvoiceById")]
        [HttpGet]
        public async Task<ActionResult> GetLrInvoiceById(decimal LrIID)
        {
            return Ok(await _unitOfWork.RepoChallan.GetLrInvoiceById(LrIID));
        }

        [Route("GetLrInvoiceDtl")]
        [HttpGet]
        public async Task<ActionResult> GetLrInvoiceDtl(DateTime? FromDt = null, DateTime? ToDt = null)
        {
            return Ok(await _unitOfWork.RepoChallan.GetLrInvoiceDtl(FromDt, ToDt));
        }
    }
}
