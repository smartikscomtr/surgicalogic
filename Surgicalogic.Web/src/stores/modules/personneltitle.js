import axios from 'axios';

const personnelTitleModule = {
  state: {
    personnelTitle: [],
    loading: false,
    totalCount:0,
    excelUrl: null
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setPersonnelTitles(state, data) {
      state.personnelTitle = data.result;
      state.totalCount = data.totalCount;
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
      state.personnelTitle.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    excelExport(state, data) {
      state.excelUrl = "/static/" + data;
    }
  },

  getters: {},

  actions: {
    getPersonnelTitles(context, params) {
      context.commit('setLoading', true);

      axios.get('PersonnelTitle/GetPersonnelTitles', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setPersonnelTitles', response.data) //Set the Personnel Title in the store
        }

        context.commit('setLoading', false);
      })

    },

    insertPersonnelTitle(context, payload) {
      axios.post('PersonnelTitle/InsertPersonnelTitle', payload)
        .then(response => {
          if (response.statusText == 'OK') {
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
          context.commit('updatePersonnelTitle', response.data.result) //Update the Personnel Title in the store
        })
    },

    excelExportPersonnelTitle(context) {
      axios.get('PersonnelTitle/ExcelExport')
        .then(response => {
          context.commit('excelExport', response.data) //Excel Export the Personnel Title in the store
        })
    }
  }
}

export default personnelTitleModule;
