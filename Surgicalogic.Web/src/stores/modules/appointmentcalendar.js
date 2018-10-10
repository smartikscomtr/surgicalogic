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
      state.appointments = payload;
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
    },

    getFutureAppointments(context, payload) {
      axios.get('AppointmentCalendar/GetFutureAppointmentListAsync/' + payload.doctorId)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('setFutureAppointments', response.data) //Set the  getFutureAppointments in the store
          }
        })
    },

    // deletePersonnel(context, payload) {
    //   return new Promise((resolve, reject) => {
    //     axios.post('Personnel/DeletePersonnel/' + payload.id)
    //       .then(response => {
    //         if (response.statusText == 'OK' && response.data.info.succeeded == true) {
    //           context.commit('deletePersonnel', {
    //             payload
    //           }); //Delete the Personnel in the store
    //         }

    //         resolve(response);
    //       })
    //   });
    // },
  }
}

export default appointmentCalendarModule;
