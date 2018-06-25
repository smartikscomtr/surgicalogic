import axios from 'axios';

const operationTypeModule = {
    state: { operationTypes: []},
    mutations: {
        setOperationTypes(state, operationTypes) {
            state.operationTypes = operationTypes;
          }
    },
    getters: {},
    actions: {
        getOperationTypes(context) {
            axios.get('http://localhost:8080/static/operationtypes.json')
              .then(response => {
                context.commit('setOperationTypes', response.data) // set the OperationTypes in the store
              })
            }
    }
  }

  export default operationTypeModule;
