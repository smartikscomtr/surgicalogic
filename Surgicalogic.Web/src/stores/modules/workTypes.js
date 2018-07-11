import axios from 'axios';

const workTypesModule = {
  state: {
    workTypes: []
  },

  mutations: {
    setWorkTypes(state, workTypes) {
      state.workTypes = workTypes;
    },

    insertWorkType(state, { item }) {
      state.workTypes.push(item);
    },

    deleteWorkType(state, { payload }) {
      let index = state.workTypes.findIndex((item) => {
        return item.id === payload.id
      });

      state.workTypes.splice(index, 1);
    },

    updateWorkType(state, payload) {
      //state.workTypes.payload = payload;
    }
  },

  getters: {},

  actions: {
    getWorkTypes(context) {
      axios.get('WorkType/GetWorkTypes')
        .then(response => {
          if (response.data.info.succeeded == true){
            context.commit('setWorkTypes', response.data.result) //Set the Work Type in the store
          }
        })
    },

    insertWorkType(context, payload) {
      axios.post('WorkType/InsertWorkType', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertWorkType', { item: response.data.result }) //Insert the Work Type in the store
          }
        })
    },

    deleteWorkType(context, payload) {
      axios.post('WorkType/DeleteWorkType/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteWorkType', { payload }); //Delete the Work Type in the store
          }
        })
    },

    updateWorkType(context, payload) {
      axios.post('WorkType/UpdateWorkType', payload)
        .then(response => {
          //context.commit('updateWorkType', {payload}) //Update the Work Type in the store
        })
    }
  }
}

export default workTypesModule;
