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
    public class LrEntryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        LrMasterSuperModel lrMasterModel = new LrMasterSuperModel();
        public LrEntryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetLrPending")]
        public IActionResult GetLrPending(decimal? AcId = 0, string fromDt = null, string toDt = null,decimal? VehId=null, decimal? LrItemId = null)
        {
            try
            {
                List<LrMasterModel> lstChallan = new List<LrMasterModel>();
                DateTime? FromDt = null;
                DateTime? ToDt = null;
                if (!string.IsNullOrEmpty(fromDt))
                    FromDt = Convert.ToDateTime(fromDt);
                if (!string.IsNullOrEmpty(toDt))
                    ToDt = Convert.ToDateTime(toDt);

                lstChallan = _unitOfWork.RepoLREntry.GetLrPending(AcId, FromDt, ToDt, VehId, LrItemId);

                if (lstChallan != null && lstChallan.Count > 0)
                {
                    lrMasterModel.lstLrPending = lstChallan;

                    lrMasterModel.Status = StatusCodes.Status200OK;
                    lrMasterModel.Message = "All Lr Pending Retrieved Successfully";
                    return new JsonResult(lrMasterModel);
                }
                else
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "No Records found";
                    return new JsonResult(lrMasterModel);
                }
            }
            catch (Exception ex)
            {
                lrMasterModel.Status = StatusCodes.Status500InternalServerError;
                lrMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(lrMasterModel);
            }
        }

        [HttpGet]
        [Route("GetAllLr")]
        public IActionResult GetAllLr(string fromDt, string toDt, string Status, decimal? Acid = 0, decimal? PartyAcid = 0)
        {
            try
            {
                if (fromDt == null)
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "From Date is required";
                    return new JsonResult(lrMasterModel);
                }
                else if (toDt == null)
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "To Date is required";
                    return new JsonResult(lrMasterModel);
                }
                if (string.IsNullOrEmpty(Status))
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "Status is required";
                    return new JsonResult(lrMasterModel);
                }

                var lrs = _unitOfWork.RepoLREntry.GetAllLr(Convert.ToDateTime(fromDt), Convert.ToDateTime(toDt), Status, Acid, PartyAcid);

                if (lrs != null && lrs.Count > 0)
                {
                    lrMasterModel.lstAllLr = lrs;

                    lrMasterModel.Status = StatusCodes.Status200OK;
                    lrMasterModel.Message = "All Lrs Retrieved Successfully";
                    return new JsonResult(lrMasterModel);
                }
                else
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "No Records found";
                    return new JsonResult(lrMasterModel);
                }
            }
            catch (Exception ex)
            {
                lrMasterModel.Status = StatusCodes.Status500InternalServerError;
                lrMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(lrMasterModel);
            }
        }

        [HttpGet]
        [Route("GetAllLrById")]
        public IActionResult GetAllLr(decimal NLrid)
        {
            try
            {                
                var lrs = _unitOfWork.RepoLREntry.GetAllLr(NLrid);

                if (lrs != null && lrs.Count > 0)
                {
                    lrMasterModel.lstAllLr = lrs;

                    lrMasterModel.Status = StatusCodes.Status200OK;
                    lrMasterModel.Message = "All Lrs Retrieved Successfully";
                    return new JsonResult(lrMasterModel);
                }
                else
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "No Records found";
                    return new JsonResult(lrMasterModel);
                }
            }
            catch (Exception ex)
            {
                lrMasterModel.Status = StatusCodes.Status500InternalServerError;
                lrMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(lrMasterModel);
            }
        }


        [HttpPost]
        [Route("AddEditLrEntry")]
        public IActionResult AddEditLrEntry([FromBody] Lrmaster lrmaster)
        {
            try
            {
                if (lrmaster.NConsignorAcid == 0)
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "Consignor Account ID is required";
                    return new JsonResult(lrMasterModel);
                }
                else if (lrmaster.DLrdate == null)
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "LR Date is required";
                    return new JsonResult(lrMasterModel);
                }
                if (lrmaster.NConsigneeAcid == 0)
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "Consignee Account ID is required";
                    return new JsonResult(lrMasterModel);
                }
                else if (string.IsNullOrEmpty(lrmaster.CLrno))
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "LR Nbr is required";
                    return new JsonResult(lrMasterModel);
                }
                else if (lrmaster.NVehid == 0)
                {
                    lrMasterModel.Status = StatusCodes.Status400BadRequest;
                    lrMasterModel.Message = "Vehicle ID is required";
                    return new JsonResult(lrMasterModel);
                }
                else
                {
                    if (lrmaster.NLrid == 0)
                        lrMasterModel.Message = "LR Entry saved successfully";
                    else
                        lrMasterModel.Message = "LR Entry updated successfully";

                    decimal lrId = _unitOfWork.RepoLREntry.AddEditLrEntry(lrmaster);

                    if (lrId > 0)
                    {
                        lrMasterModel.NLrid = lrId;
                        lrMasterModel.Status = StatusCodes.Status200OK;

                        return new JsonResult(lrMasterModel);
                    }
                    else
                    {
                        lrMasterModel.Status = StatusCodes.Status400BadRequest;
                        lrMasterModel.Message = "LR entry not saved due to some error";
                        return new JsonResult(lrMasterModel);

                    }
                }
            }
            catch (Exception ex)
            {
                lrMasterModel.Status = StatusCodes.Status500InternalServerError;
                lrMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(lrMasterModel);
            }
        }


        [Route("~/api/DeleteLr")]
        [HttpPost]
        public async Task<ActionResult> DeleteLr(decimal LrID, decimal OptId)
        {
            return Ok(await _unitOfWork.RepoLREntry.DeleteLr(LrID, OptId));
        }

        [Route("~/api/GetDeleteLr")]
        [HttpGet]
        public async Task<ActionResult> GetDeleteLr(decimal Acid, DateTime? fromDt, DateTime? toDt)
        {
            return Ok(await _unitOfWork.RepoLREntry.GetDeleteLr(Acid, fromDt, toDt));
        }

        [Route("GetLR")]
        [HttpGet]
        public async Task<ActionResult> GetLR(string LRNo)
        {
            return Ok(await _unitOfWork.RepoLREntry.GetLR(LRNo));
        }

        [Route("GetLRByLRNo")]
        [HttpGet]
        public async Task<ActionResult> GetLRByLRNo(string LRNo, decimal LrID)
        {
            return Ok(await _unitOfWork.RepoLREntry.GetLRByLRNo(LRNo, LrID));
        }
    }
}
