using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Helpers;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Surgicalogic.Api.Helpers
{
    public class Simulation : ISimulation
    {
        #region Properties

        private readonly IOperationPlanStoreService _operationPlanStoreService;
        private readonly ISettingStoreService _settingStoreService;

        public static readonly Random random = new Random();

        public int periot = 15;
        public string startTime;
        public string endTime;
        #endregion

        #region Constructor

        /// <summary>
        /// Simulation Helper Constructor     
        /// </summary>
        public Simulation(IOperationPlanStoreService operationPlanStoreService, ISettingStoreService settingStoreService)
        {
            _operationPlanStoreService = operationPlanStoreService;
            _settingStoreService = settingStoreService;            
        }

        #endregion

        #region Core Logic

        public async Task<List<RoomPlanModel>> Run()
        {
            //Gets settings. 
            //ex: periot, start working hour Time, end working hour
            var settings = await _settingStoreService.GetAllAsync();

            periot = settings.FirstOrDefault(x => x.Key == "OperationPeriodInMinutes").IntValue ?? periot;

            startTime = settings.FirstOrDefault(x => x.Key == "OperationWorkingHourStart").TimeValue;

            endTime = settings.FirstOrDefault(x => x.Key == "OperationWorkingHourEnd").TimeValue;

            //Gets all tomorrow operation list.
            var operationList = await _operationPlanStoreService.GetTomorrowOperationListAsync();

            //Grouping tomorrow operations
            List<RoomPlanModel> roomPlan =
            operationList.GroupBy(x =>
                x.OperatingRoomId,
                (key, g) =>                
                    new RoomPlanModel()
                    {
                        OperatingRoomId = key,
                        SimulationOperationPlanModels = g.ToList()
                    }                           
            ).ToList();

            //Iterate simulation list selected time. 
            //Dfault set 10000.  
            for (int l = 0; l < 1; l++)
            {
                var list = GetList(roomPlan);

                for (int t = 0; list.Any(x => x.SimulationOperationPlanModels.Any(y => !y.IsFinished)); t++)
                {
                    SetEnd(list, t);

                    SetStart(list, t);
                }
            }

            return roomPlan;
        }

        #endregion

        #region Helper Methodes

        private int GetRandom(int max)
        {
            //Generatates raddom opearation duration.
            //Between periot = (15) and max = (double of the operation duration)
            return random.Next(periot, max);
        }

        private List<RoomPlanModel> GetList(List<RoomPlanModel> list)
        {
            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                //First oepration and resources are set InUse for first operation in every room  
                plan[0].InUse = true;

                //Set random operation duration for each operation
                for (int b = 0; b < plan.Count; b++)
                {
                    plan[b].OperationTime = GetRandom(plan[b].ActualOperationTime * 2);

                    SetPeriot(plan[b]);
                }

            }

            return list;
        }
        
        private void SetPeriot(SimulationOperationPlanModel model)
        {
            //TODO: System work start hour wil set. 
            var operationStartPeriot = ((model.OperationDate.Hour - 8) * 60 / periot) + (model.OperationDate.Minute / periot + (model.OperationDate.Minute % periot > 0 ? 1: 0));

            var operationPeriot = model.OperationTime / periot + (model.OperationTime % periot > 0 ? 1 : 0);

            model.StartPeriot = operationStartPeriot;

            model.EndPeriot = operationStartPeriot + operationPeriot;

            model.OperationPeriot = model.EndPeriot - model.StartPeriot;
        }

        private void SetEnd(List<RoomPlanModel> list, int t)
        {            
            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                for (int b = 0; b < plan.Count; b++)
                {
                    if (plan[b].InUse && plan[b].EndPeriot == t)
                    {
                        plan[b].InUse = false;
                        plan[b].IsFinished = true;
                    }
                }
            }
            
        }

        private void SetStart(List<RoomPlanModel> list, int t)
        {
            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                for (int b = 0; b < plan.Count; b++)
                {
                    if (!plan[b].InUse && plan[b].StartPeriot <= t)
                    {

                        if (b > 0)
                        {                             
                            plan[b].StartPeriot = plan[b].StartPeriot < plan[b - 1].EndPeriot ? plan[b - 1].EndPeriot : plan[b].StartPeriot;
                            plan[b].EndPeriot = plan[b].StartPeriot + plan[b].OperationPeriot;

                            plan[b].InUse = true;

                        }
                        
                    }
                }
            }            
        }


        #endregion



    }
}
