using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    public class PersonnelTitleController : Controller
    {
        private readonly IPersonnelTitleStoreService _personnelTitleStoreService;

        public PersonnelTitleController(IPersonnelTitleStoreService personnelTitleStoreService)
        {
            _personnelTitleStoreService = personnelTitleStoreService;
        }

        [Route("PersonnelTitle/GetPersonnelTitles")]
        [HttpGet]
        public async Task<ResultModel<PersonnelTitleOutputModel>> GetPersonnelTitles()
        {
            return await _personnelTitleStoreService.GetAsync<PersonnelTitleOutputModel>();
        }
    }
}