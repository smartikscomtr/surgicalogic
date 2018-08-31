import axios from 'axios';

const personnelModule = {
  state: {
    loading: false,
    totalCount:0,
    personnel: [],
    allBranches: [],
    allPersonnelTitles: [],
    allWorkTypes: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

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
      state.personnel.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setAllBranches(state, payload) {
      state.allBranches = payload;
    },

    setAllPersonnelTitles(state, payload) {
      state.allPersonnelTitles = payload;
    },

    setAllWorkTypes(state, payload) {
      state.allWorkTypes = payload;
    }
  },

  getters: {},

  actions: {
    getPersonnels(context, params) {
      context.commit('setLoading', true);

      axios.get('Personnel/GetPersonnels', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setPersonnels', response.data) //Set the Personnel in the store
        }

        context.commit('setLoading', false);
      })

    },

    insertPersonnel(context, payload) {
      axios.post('Personnel/InsertPersonnel', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertPersonnel', { item: response.data.result }) //Insert the Personnel in the store
          }
        })
    },

    deletePersonnel(context, payload) {
      axios.post('Personnel/DeletePersonnel/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK' && response.data.info.succeeded == true) {
            context.commit('deletePersonnel', { payload }); //Delete the Personnel in the store
          }
        })
    },

    updatePersonnel(context, payload) {
      axios.post('Personnel/UpdatePersonnel', payload)
        .then(response => {
          context.commit('updatePersonnel', response.data.result) //Update the Personnel in the store
        })
    },

    getAllBranches(context) {
      axios.get('Branch/GetAllBranches')
      .then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setAllBranches', response.data.result) //Set the All Branches in the store
        }
      })
    },

    getAllPersonnelTitles(context) {
      axios.get('PersonnelTitle/GetAllPersonnelTitles')
      .then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setAllPersonnelTitles', response.data.result) //Set the All Personnel Titles in the store
        }
      })
    },

    getAllWorkTypes(context) {
      axios.get('WorkType/GetAllWorkTypes')
      .then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setAllWorkTypes', response.data.result) //Set the Work Types in the store
        }
      })
    },

    excelExportPersonnel(context) {
      axios.get('Personnel/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href =  "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    }
  }
}

export default personnelModule;
