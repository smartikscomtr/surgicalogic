import axios from 'axios';

const reportsModule = {
  state: {
    reportItems: [],
    loading: false,
    totalCount: 0
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setOvertimeOperations(state, data) {
      state.reportItems = data.result;
      state.totalCount = data.totalCount;
    }
  },

  getters: {},

  actions: {
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

    excelExportOvertimeOperations(context) {
      axios.get('OperationPlan/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href =  "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    }
  }
}

export default reportsModule;
