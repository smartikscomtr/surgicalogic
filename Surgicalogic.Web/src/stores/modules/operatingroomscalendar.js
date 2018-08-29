import axios from 'axios';

const operatingRoomCalendarModule = {
  state: {
    operatingRoomCalendar: [],
    operatingRoomId:0,
    loading: false,
    startDate: null,
    endDate: null,
    totalCount: 0,
    excelUrl: null
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setOperatingRoomCalendar(state, data) {
      state.operatingRoomCalendar = data.result;
      state.totalCount = data.totalCount;
    },

    insertOperatingRoomCalendar(state, { item }) {
      state.operatingRoomCalendar.push(item);
    },

    deleteOperatingRoomCalendar(state, { payload }) {
      let index = state.operatingRoomCalendar.findIndex((item) => {
        return item.id === payload.id
      });

      state.operatingRoomCalendar.splice(index, 1);
    },

    updateOperatingRoomCalendar(state, payload) {
      state.operatingRoomCalendar.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    saveStartDate(state, newValue){
      state.startDate = newValue;
    },

    saveEndDate(state, newValue){
      state.endDate = newValue;
    },

    excelExport(state, data) {
      state.excelUrl = "/static/" + data;
    }
  },

  getters: {},

  actions: {
    getOperatingRoomsCalendar(context, params) {
      if (params.operatingRoomId)
      {
        context.commit('setLoading', true);

          axios.get('OperatingRoomCalendar/GetOperatingRoomCalendar', {
          params: params
          }).then(response => {
          if (response.data.info.succeeded == true){
            context.commit('setOperatingRoomCalendar', response.data) //Set the Operating Rooms in the store
          }

          context.commit('setLoading', false);
        })
    }

    },

    insertOperatingRoomCalendar(context, payload) {
      axios.post('OperatingRoomCalendar/InsertOperatingRoomCalendar', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertOperatingRoomCalendar', { item: response.data.result }) //Insert the Operating Rooms in the store
          }
        })
    },

    deleteOperatingRoomCalendar(context, payload) {
      axios.post('OperatingRoomCalendar/DeleteOperatingRoomCalendar/' + payload.id)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('deleteOperatingRoomCalendar', { payload }); //Delete the Operating Rooms in the store
          }
        })
    },

    updateOperatingRoomCalendar(context, payload) {
      axios.post('OperatingRoomCalendar/UpdateOperatingRoomCalendar', payload)
        .then(response => {
          context.commit('updateOperatingRoomCalendar', response.data.result) //Update the Operating Rooms in the store
        })
    },

    excelExportOperatingRoomCalendar(context, payload) {
      axios.get('OperatingRoomCalendar/ExcelExport/' +  payload.id)
        .then(response => {
          context.commit('excelExport', response.data) //Excel Export the Operating Room Calendar in the store
        })
    }
  }
}

export default operatingRoomCalendarModule;
