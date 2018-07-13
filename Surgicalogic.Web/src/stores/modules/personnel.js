import axios from 'axios';

const personnelModule = {
  state: {
    personnel: [],
    totalCount:0
  },

  mutations: {
    setPersonnels(state, data) {
      state.personnel = data.result;
      state.totalCount = data.totalCount;
    },

    insertPersonnel(state, { item }) {
      state.personnel.push(item);
    },

    deletePersonnel(state, { payload }) {
      let index = state.personnel.findIndex((item) => {
        return item.id === payload.id
      });
      state.personnel.splice(index, 1);
    },

    updatePersonnel(state, payload) {
      //state.personnel.payload = payload;
    }
  },

  getters: {},

  actions: {
    getPersonnels(context, payload) {
      axios.post('Personnel/GetPersonnels', payload)
        .then(response => {
          context.commit('setPersonnels', response.data) //Set the Personnel in the store
        })
    },

    insertPersonnel(context, payload) {
      axios.post('Personnel/InsertPersonnel', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            payload.id = response.data;

            context.commit('insertPersonnel', { item: response.data.result }) //Insert the Personnel in the store
          }
        })
    },

    deletePersonnel(context, payload) {
      axios.post('Personnel/DeletePersonnel/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deletePersonnel', { payload }); //Delete the Personnel in the store
          }
        })
    },

    updatePersonnel(context, payload) {
      axios.post('Personnel/UpdatePersonnel', payload)
        .then(response => {
          //context.commit('updatePersonnel', {payload}) //Update the Personnel in the store
        })
    }
  }
}

export default personnelModule;
