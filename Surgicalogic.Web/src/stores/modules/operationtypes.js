import axios from 'axios';

const operatingTypeModule = {
    state: {
      operatingTypes: []
    },

    mutations: {
      setOperatingTypes(state, operatingTypes) {
        state.operatingTypes = operatingTypes;
      },

      insertOperationType(state, { item }) {
        state.operatingTypes.push(item);
      },

      deleteOperationType(state, { payload }) {
        let index = state.operatingTypes.findIndex((item) => {
          return item.id === payload.id
        });
        state.operatingTypes.splice(index, 1);
      },

      updateOperationTypes(state, payload) {
        //state.operatingType.payload = payload;
      }
    },

    getters: {},

    actions: {
      getOperationTypes(context) {
        axios.get('http://localhost/Surgicalogic.Api/OperationType/GetOperationTypes')
            .then(response => {
              context.commit('setOperationTypes', response.data.result) // set the OperationTypes in the store
          })
      },

      insertOperationType(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/OperationType/InsertOperationType', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data;

              context.commit('insertOperationType', { item: payload }) // insert the OperationTypes in the store
            }
          })
      },

      deleteOperationType(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/OperationType/DeleteOperationType/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteOperationType', { payload }); // delete the OperationTypes in the store
            }
          })
      },

      updateOperationType(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/OperationType/UpdateOperationType',  payload)
          .then(response => {
            //context.commit('updateOperationType', {payload}) // update the OperationTypes in the store
          })
      }
    }
  }

  export default operatingTypeModule;
