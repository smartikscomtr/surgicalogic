import axios from 'axios';

const appointmentCalendarModule = {
  state: {
    loading: false,
    totalCount: 0,
    model: [],
    appointments: []
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
    },

    setFutureAppointments(state, payload) {
      state.appointments = payload.result;
      state.totalCount = payload.totalCount;
    },

    deleteAppointment(state, { payload }) {
      let index = state.appointments.findIndex((item) => {
        return item.id === payload.id
      });
      state.appointments.splice(index, 1);
    },
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
    },

    getFutureAppointments(context, payload) {
      axios.get('AppointmentCalendar/GetFutureAppointmentListAsync', {
        params:payload
      }).then(response => {
          if (response.statusText == 'OK') {
            context.commit('setFutureAppointments', response.data) //Set the  getFutureAppointments in the store
          }
        })
    },

    deleteAppointment(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('AppointmentCalendar/DeleteAppointment/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deleteAppointment', {
                payload
              });
            }

            resolve(response);
          })
      });
    },
  }
}

export default appointmentCalendarModule;
