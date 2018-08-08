import axios from 'axios';

const planModule = {
  state: {
    loading: false,
    totalCount: 0,
    plan: [],
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
    getOperationPlans(context, params) {
      context.commit('setLoading', true);
debugger;
      axios.get('OperationPlan/GetOperationPlans', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setOperationPlan', response.data) //Set the OperationPlanPlan in the store
        }

        context.commit('setLoading', false);
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
      axios.post('OperationPlanPlan/UpdateOperationPlan', payload)
        .then(response => {
          context.commit('updateOperationPlan', response.data.result) //Update the OperationPlanPlan in the store
        })
    }
  }
}

export default planModule;
