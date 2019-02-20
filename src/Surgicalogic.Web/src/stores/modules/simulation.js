import axios from 'axios';

const simulationModule = {
  state : {
    loading: false,
    totalCount: 0,
    model: [],
    operationList : [],
    operationListTotalCount : 0
  },

  mutations : {
    setLoading(state, data) {
      state.loading = data;
    },

    setOperationList(state, data) {
      state.operationList = data.result;
      state.operationListTotalCount = data.totalCount;
    }
  },

  getters: {},

  actions: {
    runSimulation(context, payload) {
      context.commit('setLoading', true);

      return new Promise((resolve, reject) => {
        axios.get('Simulation/Run/' + payload.selectDate)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true){
              context.commit('setOperationList', response.data) //Set the OperationList in the store
            }

            context.commit('setLoading', false);
            resolve(response);
          }, error => {
            // http failed, let the calling function know that action did not work out
            reject(error);
          })
        });
    }
  }
}

export default simulationModule;
