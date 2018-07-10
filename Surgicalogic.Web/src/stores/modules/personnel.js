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
      //state.personnel.payload = payload;
    }
  },

  getters: {},

  actions: {
    getPersonnels(context) {
      axios.get('http://localhost/Surgicalogic.Api/Personnel/GetPersonnels')
        .then(response => {
          context.commit('setPersonnels', response.data.result) //Set the Personnel in the store
        })
    },

    insertPersonnel(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/Personnel/InsertPersonnel', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            payload.id = response.data;

            context.commit('insertPersonnel', { item: response.data.result }) //Insert the Personnel in the store
          }
        })
    },

    deletePersonnel(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/Personnel/DeletePersonnel/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deletePersonnel', { payload }); //Delete the Personnel in the store
          }
        })
    },

    updatePersonnel(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/Personnel/UpdatePersonnel', payload)
        .then(response => {
          //context.commit('updatePersonnel', {payload}) //Update the Personnel in the store
        })
    }
  }
}

export default personnelModule;
