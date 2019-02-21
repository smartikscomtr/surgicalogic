using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Helpers;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Model.Enum;
using Surgicalogic.Common.Extensions;

namespace Surgicalogic.Api.Helpers
{
    public class Simulation : ISimulation
    {
        private readonly IOperationPlanStoreService _operationPlanStoreService;
        private readonly ISettingStoreService _settingStoreService;

        public static readonly Random random = new Random();
        
        public int period = 15;
        public string startTime;
        public string endTime;

        public Simulation(IOperationPlanStoreService operationPlanStoreService, ISettingStoreService settingStoreService)
        {
            _operationPlanStoreService = operationPlanStoreService;
            _settingStoreService = settingStoreService;
        }

        public async Task<ResultModel<SimulationResultModel>> Run(GridInputModel input, DateTime selectDate)
        {
            var settings = await _settingStoreService.GetAllAsync();

            period = settings.FirstOrDefault(x => x.Key == "OperationPeriodInMinutes").IntValue ?? period;
            startTime = settings.FirstOrDefault(x => x.Key == "OperationWorkingHourStart").TimeValue;
            var workingStartTime = settings.FirstOrDefault(x => x.Key == SettingKey.OperationWorkingHourStart.ToString()).TimeValue.HourToDateTime();
            var workingEndTime = settings.FirstOrDefault(x => x.Key == SettingKey.OperationWorkingHourEnd.ToString()).TimeValue.HourToDateTime();

            var simulationCount = settings.SingleOrDefault(x => x.Key == SettingKey.SimulationIterationCount.ToString());

            //Yarının planlanmıs operasyonları cekilir
            var operationList = await _operationPlanStoreService.GetOperationByIdListAsync(selectDate);
            
            //Yarının operasyonlarını operasyon odasına göre grupla
            List<RoomPlanModel> roomPlan = operationList.GroupBy(x => x.OperatingRoomId,
                (key, g) =>
                    new RoomPlanModel()
                    {
                        OperatingRoomId = key,
                        OperatingRoomName = g.FirstOrDefault().OperatingRoomName,
                        SimulationOperationPlanModels = g.ToList() //Maping from OperationPlan entity
                    }
                ).ToList();


            //Create Calculation List
            List<SimulationResultModel> simulationResultList = new List<SimulationResultModel>();

            //Iterate simulation list selected time
            for (int l = 0; l < simulationCount.IntValue; l++)
            {
                //Operasyon odasına göre gruplanmış operasyonların sürelerini random olarak değiştirir
                var list = GetList(roomPlan);

                for (int t = 0; list.Any(x => x.SimulationOperationPlanModels.Any(y => !y.IsFinished)); t++)
                {
                    SetEnd(list, t);

                    SetStart(list, t);
                }

                simulationResultList.AddRange(AddSimulationResultList(list, workingStartTime, workingEndTime));

            }

            var result = Calculation(simulationResultList);

            return new ResultModel<SimulationResultModel>
            {
                Result = AutoMapper.Mapper.Map<List<SimulationOutputModel>>(result),
                Info = new Info()
            };
        }

        #region Helper Methodes
       
        private int GetRandom(int max)
        {
            //Operasyon süresini periyot ve max operasyon süresi arasında random olarak uzatır. max = operasyon süresinin 2 katı
            return random.Next(period, max);
        }

        /// <summary>
        ///  Gets the list of grouped operation plans. This methode changes the duration of planed operaitons.  
        /// </summary>
        /// <param name="list">Operation plans list grouped by operation room.</param>
        /// <returns>List of RoomPlanModel</returns>
        /// 


        //Planlanmış operasyonlarının sürelerini değiştirip listesini getirir
        private List<RoomPlanModel> GetList(List<RoomPlanModel> list)
        {            
            //loops for each room
            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                //First oepration and resources are set InUse for first operation in every room  
                plan[0].InUse = true;

                //loops for each operaiton in specific room.
                //Set random operation duration for each operation
                for (int b = 0; b < plan.Count; b++)
                {
                    //Clear IsFinised field for Operations by setting it to the false; 
                    plan[b].IsFinished = false;

                    //TODO: oepration time constraint will set after defined.
                    //Random olarak atanmıs operasyon süreleri 
                    plan[b].OperationTime = GetRandom(plan[b].ActualOperationTime * 2);

                    //Sets start-period, end-period and operation-period the perit of operaiton. 
                    SetPeriod(plan[b]);
                }
            }
            return list;
        }

        /// <summary>
        /// Sets the operation total, start and end period. 
        /// </summary>
        /// <param name="model">SimulationOperationPlanModel</param>
        private void SetPeriod(SimulationOperationPlanModel model)
        {

            //TODO: System work start hour wil set. 

            var operationStartPeriod = ((model.OperationDate.Hour - Convert.ToInt32(startTime.Split(":")[0])) * 60 / period) + (model.OperationDate.Minute / period + (model.OperationDate.Minute % period > 0 ? 1: 0));

            var operationPeriod = model.OperationTime / period + (model.OperationTime % period > 0 ? 1 : 0);

            model.StartPeriod = operationStartPeriod;

            model.EndPeriod = operationStartPeriod + operationPeriod;
            model.OperationPeriod = model.EndPeriod - model.StartPeriod;
        }

