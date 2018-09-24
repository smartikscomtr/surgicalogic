import axios from 'axios';

const personnelModule = {
  state: {
    loading: false,
    totalCount: 0,
    personnel: [],
    allPersonnels: [],
    allBranches: [],
    allPersonnelTitles: [],
    allWorkTypes: [],
    filteredDoctor: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setPersonnels(state, data) {
      state.personnel = data.result;
      state.totalCount = data.totalCount;
    },

    setAllPersonnels(state, data) {
      state.allPersonnels = data;
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
      state.personnel.forEach(element => {
        if (element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setAllBranchesForPersonnel(state, payload) {
      state.allBranches = payload;
    },

    setAllPersonnelTitlesForPersonnel(state, payload) {
      state.allPersonnelTitles = payload;
    },

    setAllWorkTypesForPersonnel(state, payload) {
      state.allWorkTypes = payload;
    },

    setDoctorsByBranchIdAsync(state, payload) {
      state.filteredDoctor = payload;
    }
  },

  getters: {},

  actions: {
    getPersonnels(context, params) {
      context.commit('setLoading', true);

      axios.get('Personnel/GetPersonnels', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setPersonnels', response.data) //Set the Personnel in the store
        }

        context.commit('setLoading', false);
      })
    },

    getAllPersonnels(context) {
      axios.get('Personnel/GetAllPersonnels')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllPersonnels', response.data.result) //Set the Personnels in the store
          }
        })
    },

    insertPersonnel(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Personnel/InsertPersonnel', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {
              context.commit('insertPersonnel', {
                item: response.data.result
              }) //Insert the Personnel in the store
            }

            resolve(response);
          })
      });
    },

    deletePersonnel(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Personnel/DeletePersonnel/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deletePersonnel', {
                payload
              }); //Delete the Personnel in the store
            }

            resolve(response);
          })
      });
    },

    updatePersonnel(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Personnel/UpdatePersonnel', payload)
          .then(response => {
            context.commit('updatePersonnel', response.data.result) //Update the Personnel in the store
            resolve(response);
          })
      });
    },

    getAllBranchesForPersonnel(context) {
      axios.get('Branch/GetAllBranches')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllBranchesForPersonnel', response.data.result) //Set the All Personnel in the store
          }
        })
    },

    getAllPersonnelTitlesForPersonnel(context) {
      axios.get('PersonnelTitle/GetAllPersonnelTitles')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllPersonnelTitlesForPersonnel', response.data.result) //Set the All Personnel Titles in the store
          }
        })
    },

    getAllWorkTypesForPersonnel(context) {
      axios.get('WorkType/GetAllWorkTypes')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllWorkTypesForPersonnel', response.data.result) //Set the Work Types in the store
          }
        })
    },

    getDoctorsByBranchIdAsync(context, payload) {
      axios.get('Personnel/GetDoctorsByBranchIdAsync/' + payload.branchId).then(response => {
        if (response.data) {
          context.commit('setDoctorsByBranchIdAsync', response.data)
        }
      })
    },

    excelExportPersonnel(context) {
      axios.get('Personnel/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    }
  }
}

export default personnelModule;
