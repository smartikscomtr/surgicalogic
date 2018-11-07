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
    }
  }
}

export default patientModule;
