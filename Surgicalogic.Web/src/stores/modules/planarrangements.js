import axios from 'axios';

const planArrangementsModule = {
  state: {
    loading: false,
    totalCount: 0,
    model: [],
    allPlans: [],
    generateOperationPlan: [],
    tomorrowList:[],
    tomorrowListTotalCount: 0,
    date: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setPlanArrangements(state, data) {
      state.model = data.result;
      state.totalCount = data.totalCount;
    },

    setDashboardTimelinePlans(state, payload) {
      state.date = payload;
    },
    setTomorrowOperationList(state, data) {
      state.tomorrowList = data.result;
      state.tomorrowListTotalCount = data.totalCount;
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

    // updatePlanArrangements(state, payload) {
    //   state.model.plan.forEach(element => {
    //     if(element.id == payload.id)
    //       Object.assign(element, payload);
    //   });
    // },

    setGenerateOperationPlan(state, data) {
      state.generateOperationPlan = data;
    },
  },

  getters: {},

  actions: {
    getPlanArrangements(context, params) {
      context.commit('setLoading', true);

      return new Promise((resolve, reject) => {
        axios.get('OperationPlan/GetOperationPlans', {
          params: params
        }).then(response => {
          if (response.statusText == 'OK' && response.data.info.succeeded == true){
            context.commit('setPlanArrangements', response.data) //Set the OperationPlanPlan in the store
          }

          context.commit('setLoading', false);
          resolve(response);
        }, error => {
          // http failed, let the calling function know that action did not work out
          reject(error);
        })
      })
    },

    getDashboardTimelinePlans(context, payload) {
      context.commit('setLoading', true);

      return new Promise((resolve, reject) => {
        axios.get('OperationPlan/GetDashboardTimelinePlans/' + payload.selectDate, {
        }).then(response => {
          if (response.statusText == 'OK' && response.data.info.succeeded == true){
            context.commit('setDashboardTimelinePlans', response.data.result) //Set the OperationPlanPlan in the store
          }

          context.commit('setLoading', false);
          resolve(response);
        }, error => {
          // http failed, let the calling function know that action did not work out
          reject(error);
        })
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

    updatePlanArrangements(context, payload) {
      context.commit('setLoading', true);

      return new Promise((resolve, reject) => {
      axios.post('OperationPlan/UpdateOperationPlan', payload, {
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        }})
        .then(response => {
          context.commit('setLoading', false);
         resolve(response);
        });
      })
    },

    getTomorrowOperationList(context){
        axios.get('OperationPlan/GetTomorrowOperationList')
            .then(response => {
                context.commit('setTomorrowOperationList', response.data) //Set the OperationPlanPlan in the store
            })
    },

    getGenerateOperationPlan(context) {
      return new Promise((resolve, reject) => {
      context.commit('setLoading', true);

      axios.post('OperationPlan/GenerateOperationPlan')
        .then(response => {
          context.commit('setGenerateOperationPlan', response.data)
          context.commit('setLoading', false);
          resolve(response);
        });
      })
    }
  }
}

export default planArrangementsModule;
