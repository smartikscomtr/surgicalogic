import axios from 'axios';

const personnelTitleModule = {
  state: {
    personnelTitle: [],
    totalCount:0,
    allPersonelTitles:[]
  },

  mutations: {
    setPersonnelTitles(state, data) {
      state.personnelTitle = data.result;
      state.totalCount = data.totalCount;
    },

    setAllPersonnelTitles(state, data) {
      state.allPersonelTitles = data;
    },

    insertPersonnelTitle(state, { item }) {
      state.personnelTitle.push(item);
    },

    deletePersonnelTitle(state, { payload }) {
      let index = state.personnelTitle.findIndex((item) => {
        return item.id === payload.id
      });

      state.personnelTitle.splice(index, 1);
    },

    updatePersonnelTitle(state, payload) {
      //state.personnelTitle.payload = payload;
    }
  },

  getters: {},

  actions: {
    getPersonnelTitles(context, payload) {
      axios.post('PersonnelTitle/GetPersonnelTitles', payload)
        .then(response => {
          context.commit('setPersonnelTitles', response.data) //Set the Personnel Title in the store
        })
    },

    getAllPersonnelTitles(context) {
      axios.get('PersonnelTitle/GetAllPersonnelTitles')
        .then(response => {
          context.commit('setAllPersonnelTitles', response.data.result) //Set the Personnel Title in the store
        })
    },

    insertPersonnelTitle(context, payload) {
      axios.post('PersonnelTitle/InsertPersonnelTitle', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            payload.id = response.data;

            context.commit('insertPersonnelTitle', { item: response.data.result }) //Insert the Personnel Title in the store
          }
        })
    },

    deletePersonnelTitle(context, payload) {
      axios.post('PersonnelTitle/DeletePersonnelTitle/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deletePersonnelTitle', { payload }); //Delete the Personnel Title in the store
          }
        })
    },

    updatePersonnelTitle(context, payload) {
      axios.post('PersonnelTitle/UpdatePersonnelTitle', payload)
        .then(response => {
          //context.commit('updatePersonnelTitle', {payload}) //Update the Personnel Title in the store
        })
    }
  }
}

export default personnelTitleModule;
