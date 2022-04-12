using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Entities;
using Domain.CustomModels;

namespace Sabri_Logistics_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GstInfoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        GstMasterModel gstMasterModel = new GstMasterModel();

        public GstInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetGstData")]
        public IActionResult GetGstData(decimal gstTax = 0)
        {
            try
            {
                var gstData = _unitOfWork.RepoGstInfo.GetGstData(gstTax);

                if (gstData != null && gstData.Count > 0)
                {
                    gstMasterModel.lstGsts = gstData;

                    gstMasterModel.Status = StatusCodes.Status200OK;
                    gstMasterModel.Message = "All Gst data Retrieved Successfully";
                    return new JsonResult(gstMasterModel);
                }
                else
                {
                    gstMasterModel.Status = StatusCodes.Status400BadRequest;
                    gstMasterModel.Message = "No Records found";
                    return new JsonResult(gstMasterModel);
                }
            }
            catch (Exception ex)
            {
                gstMasterModel.Status = StatusCodes.Status500InternalServerError;
                gstMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(gstMasterModel);
            }
        }

        [HttpPost]
        [Route("AddEditGstData")]
        public IActionResult AddEditGstData([FromBody] Gstmaster gstmaster)
        {
            try
            {
                if (string.IsNullOrEmpty(gstmaster.CGstclass))
                {
                    gstMasterModel.Status = StatusCodes.Status400BadRequest;
                    gstMasterModel.Message = "Gst information is required";
                    return new JsonResult(gstMasterModel);
                }               
                else
                {
                    if (gstmaster.NGstid == 0)
                        gstMasterModel.Message = "GST info saved successfully";
                    else
                        gstMasterModel.Message = "GST info updated successfully";

                    decimal gstId = _unitOfWork.RepoGstInfo.AddEditGstData(gstmaster);

                    if (gstId > 0)
                    {
                        gstMasterModel.NGstid = gstId;
                        gstMasterModel.Status = StatusCodes.Status200OK;
                        
                        return new JsonResult(gstMasterModel);
                    }
                    else
                    {
                        gstMasterModel.Status = StatusCodes.Status400BadRequest;
                        gstMasterModel.Message = "GST information not saved due to some error";
                        return new JsonResult(gstMasterModel);

                    }
                }
            }
            catch (Exception ex)
            {
                gstMasterModel.Status = StatusCodes.Status500InternalServerError;
                gstMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(gstMasterModel);
            }
        }
    }
}
