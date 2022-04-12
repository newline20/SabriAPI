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
    public class GroupController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        GroupTypeMasterSuperModel grpGroupType = new GroupTypeMasterSuperModel();
        CategoryCodeMasterSuperModel ctgCode = new CategoryCodeMasterSuperModel();
        public GroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllGroups")]
        public IActionResult GetAllGroups(int? groupTypeId=null)
        {
            try
            {
                var Groups = _unitOfWork.RepoGroups.GetAllGroups(groupTypeId);

                if (Groups != null && Groups.Count > 0)
                {
                    grpGroupType.lstGroup = Groups;
                    grpGroupType.Status = StatusCodes.Status200OK;
                    grpGroupType.Message = "All Groups Retrieved Successfully";
                    return new JsonResult(grpGroupType);
                }
                else
                {
                    grpGroupType.Status = StatusCodes.Status400BadRequest;
                    grpGroupType.Message = "No Records found";
                    return new JsonResult(grpGroupType);
                }
            }
            catch (Exception ex)
            {
                grpGroupType.Status = StatusCodes.Status500InternalServerError;
                grpGroupType.Message = ex.InnerException.Message;
                return new JsonResult(grpGroupType);
            }
        }

        [HttpGet]
        [Route("GroupType")]
        public IActionResult GroupType()
        {
            try
            {
                var typeList = _unitOfWork.RepoGroups.GetGroupType();

                if (typeList != null && typeList.Count > 0)
                {
                    grpGroupType.lstGLType = typeList;
                    grpGroupType.Status = StatusCodes.Status200OK;
                    grpGroupType.Message = "Group Type Retrieved Successfully";
                    return new JsonResult(grpGroupType);
                }
                else
                {
                    grpGroupType.Status = StatusCodes.Status400BadRequest;
                    grpGroupType.Message = "No Records found";
                    return new JsonResult(grpGroupType);
                }
            }
            catch (Exception ex)
            {
                grpGroupType.Status = StatusCodes.Status500InternalServerError;
                grpGroupType.Message = ex.InnerException.Message;
                return new JsonResult(grpGroupType);
            }
        }

        [HttpGet]
        [Route("GroupById")]
        public IActionResult GroupById(int grpId)
        {
            try
            {
                var grpList = _unitOfWork.RepoGroups.GetGroupById(grpId);

                if (grpList != null)
                {
                    grpGroupType.grpType = grpList;
                    grpGroupType.Status = StatusCodes.Status200OK;
                    grpGroupType.Message = "Group Retrieved Successfully";
                    return new JsonResult(grpGroupType);
                }
                else
                {
                    grpGroupType.Status = StatusCodes.Status400BadRequest;
                    grpGroupType.Message = "No Records found";
                    return new JsonResult(grpGroupType);
                }
            }
            catch (Exception ex)
            {
                grpGroupType.Status = StatusCodes.Status500InternalServerError;
                grpGroupType.Message = ex.InnerException.Message;
                return new JsonResult(grpGroupType);
            }
        }

        [HttpPost]
        [Route("AddEditGroupInfo")]
        public IActionResult AddGroupInfo([FromBody] GroupMaster groupMaster)
        {
            try
            {
                if (groupMaster is null)
                {
                    grpGroupType.Status = StatusCodes.Status400BadRequest;
                    grpGroupType.Message = "Group info list is empty";
                    return new JsonResult(grpGroupType);
                    
                }
                else
                {
                    _unitOfWork.RepoGroups.AddGroupInfo(groupMaster);

                    int res = _unitOfWork.Complete();
                    if (res > 0)
                    {
                        grpGroupType.Status = StatusCodes.Status200OK;
                        if(groupMaster.Ngrpid == 0)
                            grpGroupType.Message = "Group info saved successfully";
                        else
                            grpGroupType.Message = "Group info updated successfully";
                        return new JsonResult(grpGroupType);
                    }
                    else
                    {
                        grpGroupType.Status = StatusCodes.Status400BadRequest;
                        grpGroupType.Message = "Group information not saved due to some error";
                        return new JsonResult(grpGroupType);

                    }
                }
            }
            catch (Exception ex)
            {
                grpGroupType.Status = StatusCodes.Status500InternalServerError;
                grpGroupType.Message = ex.InnerException.Message;
                return new JsonResult(grpGroupType);
            }
        }


        [HttpGet]
        [Route("GetCodeByCtgId")]
        public IActionResult GetCodeByCtgId(int? CtgId, int? ParentSrlNbr = null)
        {
            try
            {
                if (CtgId == null)
                {
                    ctgCode.Status = StatusCodes.Status400BadRequest;
                    ctgCode.Message = "Category Id is required";
                    return new JsonResult(ctgCode);
                }                    

                var CtgCodeList = _unitOfWork.RepoGroups.GetCodeByCtgId(Convert.ToInt32(CtgId), ParentSrlNbr);

                if (CtgCodeList != null && CtgCodeList.Count > 0)
                {
                    ctgCode.lstCategoryCode = CtgCodeList;
                    ctgCode.Status = StatusCodes.Status200OK;
                    ctgCode.Message = "Get Code By Category retrieved scuccessfully";
                    return new JsonResult(ctgCode);
                }
                else
                {
                    ctgCode.Status = StatusCodes.Status400BadRequest;
                    ctgCode.Message = "No Records found";
                    return new JsonResult(ctgCode);                    
                }
            }
            catch (Exception ex)
            {
                ctgCode.Status = StatusCodes.Status500InternalServerError;
                ctgCode.Message = ex.InnerException.Message;
                return new JsonResult(ctgCode);
            }
        }

        [HttpGet]
        [Route("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList(decimal CtgId)
        {
            return Ok(await _unitOfWork.RepoGroups.GetCategoryList(CtgId));
        }

    }
}
