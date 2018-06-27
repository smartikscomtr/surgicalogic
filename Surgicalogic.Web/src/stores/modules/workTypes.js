import axios from 'axios';

const workTypesModule = {
  state: {
    workTypes: []
  },
  mutations: {
    setworkTypes(state, workTypes) {
      state.workTypes = workTypes;
    }
  },
  getters: {},
  actions: {
    getWorkTypes(context) {
      axios.get('http://localhost:8080/static/worktypes.json')
        .then(response => {
          context.commit('setworkTypes', response.data) // set the Equipments in the store
        })
    }
  }
}

export default workTypesModule;
