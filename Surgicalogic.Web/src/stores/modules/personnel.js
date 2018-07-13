import axios from 'axios';

const personnelModule = {
  state: {
    personnel: []
  },

  mutations: {
    setPersonnels(state, personnel) {
      state.personnel = personnel;
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
      state.personnel = payload;
    }
  },

  getters: {},

  actions: {
    getPersonnels(context) {
      axios.get('Personnel/GetPersonnels')
        .then(response => {
          if (response.data.info.succeeded == true){
            context.commit('setPersonnels', response.data.result) //Set the Personnel in the store
          }
        })
    },

    insertPersonnel(context, payload) {
      axios.post('Personnel/InsertPersonnel', payload)
        .then(response => {
          if (response.statusText == true) {
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
          context.commit('updatePersonnel', response.data.result) //Update the Personnel in the store
        })
    }
  }
}

export default personnelModule;
