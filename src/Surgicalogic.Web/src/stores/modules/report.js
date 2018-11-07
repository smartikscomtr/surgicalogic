import axios from 'axios';

const reportsModule = {
  state: {
    reportItems: [],
    loading: false,
    totalCount: 0,
    plan: [],
    tomorrowPlan: [],
    allPlans: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setOvertimeOperations(state, data) {
      state.reportItems = data.result;
      state.totalCount = data.totalCount;
    },

    setOperationPlan(state, data) {
      state.plan = data.result;
      state.totalCount = data.totalCount;
    },

    setTomorrowOperationPlans(state, data) {
      state.tomorrowPlan = data.result;
      state.totalCount = data.totalCount;
    }
  },

  getters: {},

  actions: {
    getOperationPlanHistory(context, params) {
      context.commit('setLoading', true);

      axios.get('OperationPlan/GetOperationPlanHistory', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setOperationPlan', response.data) //Set the OperationPlanPlan in the store
        }

        context.commit('setLoading', false);
      })
    },

    getOvertimeOperations(context, params) {
      context.commit('setLoading', true);

      axios.get('Report/OvertimeReportPage', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setOvertimeOperations', response.data) //Set the OperationPlanPlan in the store
        }

        context.commit('setLoading', false);
      })

    },

    excelExportOvertimeOperations(context, params) {
      axios.get('Report/OvertimeReportExcelExport', {
        params: params
      })
        .then(response => {
          const link = document.createElement('a');

          link.href =  "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    },

    excelExportHistoryPlanning(context, params) {
      axios.get('Report/HistoryPlanningReportExcelExport', {
        params: params
      })
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    },
  }
}

export default reportsModule;
