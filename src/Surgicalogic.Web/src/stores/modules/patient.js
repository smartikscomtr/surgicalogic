import axios from 'axios';

const patientModule = {
  state: {
    loading: false,
    totalCount: 0,
    patient: [],
    allPatients: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setPatient(state, data) {
      state.patient = data.result;
      state.totalCount = data.totalCount;
    },

    setAllPatients(state, data) {
      state.allPatients = data;
    },

    insertPatient(state, { item }) {
      state.patient.push(item);
    },

    deletePatient(state, { payload }) {
      let index = state.patient.findIndex((item) => {
        return item.id === payload.id
      });

      state.patient.splice(index, 1);
    },

    updatePatient(state, payload) {
      state.patient.forEach(element => {
        if (element.id == payload.id)
          Object.assign(element, payload);
      });
    }
  },

  getters: {},

  actions: {
    getPatients(context, params) {
      context.commit('setLoading', true);
      axios.get('Patient/GetPatients', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setPatient', response.data) //Set the Patient in the store
        }

        context.commit('setLoading', false);
      })

    },

    getAllPatients(context) {
      axios.get('Patient/GetAllPatients')
        .then(response => {
          context.commit('setAllPatients', response.data.result) //Set the Patient in the store
        })
    },

    getAllPatientsForOperation(context) {
      axios.get('Patient/GetAllPatientsForOperation')
        .then(response => {
          context.commit('setAllPatients', response.data.result) //Set the Patient in the store
        })
    },

    getPatientById(context, payload) {
      return new Promise((resolve, reject) => {
        axios.get('Patient/GetPatientById/' + payload)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('setPatientById', response.data.result); //Delete the Patient in the store
            }

            resolve(response);
          })
      });
    },

    insertPatient(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Patient/InsertPatient', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {
              context.commit('insertPatient', {
                item: response.data.result
              }) //Insert the Patient in the store
            }

            resolve(response);
          })
      });
    },

    deletePatient(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Patient/DeletePatient/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deletePatient', {
                payload
              }); //Delete the Patient in the store
            }

            resolve(response);
          })
      });
    },

    updatePatient(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Patient/UpdatePatient', payload)
          .then(response => {
            if (response.data.info.succeeded){
              context.commit('updatePatient', response.data.result) //Update the Patient in the store
            }

            resolve(response);
          })
      });
    },

    excelExportPatient(context, payload) {
      axios.get('Patient/ExcelExport?langId=' + payload.langId)
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

export default patientModule;
