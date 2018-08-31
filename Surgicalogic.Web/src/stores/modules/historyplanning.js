import axios from 'axios';

const historyPlanningModule = {
  state: {
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

    setOperationPlan(state, data) {
      state.plan = data.result;
      state.totalCount = data.totalCount;
    },

    setTomorrowOperationPlans(state, data) {
      state.tomorrowPlan = data.result;
      state.totalCount = data.totalCount;
    },

    // setAllOperationPlans(state, data) {
    //   state.allPlans = data;
    // },

    // insertOperationPlan(state, { item }) {
    //   state.plan.push(item);
    // },

    // deleteOperationPlan(state, { payload }) {
    //   let index = state.plan.findIndex((item) => {
    //     return item.id === payload.id
    //   });

    //   state.plan.splice(index, 1);
    // },

    updateOperationPlan(state, payload) {
      state.plan.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
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

    getTomorrowOperations(context){
      axios.get('OperationPlan/GetTomorrowOperationPlans')
          .then(response => {
            context.commit('setTomorrowOperationPlans', response.data) //Set the OperationPlanPlan in the store
        })
    },

    // getAllOperationPlans(context) {
    //   axios.get('OperationPlanPlan/GetAllOperationPlans')
    //       .then(response => {
    //         context.commit('setAllOperationPlans', response.data.result) //Set the OperationPlanPlan in the store
    //     })
    // },

    // insertOperationPlan(context, payload) {
    //   axios.post('OperationPlanPlan/InsertOperationPlan', payload)
    //     .then(response => {
    //       if (response.data.info.succeeded == true) {
    //         context.commit('insertOperationPlan', { item: response.data.result }) //Insert the OperationPlanPlan in the store
    //       }
    //     })
    // },

    // deleteOperationPlan(context, payload) {
    //   axios.post('OperationPlanPlan/DeleteOperationPlan/' + payload.id)
    //     .then(response => {
    //       if (response.data.info.succeeded == true) {
    //         context.commit('deleteOperationPlan', { payload }); //Delete the OperationPlanPlan in the store
    //       }
    //     })
    // },

    updateOperationPlan(context, payload) {
      axios.post('OperationPlan/UpdateOperationPlan', payload)
        .then(response => {
          context.commit('updateOperationPlan', response.data.result) //Update the OperationPlanPlan in the store
        })
    },

    excelExportPlanningHistory(context) {
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

export default historyPlanningModule;
