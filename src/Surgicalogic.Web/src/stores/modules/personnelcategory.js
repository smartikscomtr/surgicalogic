import axios from 'axios';

const personnelCategoryModule = {
  state: {
    personnelCategory: [],
    loading: false,
    totalCount: 0
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setPersonnelCategories(state, data) {
      state.personnelCategory = data.result;
      state.totalCount = data.totalCount;
    },

    insertPersonnelCategory(state, { item }) {
      state.personnelCategory.push(item);
    },

    deletePersonnelCategory(state, { payload }) {
      let index = state.personnelCategory.findIndex((item) => {
        return item.id === payload.id
      });

      state.personnelCategory.splice(index, 1);
    },

    updatePersonnelCategory(state, payload) {
      state.personnelCategory.forEach(element => {
        if (element.id == payload.id)
          Object.assign(element, payload);
      });
    }
  },

  getters: {},

  actions: {
    getPersonnelCategories(context, params) {
      context.commit('setLoading', true);

      axios.get('PersonnelCategory/GetPersonnelCategories', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setPersonnelCategories', response.data) //Set the Personnel Title in the store
        }

        context.commit('setLoading', false);
      })

    },

    insertPersonnelCategory(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('PersonnelCategory/InsertPersonnelCategory', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('insertPersonnelCategory', {
                item: response.data.result
              }) //Insert the Personnel Title in the store
            }

            resolve(response);
          })
      });
    },

    deletePersonnelCategory(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('PersonnelCategory/DeletePersonnelCategory/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deletePersonnelCategory', {
                payload
              }); //Delete the Personnel Title in the store
            }

            resolve(response);
          })
      });
    },

    updatePersonnelCategory(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('PersonnelCategory/UpdatePersonnelCategory', payload)
          .then(response => {
            context.commit('updatePersonnelCategory', response.data.result) //Update the Personnel Title in the store
            resolve(response);
          })
      });
    },

    excelExportPersonnelCategory(context, payload) {
      axios.get('PersonnelCategory/ExcelExport?langId=' + payload.langId)
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          setTimeout(function() {
            link.click();
          }, 1000);        })
    }
  }
}

export default personnelCategoryModule;
