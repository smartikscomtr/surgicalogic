import axios from 'axios';

const personnelTitleModule = {
  state: {
    personnelTitle: [],
    loading: false,
    totalCount: 0
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
        if (element.id == payload.id)
          Object.assign(element, payload);
      });
    }
  },

  getters: {},

  actions: {
    getPersonnelTitles(context, params) {
      context.commit('setLoading', true);

      axios.get('PersonnelTitle/GetPersonnelTitles', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setPersonnelTitles', response.data) //Set the Personnel Title in the store
        }

        context.commit('setLoading', false);
      })

    },

    insertPersonnelTitle(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('PersonnelTitle/InsertPersonnelTitle', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('insertPersonnelTitle', {
                item: response.data.result
              }) //Insert the Personnel Title in the store
            }

            resolve(response);
          })
      });
    },

    deletePersonnelTitle(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('PersonnelTitle/DeletePersonnelTitle/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deletePersonnelTitle', {
                payload
              }); //Delete the Personnel Title in the store
            }

            resolve(response);
          })
      });
    },

    updatePersonnelTitle(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('PersonnelTitle/UpdatePersonnelTitle', payload)
          .then(response => {
            context.commit('updatePersonnelTitle', response.data.result) //Update the Personnel Title in the store
            resolve(response);
          })
      });
    },

    excelExportPersonnelTitle(context) {
      axios.get('PersonnelTitle/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    }
  }
}

export default personnelTitleModule;
