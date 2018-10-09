import axios from 'axios';

const appointmentCalendarModule = {
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
    },

    setAppointmentCalendar(state, payload) {
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
    },

    insertAppointmentCalendar(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('AppointmentCalendar/InsertAppointmentCalendar', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('setAppointmentCalendar', {
                item: response.data.result
              }) //Insert the AppointmentCalendar in the store
            }

            resolve(response);
          })
      });
    }
  }
}

export default appointmentCalendarModule;
