import axios from 'axios';

const workTypesModule = {
  state: {
    workTypes: [],
    loading: false,
    totalCount: 0,
    allWorkTypes: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setWorkTypes(state, data) {
      state.workTypes = data.result;
      state.totalCount = data.totalCount;
    },

    setAllWorkTypes(state, data) {
      state.allWorkTypes = data;
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
      state.workTypes.forEach(element => {
        if (element.id == payload.id)
          Object.assign(element, payload);
      });
    }
  },

  getters: {},

  actions: {
    getWorkTypes(context, params) {
      context.commit('setLoading', true);

      axios.get('WorkType/GetWorkTypes', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setWorkTypes', response.data) //Set the Work Type in the store
        }

        context.commit('setLoading', false);
      })

    },

    getAllWorkTypes(context) {
      axios.get('WorkType/GetAllWorkTypes')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllWorkTypes', response.data.result) //Set the Work Type in the store
          }
        })
    },

    insertWorkType(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('WorkType/InsertWorkType', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {
              context.commit('insertWorkType', {
                item: response.data.result
              }) //Insert the Work Type in the store
            }

            resolve(response);
          })
      });
    },

    deleteWorkType(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('WorkType/DeleteWorkType/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deleteWorkType', {
                payload
              }); //Delete the Work Type in the store
            }

            resolve(response);
          })
      });
    },

    updateWorkType(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('WorkType/UpdateWorkType', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {
              context.commit('updateWorkType', payload) //Update the Work Type in the store
            }

            resolve(response);
          })
      });
    },

    excelExportWorkType(context, payload) {
      axios.get('WorkType/ExcelExport?langId=' + payload.langId)
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

export default workTypesModule;
