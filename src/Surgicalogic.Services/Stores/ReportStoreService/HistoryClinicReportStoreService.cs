﻿using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Surgicalogic.Contracts.Stores.IReportStoreService;
using Surgicalogic.Data.DbContexts;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.CustomModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel.ReportOutputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Services.Stores.ReportStoreService
{
    public class HistoryClinicReportStoreService : IHistoryClinicReportStoreService
    {
        private DataContext _context;
        public HistoryClinicReportStoreService(DataContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<HistoryClinicReportOutputModel>> GetAsync<TOutputModel>(HistoryClinicReportInputModel input)
        {
            var query = _context.AppointmentCalendars.Where(x => x.IsActive);

            var branchIds = input.BranchId?.Split(',').Select(int.Parse).ToList();
            var doctorIds = input.DoctorId?.Split(',').Select(int.Parse).ToList();
            var patientIds = input.PatientId?.Split(',').Select(int.Parse).ToList();

            if (!string.IsNullOrEmpty(input.SortBy))
            {
                Expression<Func<AppointmentCalendar, object>> orderBy = null;

                switch (input.SortBy)
                {
                    case "branchName":
                        orderBy = x => x.Personnel.PersonnelBranches.FirstOrDefault().Branch.Name;
                        break;
                    case "doctorName":
                        orderBy = x => x.Personnel.FirstName + " " + x.Personnel.LastName;
                        break;
                    case "patientName":
                        orderBy = x => x.Patient.FirstName + " " + x.Patient.LastName;
                        break;
                    case "appointmentDate":
                        orderBy = x => x.AppointmentDate;
                        break;
                    default:
                        orderBy = x => x.Id;
                        break;
                }

                if (input.Descending == true)
                {
                    query = query.OrderByDescending(orderBy);
                }

                else
                {
                    query = query.OrderBy(orderBy);
                }
            }

            if (branchIds?.Count > 0)
            {
                query = query.Where(x => x.Personnel.PersonnelBranches.Any(y => branchIds.Contains(y.BranchId)));
            }

            if (doctorIds?.Count > 0)
            {
                query = query.Where(x => doctorIds.Contains(x.PersonnelId));
            }

            if (patientIds?.Count > 0)
            {
                query = query.Where(x => patientIds.Contains(x.PatientId));
            }

            if (input.AppointmentStartDate != null && input.AppointmentStartDate > DateTime.MinValue)
            {
                query = query.Where(x => x.AppointmentDate >= input.AppointmentStartDate);
            }

            if (input.AppointmentEndDate !=null && input.AppointmentEndDate > DateTime.MinValue)
            {
                query = query.Where(x => x.AppointmentDate <= input.AppointmentEndDate.AddDays(1));
            }

            if (!string.IsNullOrEmpty(input.Search))
            {
                query = query.Where(
                    x =>
                    x.Personnel.PersonnelBranches.FirstOrDefault().Branch.Name.IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    (x.Personnel.FirstName + " " + x.Personnel.LastName).IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    (x.Patient.FirstName + " " + x.Patient.LastName).IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    x.AppointmentDate.ToString().IndexOf(input.Search, StringComparison.CurrentCultureIgnoreCase) >= 0
                );
            }

            int totalCount = await query.CountAsync();

            if (input.PageSize > 0)
            {
                query = query.Skip((input.CurrentPage - 1) * input.PageSize).Take(input.PageSize);
            }

            var result = await query.ProjectTo<HistoryClinicReportOutputModel>().ToListAsync();

            return new ResultModel<HistoryClinicReportOutputModel>
            {
                Result = result,
                TotalCount = totalCount,
                Info = new Info { Succeeded = true }
            };

            //return base.GetAsync<TOutputModel>(input, expression);
        }

        public async Task<List<HistoryClinicReportExportModel>> GetExportAsync<HistoryClinicReportOutputModel>(HistoryClinicReportInputModel input)
        {
            var query = _context.AppointmentCalendars.Where(x => x.IsActive);

            return await query.ProjectTo<HistoryClinicReportExportModel>().ToListAsync();
        }
    }
}
