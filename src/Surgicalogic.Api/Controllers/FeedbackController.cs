using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Smartiks.Framework.IO.Excel;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;

namespace Surgicalogic.Api.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackStoreService _feedbackStoreService;

        public FeedbackController(IFeedbackStoreService feedbackStoreService)
        {
            _feedbackStoreService = feedbackStoreService;
        }

        [Route("Feedback/GetFeedbacks")]
        [HttpGet]
        public async Task<ResultModel<FeedbackOutputModel>> GetFeedbacks(GridInputModel input)
        {
            return await _feedbackStoreService.GetAsync<FeedbackOutputModel>(input);
        }

        [Route("Feedback/GetAllFeedbacks")]
        [HttpGet]
        public async Task<ResultModel<FeedbackOutputModel>> GetAllFeedbacks()
        {
            return await _feedbackStoreService.GetAsync<FeedbackOutputModel>();
        }

        [Route("Feedback/ExcelExport")]
        public async Task<string> ExcelExport(string langId)
        {
            AppSettings.SetSiteLanguage(langId);

            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Feedbacks_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _feedbackStoreService.GetExportAsync<FeedbackExportModel>();

            await excelService.WriteAsync(fs, "Worksheet", items, typeof(FeedbackExportModel), System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }

        /// <summary>
        /// Add feedbcak methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>FeedbackOutputModel</returns>
        [Route("Feedback/InsertFeedback")]
        [HttpPost]
        public async Task<ResultModel<FeedbackOutputModel>> InsertFeedback([FromBody] FeedbackInputModel item)
        {
            var feedbackItem = new FeedbackModel()
            {
                Email = item.Email,
                Body = item.Body
            };

            return await _feedbackStoreService.InsertAndSaveAsync<FeedbackOutputModel>(feedbackItem);
        }

        /// <summary>
        /// Remove branch item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("Feedback/DeleteFeedback/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteFeedback(int id)
        {
            return await _feedbackStoreService.DeleteAndSaveByIdAsync(id);
        }
    }
}