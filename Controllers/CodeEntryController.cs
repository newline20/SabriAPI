using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.CustomModels;

namespace Sabri_Logistics_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeEntryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CodeEntryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("GetList")]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _unitOfWork.RepoCode.GetList());
        }

        [Route("GetListByID")]
        [HttpGet]
        public async Task<IActionResult> GetListByID(decimal id)
        {
            return Ok(await _unitOfWork.RepoCode.GetListByID(id));
        }

        [Route("GetListByCtgID")]
        [HttpGet]
        public async Task<IActionResult> GetListByCtgID(decimal CtgID)
        {
            return Ok(await _unitOfWork.RepoCode.GetListByCtgID(CtgID));
        }

        [Route("GetParentCodesByCtgid")]
        [HttpGet]
        public async Task<IActionResult> GetParentCodesByCtgid(decimal CtgID)
        {
            return Ok(await _unitOfWork.RepoCode.GetParentCodesByCtgid(CtgID));
        }

        //[HttpGet]
        //[Route("GetParentCodesByCtgid")]
        //public async Task<IActionResult> GetParentCodesByCtgid(decimal CtgID)
        //{
        //    return Ok(await _unitOfWork.RepoCode.GetParentCodesByCtgid(CtgID));
        //}

        [Route("AddData")]
        [HttpPost]
        public async Task<ActionResult> PostData(CodeMasterModel request)
        {
            return Ok(await _unitOfWork.RepoCode.AddData(request));
        }

        [Route("EditData")]
        [HttpPut]
        public async Task<ActionResult> PutData(CodeMasterModel request)
        {
            return Ok(await _unitOfWork.RepoCode.EditData(request));
        }

        [Route("DeleteData")]
        [HttpDelete]
        public async Task<IActionResult> DeleteData(decimal id)
        {
            return Ok(await _unitOfWork.RepoCode.DeleteData(id));
        }
    }
}
