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
    public class AccountInfoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        AccountMasterSuperModel acntMaster = new AccountMasterSuperModel();
        
        public AccountInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetLedger")]
        public IActionResult GetLedger(int grpId = 0, decimal Acid = 0, string ledgerName = null, int LoadLimit = 0, int GrpTypeId = 0)
        {
            try
            {
                var accounts = _unitOfWork.RepoAccounts.getAccounts(grpId,Acid,ledgerName,LoadLimit, GrpTypeId);

                if (accounts != null && accounts.Count > 0)
                {
                    acntMaster.lstAcntMaster = accounts;
                    acntMaster.Status = StatusCodes.Status200OK;
                    acntMaster.Message = "All Accounts Retrieved Successfully";
                    return new JsonResult(acntMaster);
                }
                else
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "No Records found";
                    return new JsonResult(acntMaster);
                }
            }
            catch (Exception ex)
            {
                acntMaster.Status = StatusCodes.Status500InternalServerError;
                acntMaster.Message = ex.InnerException.Message;
                return new JsonResult(acntMaster);
            }
        }

        [HttpPost]
        [Route("AddEditAcntLedger")]
        public IActionResult AddEditAcntLedger([FromBody] AccountMasterModelInsert accountMaster)
        {
            try
            {
                if (String.IsNullOrEmpty(accountMaster.CLedger))
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "Ledger Name is required";
                    return new JsonResult(acntMaster);
                }
                else if (accountMaster.NGrpid == 0)
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "Group Id is required";
                    return new JsonResult(acntMaster);
                }
                else if (accountMaster.DOpenDate == null)
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "Open date is required";
                    return new JsonResult(acntMaster);
                }
                else
                {
                    decimal acntId = _unitOfWork.RepoAccounts.AddEditAcntLedger(accountMaster);
                    
                    if (acntId > 0)
                    {
                        //acntMaster.NAcid = acntId;
                        acntMaster.Status = StatusCodes.Status200OK;
                        if (accountMaster.NAcid == 0)
                            acntMaster.Message = "Account info saved successfully";
                        else
                            acntMaster.Message = "Account info updated successfully";
                        return new JsonResult(acntMaster);
                    }
                    else
                    {
                        acntMaster.Status = StatusCodes.Status400BadRequest;
                        acntMaster.Message = "Account information not saved due to some error";
                        return new JsonResult(acntMaster);

                    }
                }
            }
            catch (Exception ex)
            {
                acntMaster.Status = StatusCodes.Status500InternalServerError;
                acntMaster.Message = ex.InnerException.Message;
                return new JsonResult(acntMaster);
            }
        }

        [HttpGet]
        [Route("IsExistVehicle")]
        public IActionResult IsExistVehicle(int VehId)
        {
            try
            {
                if (VehId == 0)
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "Vehicle Id required";
                    return new JsonResult(acntMaster);
                }

                var isExist = _unitOfWork.RepoAccounts.IsExistVehicle(VehId);
                acntMaster.IsExistVehicle = isExist;
                acntMaster.Status = StatusCodes.Status200OK;
                acntMaster.Message = "All Accounts Retrieved Successfully";
                return new JsonResult(acntMaster);
            }
            catch (Exception ex)
            {
                acntMaster.Status = StatusCodes.Status500InternalServerError;
                acntMaster.Message = ex.InnerException.Message;
                return new JsonResult(acntMaster);
            }
        }


        [HttpGet]
        [Route("GetTrucks")]
        public IActionResult GetTrucks()
        {
            try
            {
                var lstTrucks = _unitOfWork.RepoAccounts.GetTrucks();

                if (lstTrucks != null && lstTrucks.Count > 0)
                {
                    acntMaster.lstVehicles = lstTrucks;

                    acntMaster.Status = StatusCodes.Status200OK;
                    acntMaster.Message = "All Trucks Retrieved Successfully";
                    return new JsonResult(acntMaster);
                }
                else
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "No Records found";
                    return new JsonResult(acntMaster);
                }
            }
            catch (Exception ex)
            {
                acntMaster.Status = StatusCodes.Status500InternalServerError;
                acntMaster.Message = ex.InnerException.Message;
                return new JsonResult(acntMaster);
            }
        }

        [HttpGet]
        [Route("GetDrivers")]
        public IActionResult GetDrivers(decimal nAcId = 0, int nVehId = 0)
        {
            try
            {
                if(nAcId == 0 && nVehId == 0)
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "Account Id or Vehicle Id is Required";
                    return new JsonResult(acntMaster);
                }

                var lstDrivers = _unitOfWork.RepoAccounts.GetDrivers(nAcId, nVehId);

                if (lstDrivers != null && lstDrivers.Count > 0)
                {
                    acntMaster.lstDrivers = lstDrivers;

                    acntMaster.Status = StatusCodes.Status200OK;
                    acntMaster.Message = "All Drivers Retrieved Successfully";
                    return new JsonResult(acntMaster);
                }
                else
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "No Records found";
                    return new JsonResult(acntMaster);
                }
            }
            catch (Exception ex)
            {
                acntMaster.Status = StatusCodes.Status500InternalServerError;
                acntMaster.Message = ex.InnerException.Message;
                return new JsonResult(acntMaster);
            }
        }

        [HttpGet]
        [Route("GetPaymentDetails")]
        public IActionResult GetPaymentDetails(string PaidChallan)
        {
            try
            {
                if (PaidChallan != "E" && PaidChallan != "P")
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "Paid Challan Required in Entry(E)/Pass(P)";
                    return new JsonResult(acntMaster);
                }
                var lstPaymentDtls = _unitOfWork.RepoAccounts.GetPaymentDtls(PaidChallan);

                if (lstPaymentDtls != null && lstPaymentDtls.Count > 0)
                {
                    acntMaster.lstPaymentDtls = lstPaymentDtls;

                    acntMaster.Status = StatusCodes.Status200OK;
                    acntMaster.Message = "All Payments Retrieved Successfully";
                    return new JsonResult(acntMaster);
                }
                else
                {
                    acntMaster.Status = StatusCodes.Status400BadRequest;
                    acntMaster.Message = "No Records found";
                    return new JsonResult(acntMaster);
                }
            }
            catch (Exception ex)
            {
                acntMaster.Status = StatusCodes.Status500InternalServerError;
                acntMaster.Message = ex.InnerException.Message;
                return new JsonResult(acntMaster);
            }
        }

        [HttpGet]
        [Route("GetAcLedger")]
        public async Task<IActionResult> GetAcLedger(int AcId, DateTime frmDt, DateTime toDt)
        {
            return Ok(await _unitOfWork.RepoAccounts.GetAcLedger(AcId, frmDt, toDt));
        }

        [HttpGet]
        [Route("GetVoucherScroll")]
        public async Task<IActionResult> GetVoucherScroll(DateTime frmDt, DateTime toDt, int VrType)
        {
            return Ok(await _unitOfWork.RepoAccounts.GetVoucherScroll(frmDt, toDt, VrType));
        }

        [HttpPost]
        [Route("ChangeLedgerInVehicle")]
        public async Task<IActionResult> ChangesLedgerInVehicle(decimal VehId, decimal AcId, bool? Active)
        {
            return Ok(await _unitOfWork.RepoAccounts.ChangesLedgerInVehicle(VehId, AcId, Active));
        }

        [HttpPost]
        [Route("ChangeOwnerOnVehicle")]
        public async Task<IActionResult> ChangeOwnerOnVehicle(decimal VehId, decimal AcId)
        {
            return Ok(await _unitOfWork.RepoAccounts.ChangeOwnerOnVehicle(VehId, AcId));
        }

    }
}
