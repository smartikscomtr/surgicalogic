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
        axios.get('http://localhost:6632/Personnel/GetPersonnels')
          .then(response => {
            context.commit('setPersonnels', response.data.result) // set the Personnel in the store
          })
      },

      insertPersonnel(context, payload) {
        axios.post('http://localhost:6632/Personnel/InsertPersonnel', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data;

              context.commit('insertPersonnel', { item: response.data.result }) // insert the Personnel in the store
            }
          })
      },

      deletePersonnel(context, payload) {
        axios.post('http://localhost:6632/Personnel/DeletePersonnel/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deletePersonnel', { payload }); // delete the Personnel in the store
            }
          })
      },

      updatePersonnel(context, payload) {
        axios.post('http://localhost:6632/Personnel/UpdatePersonnel',  payload)
          .then(response => {
            //context.commit('updatePersonnel', {payload}) // update the Personnel in the store
          })
      }
    }
  }

  export default personnelModule;
