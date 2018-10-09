import axios from 'axios';

const clinicModule = {
  state: {
    loading: false,
    totalCount: 0,
    model: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setAppointmentCalendarByDate(state, payload) {
      state.model = payload;
    }
  },

    getters: {},

    actions: {
    getAppointmentCalendarByDate(context, params) {
      context.commit('setLoading', true);

      return new Promise((resolve, reject) => {
        axios.get('AppointmentCalendar/GetAppointmentCalendarByDate', {
          params: params
        }).then(response => {
          if (response.statusText == 'OK'){
            context.commit('setAppointmentCalendarByDate', response.data.result)
          }

          context.commit('setLoading', false);
          resolve(response);
        }, error => {
          reject(error);
        })
      })
    }
  }
}

export default clinicModule;
