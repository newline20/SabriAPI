using DataAccessLayer.UnitOfWorks;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.CustomModels;

namespace Sabri_Logistics_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        UserMasterModel userMasterModel = new UserMasterModel();
        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Post([FromBody] UserMaster user)
        {
            try
            {
                if (user is null)
                {
                    var result = new UserMasterModel {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Username or Password should not be empty"
                    };
                    return new JsonResult(result);                    
                }
                else
                {
                    var userMaster = _unitOfWork.RepoLogins.ValidateLogin(user.Cusername, user.Cpassword);
                    if (userMaster != null)
                    {
                        var result = new UserMasterModel
                        {
                            Cusername = userMaster.Cusername,
                            Nuserid = userMaster.Nuserid,
                            Nroleid = userMaster.Nroleid,
                            Cstatus = userMaster.Cstatus,
                            Menu = userMaster.Menu,
                            Status = StatusCodes.Status200OK,
                            Message = "Username or Password is validated"
                        };
                        return new JsonResult(result);
                    }
                    else
                    {
                        var result = new UserMasterModel
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Message = "Username or Password not validated"
                        };
                        return new JsonResult(result);
                    }
                }
            }
            catch(Exception ex)
            {
                userMasterModel.Status = StatusCodes.Status500InternalServerError;
                userMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(userMasterModel);
            }
        }

        [HttpGet]
        [Route("CheckUser")]
        public IActionResult CheckUser(string username)
        {
            try
            {
                var isUserExists = _unitOfWork.RepoLogins.CheckUser(username);

                if (isUserExists)
                {
                    var result = new UserMasterModel
                    {
                        Status = StatusCodes.Status200OK,
                        Message = "User is Validated"
                    };
                    return new JsonResult(result);
                }
                else
                {
                    var result = new UserMasterModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "No Records found"
                    };
                    return new JsonResult(result);                    
                }
            }
            catch (Exception ex)
            {
                userMasterModel.Status = StatusCodes.Status500InternalServerError;
                userMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(userMasterModel);
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromBody] UserMaster user)
        {
            try
            {
                if (user is null)
                {
                    var result = new UserMasterModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "User list is empty"
                    };
                    return new JsonResult(result);
                }
                else
                {
                    decimal userId = _unitOfWork.RepoLogins.AddUser(user);
                    
                    if (userId > 0)
                    {
                        var userMaster = _unitOfWork.RepoLogins.GetUserById(userId);
                        var result = new UserMasterModel
                        {
                            Cusername = userMaster.Cusername,
                            Nuserid = userMaster.Nuserid,
                            Nroleid = userMaster.Nroleid,
                            Cstatus = userMaster.Cstatus,
                            Status = StatusCodes.Status200OK,
                            Message = "User Saved Successfully"
                        };
                        return new JsonResult(result);
                    }
                    else
                    {
                        var result = new UserMasterModel
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Message = "User information not saved due to some error"
                        };
                        return new JsonResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                userMasterModel.Status = StatusCodes.Status500InternalServerError;
                userMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(userMasterModel);
            }
        }

        [HttpPost]
        [Route("EditUser")]
        public IActionResult EditUser([FromBody] UserMaster user)
        {
            try
            {
                if (user is null)
                {
                    var result = new UserMasterModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "User list is empty"
                    };
                    return new JsonResult(result);
                }
                else
                {
                    decimal userId = _unitOfWork.RepoLogins.EditUser(user);

                    if (userId > 0)
                    {
                        var userMaster = _unitOfWork.RepoLogins.GetUserById(userId);
                        var result = new UserMasterModel
                        {
                            Cusername = userMaster.Cusername,
                            Nuserid = userMaster.Nuserid,
                            Nroleid = userMaster.Nroleid,
                            Cstatus = userMaster.Cstatus,
                            Status = StatusCodes.Status200OK,
                            Message = "User Update Successfully"
                        };
                        return new JsonResult(result);
                    }
                    else
                    {
                        var result = new UserMasterModel
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Message = "User information not saved due to some error"
                        };
                        return new JsonResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                userMasterModel.Status = StatusCodes.Status500InternalServerError;
                userMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(userMasterModel);
            }
        }

        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                var Roles = _unitOfWork.RepoRoles.GetRoles();

                if (Roles != null && Roles.Count > 0)
                {
                    List<RoleMasterModel> lstRoles = new List<RoleMasterModel>();
                    foreach(var f in Roles)
                    {
                        RoleMasterModel roleMasterModel = new RoleMasterModel();
                        roleMasterModel.NRoleid = f.NRoleid;
                        roleMasterModel.CRole = f.CRole;
                        lstRoles.Add(roleMasterModel);
                    }
                    var result = new RoleMasterModel
                    {
                        lstRoleMasterModels = lstRoles,
                        Status = StatusCodes.Status200OK,
                        Message = "Get All Roles"
                    };
                    return new JsonResult(result);

                }
                else
                {
                    var result = new UserMasterModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "No Records found"
                    };
                    return new JsonResult(result);                    
                }
            }
            catch(Exception ex)
            {
                userMasterModel.Status = StatusCodes.Status500InternalServerError;
                userMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(userMasterModel);
            }
        }

        [HttpPost]
        [Route("AddRole")]
        public IActionResult AddRole([FromBody] RoleMaster req)
        {
            try
            {
                if (req is null)
                {
                    var result = new RoleMasterModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Role list is empty"
                    };
                    return new JsonResult(result);
                }
                else
                {
                    decimal roleId = _unitOfWork.RepoRoles.AddRoles(req);

                    if (roleId > 0)
                    {
                        var roleMaster = _unitOfWork.RepoRoles.GetRoleById(roleId);
                        var result = new RoleMasterModel
                        {
                            NRoleid = roleMaster.NRoleid,
                            CRole = roleMaster.CRole,
                            Status = StatusCodes.Status200OK,
                            Message = "Role Saved Successfully"
                        };
                        return new JsonResult(result);
                    }
                    else
                    {
                        var result = new RoleMasterModel
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Message = "Role information not saved due to some error"
                        };
                        return new JsonResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                userMasterModel.Status = StatusCodes.Status500InternalServerError;
                userMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(userMasterModel);
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers(decimal UserID)
        {
            try
            {
                var Users = _unitOfWork.RepoLogins.GetAllUsers(UserID);

                if (Users != null && Users.Count > 0)
                {
                    userMasterModel.lstAllUsers = Users;
                    userMasterModel.Status = StatusCodes.Status200OK;
                    userMasterModel.Message = "Get All Users";
                    return new JsonResult(userMasterModel);
                }
                else
                {
                    userMasterModel.Status = StatusCodes.Status400BadRequest;
                    userMasterModel.Message = "No Records found";
                    return new JsonResult(userMasterModel);
                }
            }
            catch (Exception ex)
            {
                userMasterModel.Status = StatusCodes.Status500InternalServerError;
                userMasterModel.Message = ex.InnerException.Message;
                return new JsonResult(userMasterModel);
            }
        }

        [Route("SetLoginPassword")]
        [HttpPost]
        public async Task<ActionResult> SetLoginPassword(UserMaster login)
        {
            return Ok(await _unitOfWork.RepoLogins.SetLoginPassword(login.Cusername, login.Cpassword));
        }

        [Route("RolePermission")]
        [HttpPost]
        public async Task<ActionResult> RolePermission(UserRoleModel request)
        {
            return Ok(await _unitOfWork.RepoLogins.RolePermission(request));
        }

        [Route("GetPermissionByRoleid")]
        [HttpGet]
        public async Task<ActionResult> GetPermissionByRoleid(decimal RoleID)
        {
            return Ok(await _unitOfWork.RepoLogins.GetPermissionByRoleid(RoleID));
        }
    }
}
