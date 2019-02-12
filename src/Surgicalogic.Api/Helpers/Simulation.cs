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

namespace Surgicalogic.Api.Helpers
{
    public class Simulation : ISimulation
    {
        private readonly IOperationPlanStoreService _operationPlanStoreService;
        private readonly ISettingStoreService _settingStoreService;

        public static readonly Random random = new Random();
        
        public int periot = 15;
        public string startTime;
        public string endTime;

        public Simulation(IOperationPlanStoreService operationPlanStoreService, ISettingStoreService settingStoreService)
        {
            _operationPlanStoreService = operationPlanStoreService;
            _settingStoreService = settingStoreService;            
        }
        //public async Task<ResultModel<SimulationResultModel>> TodayRun(GridInputModel input)
        //{
        //    var settings = await _settingStoreService.GetAllAsync();

        //    periot = settings.FirstOrDefault(x => x.Key == "OperationPeriodInMinutes").IntValue ?? periot;
        //    startTime = settings.FirstOrDefault(x => x.Key == "OperationWorkingHourStart").TimeValue;

        //    var simulationCount = settings.SingleOrDefault(x => x.Key == SettingKey.SimulationIterationCount.ToString());

        //    //Yarının planlanmıs operasyonları cekilir
        //    var operationList = await _operationPlanStoreService.GetTomorrowOperationListAsync();

        //    //Yarının operasyonlarını operasyon odasına göre grupla
        //    List<RoomPlanModel> roomPlan = operationList.GroupBy(x => x.OperatingRoomId,
        //        (key, g) =>
        //            new RoomPlanModel()
        //            {
        //                OperatingRoomId = key,
        //                OperatingRoomName = g.FirstOrDefault().OperatingRoomName,
        //                SimulationOperationPlanModels = g.ToList() //Maping from OperationPlan entity
        //            }
        //        ).ToList();


        //    //Create Calculation List
        //    List<SimulationResultModel> simulationResultList = new List<SimulationResultModel>();

        //    //Iterate simulation list selected time
        //    for (int l = 0; l < simulationCount.IntValue; l++)
        //    {
        //        //Operasyon odasına göre gruplanmış operasyonların sürelerini random olarak değiştirir
        //        var list = GetList(roomPlan);

        //        for (int t = 0; list.Any(x => x.SimulationOperationPlanModels.Any(y => !y.IsFinished)); t++)
        //        {
        //            SetEnd(list, t);

        //            SetStart(list, t);
        //        }

        //        simulationResultList.AddRange(AddSimulationResultList(list));

        //    }

        //    var result = Calculation(simulationResultList);

        //    return new ResultModel<SimulationResultModel>
        //    {
        //        Result = result,
        //        Info = new Info()
        //    };
        //}

        public async Task<ResultModel<SimulationResultModel>> Run(GridInputModel input)
        {
            var settings = await _settingStoreService.GetAllAsync();

            periot = settings.FirstOrDefault(x => x.Key == "OperationPeriodInMinutes").IntValue ?? periot;
            startTime = settings.FirstOrDefault(x => x.Key == "OperationWorkingHourStart").TimeValue;

            var simulationCount = settings.SingleOrDefault(x => x.Key == SettingKey.SimulationIterationCount.ToString());

            //Yarının planlanmıs operasyonları cekilir
            var operationList = await _operationPlanStoreService.GetTomorrowOperationListAsync();
            
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

                simulationResultList.AddRange(AddSimulationResultList(list));

            }

            var result = Calculation(simulationResultList);

            return new ResultModel<SimulationResultModel>
            {
                Result = result,
                Info = new Info()
            };
        }

        #region Helper Methodes
       
        private int GetRandom(int max)
        {
            //Operasyon süresini periyot ve max operasyon süresi arasında random olarak uzatır. max = operasyon süresinin 2 katı
            return random.Next(periot, max);
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

                    //Sets start-periot, end-periot and operation-periot the perit of operaiton. 
                    SetPeriot(plan[b]);
                }
            }
            return list;
        }

        /// <summary>
        /// Sets the operation total, start and end periot. 
        /// </summary>
        /// <param name="model">SimulationOperationPlanModel</param>
        private void SetPeriot(SimulationOperationPlanModel model)
        {

            //TODO: System work start hour wil set. 

            var operationStartPeriot = ((model.OperationDate.Hour - Convert.ToInt32(startTime.Split(":")[0])) * 60 / periot) + (model.OperationDate.Minute / periot + (model.OperationDate.Minute % periot > 0 ? 1: 0));

            var operationPeriot = model.OperationTime / periot + (model.OperationTime % periot > 0 ? 1 : 0);

            model.StartPeriot = operationStartPeriot;

            model.EndPeriot = operationStartPeriot + operationPeriot;
            model.OperationPeriot = model.EndPeriot - model.StartPeriot;
        }

        /// <summary>
        /// Sets the property of end operation at specific periot
        /// </summary>        
        /// <param name="list">Operation plans list grouped by operation room.</param>
        /// <param name="t">Periot.</param>
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

        /// <summary>
        ///  Sets the property of start operation at specific periot
        /// </summary>
        /// <param name="list">Operation plans list grouped by operation room.</param>
        /// <param name="t">Periot</param>
        private void SetStart(List<RoomPlanModel> list, int t)
        {
            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                for (int b = 1; b < plan.Count; b++)
                {
                    if (plan[b-1].IsFinished && !plan[b].IsFinished && !plan[b].InUse && plan[b].StartPeriot <= t)
                    {                                                     
                        plan[b].StartPeriot = plan[b].StartPeriot < plan[b - 1].EndPeriot ? plan[b - 1].EndPeriot : plan[b].StartPeriot;

                        plan[b].EndPeriot = plan[b].StartPeriot + plan[b].OperationPeriot;

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
        private List<SimulationResultModel> AddSimulationResultList(List<RoomPlanModel> list)
        {
            List<SimulationResultModel> result = new List<SimulationResultModel>();

            for (int a = 0; a < list.Count; a++)
            {
                var plan = list[a].SimulationOperationPlanModels;

                double usage = 0;

                for (int b = 1; b < plan.Count; b++)
                {
                    usage += plan[b].OperationPeriot;

                }
                result.Add(RoomSimulationResult(plan));
            }

            return result;
        }

        /// <summary>
        /// Operation value calculation for per room
        /// </summary>
        /// <param name="plan"></param>
        /// <returns>SimulationResultModel</returns>
        private SimulationResultModel RoomSimulationResult(List<SimulationOperationPlanModel> plan)
        {
           
            int usage = 0;
            decimal waitingPeriot = 0;
            
            for (int i = 0; i < plan.Count; i++)
            {
                usage += plan[i].OperationPeriot;

                if (i > 0)
                    waitingPeriot +=  plan[i].StartPeriot - plan[i - 1].EndPeriot;
            }

            return new SimulationResultModel
            {
                OperatingRoomId = plan[0].OperatingRoomId,
                Usage = usage / 9 * 100,
                OverTime = plan.LastOrDefault().EndPeriot > 9 ? plan.LastOrDefault().EndPeriot - 9 : 0,
                WaitingTime = waitingPeriot / plan.Count,
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
                int usage = 0;

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
                    Usage = usage / plan.Count,
                    OverTime = overTime / plan.Count,
                    WaitingTime = waitingTime / plan.Count
                });
            }

            return result;
        }


        #endregion



    }
}
