import axios from 'axios';

const operationModule = {
  state: {
    loading: false,
    totalCount: 0,
    operation: [],
    allOperations: [],
    allOperationTypes: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setOperation(state, data) {
      state.operation = data.result;
      state.totalCount = data.totalCount;
    },

    setAllOperations(state, data) {
      state.allOperations = data;
    },

    insertOperation(state, { item }) {
      state.operation.push(item);
    },

    deleteOperation(state, { payload }) {
      let index = state.operation.findIndex((item) => {
        return item.id === payload.id
      });

      state.operation.splice(index, 1);
    },

    updateOperation(state, payload) {
      state.operation.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setAllOperationType(state, payload) {
      state.allOperationTypes = payload;
    }
  },

  getters: {},

  actions: {
    getOperations(context, params) {
      context.commit('setLoading', true);

      axios.get('Operation/GetOperations', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setOperation', response.data) //Set the Operation in the store
        }

        context.commit('setLoading', false);
      })

    },

    getAllOperations(context) {
      axios.get('Operation/GetAllOperations')
          .then(response => {
            context.commit('setAllOperations', response.data.result) //Set the Operation in the store
        })
    },

    insertOperation(context, payload) {
      axios.post('Operation/InsertOperation', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertOperation', { item: response.data.result }) //Insert the Operation in the store
          }
        })
    },

    deleteOperation(context, payload) {
      axios.post('Operation/DeleteOperation/' + payload.id)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('deleteOperation', { payload }); //Delete the Operation in the store
          }
        })
    },

    updateOperation(context, payload) {
      axios.post('Operation/UpdateOperation', payload)
        .then(response => {
          context.commit('updateOperation', response.data.result) //Update the Operation in the store
        })
    },

    getAllOperationTypes(context) {
      axios.get('OperationType/GetAllOperationTypes').then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setAllOperationType', response.data.result) //Set the Operation Types in the store
        }
      })
    }
  }
}

export default operationModule;
