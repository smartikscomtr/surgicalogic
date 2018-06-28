import axios from 'axios';

const workTypesModule = {
  state: {
    workTypes: []
  },
  mutations: {
    setworkTypes(state, workTypes) {
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
      axios.get('http://localhost/Surgicalogic.Api/WorkType/GetWorkTypes')
          .then(response => {
            context.commit('setworkTypes', response.data.result) // set the WorkType in the store
        })

    },

    insertWorkType(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/WorkType/InsertWorkType', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            payload.id = response.data;

            context.commit('insertWorkType', { item: payload }) // insert the WorkType in the store
          }
        })

    },
    deleteWorkType(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/WorkType/DeleteWorkType/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteWorkType', { payload }); // delete the WorkType in the store
          }
        })

    },
    updateWorkType(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/WorkType/UpdateWorkType',  payload)
        .then(response => {
            //context.commit('updateWorkType', {payload}) // update the WorkType in the store
        })
    }
  }
}

export default workTypesModule;