        /// <summary>
        /// Sets the property of end operation at specific period
        /// </summary>        
        /// <param name="list">Operation plans list grouped by operation room.</param>
        /// <param name="t">Period.</param>
        private void SetEnd(List<RoomPlanModel> list, int t)
        {            
            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                for (int b = 0; b < plan.Count; b++)
                {
                    if (plan[b].InUse && plan[b].EndPeriod == t)
                    {
                        plan[b].InUse = false;

                        plan[b].IsFinished = true;
                    }
                }
            }
            
        }

        /// <summary>
        ///  Sets the property of start operation at specific period
        /// </summary>
        /// <param name="list">Operation plans list grouped by operation room.</param>
        /// <param name="t">Period</param>
        private void SetStart(List<RoomPlanModel> list, int t)
        {
            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                for (int b = 1; b < plan.Count; b++)
                {
                    if (plan[b-1].IsFinished && !plan[b].IsFinished && !plan[b].InUse && plan[b].StartPeriod <= t)
                    {                                                     
                        plan[b].StartPeriod = plan[b].StartPeriod < plan[b - 1].EndPeriod ? plan[b - 1].EndPeriod : plan[b].StartPeriod;

                        plan[b].EndPeriod = plan[b].StartPeriod + plan[b].OperationPeriod;

                        plan[b].InUse = true;                       
                        
                    }
                }
            }            
        }

        /// <summary>
        /// Add item to the SimulationResultList
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List<SimulationResultModel></returns>
        private List<SimulationResultModel> AddSimulationResultList(List<RoomPlanModel> list, DateTime workingStartTime, DateTime workingEndTime)
        {
            List<SimulationResultModel> result = new List<SimulationResultModel>();

            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                double usage = 0;

                for (int b = 1; b < plan.Count; b++)
                {
                    usage += plan[b].OperationPeriod;

                }
                result.Add(RoomSimulationResult(plan, workingStartTime, workingEndTime));
            }

            return result;
        }

        /// <summary>
        /// Operation value calculation for per room
        /// </summary>
        /// <param name="plan"></param>
        /// <returns>SimulationResultModel</returns>
        private SimulationResultModel RoomSimulationResult(List<SimulationOperationPlanModel> plan, DateTime workingStartTime, DateTime  workingEndTime)
        {
            int usage = 0;
            decimal waitingPeriod = 0;
            
            for (int i = 0; i < plan.Count; i++)
            {
                usage += plan[i].OperationPeriod;

                if (i > 0)
                    waitingPeriod +=  plan[i].StartPeriod - plan[i - 1].EndPeriod;
            }

            return new SimulationResultModel
            {
                OperatingRoomId = plan[0].OperatingRoomId,
                Usage = (double)usage / ((double)(workingEndTime - workingStartTime).TotalMinutes / (double)period) * 100,
                OverTime = plan.LastOrDefault().EndPeriod > Convert.ToInt32((workingEndTime - workingStartTime).TotalMinutes / period) ? plan.LastOrDefault().EndPeriod - Convert.ToInt32((workingEndTime - workingStartTime).TotalMinutes / period) : 0,
                WaitingTime = waitingPeriod / plan.Count,
                OperatingRoomName = plan[0].OperatingRoomName.ToString()
            };
        }
        
        
        private List<SimulationResultModel> Calculation(List<SimulationResultModel> list)
        {
            var ListGroup = list.GroupBy(x =>
            x.OperatingRoomId,
                (key, g) =>
                new SimulationResultGroupModel()
                {
                    OperatingRoomId = key,
                    OperatingRoomName = g.FirstOrDefault().OperatingRoomName,
                    SimulationResultModels = g.ToList() 
                }
            ).ToList();

            var result = new List<SimulationResultModel>();

            for (int a = 0; a < ListGroup.Count; a++)
            {
                double usage = 0;

                int overTime = 0;

                decimal waitingTime = 0;

                var plan = ListGroup[a].SimulationResultModels;                
                    
                for (int b = 0; b < plan.Count; b++)
                {
                    usage += plan[b].Usage;

                    overTime += plan[b].OverTime;

                    waitingTime += plan[b].WaitingTime;                    
                }

                result.Add(new SimulationResultModel
                {
                    OperatingRoomId = ListGroup[a].OperatingRoomId,
                    OperatingRoomName = ListGroup[a].OperatingRoomName,
                    Usage = Math.Round(usage / (double)plan.Count, 2),
                    OverTime = overTime / plan.Count,
                    WaitingTime = Math.Round((waitingTime / plan.Count), 2)
                });
            }

            return result;
        }


        #endregion
    }
}
