﻿using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using System.Threading.Tasks;
using Smartiks.Framework.IO;
using System.IO;
using System;
using System.Collections.Generic;

namespace Surgicalogic.Api.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class BranchController : Controller
    {
        private readonly IBranchStoreService _branchStoreService;

        public BranchController(IBranchStoreService branchStoreService)
        {
            _branchStoreService = branchStoreService;
        }

        /// <summary>
        /// Get branch methode
        /// </summary>
        /// <returns>BranchOutputModel list</returns>
        [Route("Branch/GetBranches")]
        [HttpGet]
        public async Task<ResultModel<BranchOutputModel>> GetBranch(GridInputModel input)
        {
            return await _branchStoreService.GetAsync<BranchOutputModel>(input);
        }

        [Route("Branch/GetAllBranches")]
        public async Task<ResultModel<BranchOutputModel>> GetAllBranches()
        {
            return await _branchStoreService.GetAsync<BranchOutputModel>();
        }

        [Route("Branch/ExcelExport")]
        //Write(Stream excelStream, string worksheetName, Type type, IEnumerable items, IFormatProvider formatProvider)
        public async Task ExcelExport()
        {
            FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory, "App_Data", "Migration", "InitialData4.xlsx"), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items =await _branchStoreService.GetAsync<BranchOutputModel>();
            
            excelService.Write(fs, "Worksheet", typeof(BranchOutputModel), items.Result, System.Globalization.CultureInfo.CurrentCulture);

            return 
        }

        /// <summary>
        /// Add branch methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>BranchOutputModel</returns>
        [Route("Branch/InsertBranch")]
        [HttpPost]
        public async Task<ResultModel<BranchOutputModel>> InsertBranch([FromBody] BranchInputModel item)
        {
            var branchItem = new BranchModel()
            {
                Name = item.Name,
                Description = item.Description
            };

            return await _branchStoreService.InsertAndSaveAsync<BranchOutputModel>(branchItem);
        }

        /// <summary>
        /// Remove branch item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Branch/DeleteBranch/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteBranch(int id)
        {
            return await _branchStoreService.DeleteAndSaveByIdAsync(id);
        }

        /// <summary>
        /// Update branch methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>BranchModel</returns>
        [Route("Branch/UpdateBranch")]
        [HttpPost]
        public async Task<ResultModel<BranchOutputModel>> UpdateBranch([FromBody] BranchInputModel item)
        {
            var branchItem = new BranchModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description
            };

            return await _branchStoreService.UpdateAndSaveAsync<BranchOutputModel>(branchItem);
        }
    }
}