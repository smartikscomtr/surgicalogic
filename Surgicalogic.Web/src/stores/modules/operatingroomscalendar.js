import axios from 'axios';

const operatingRoomCalenadarModule = {
  state: {
    operatingRoomCalendar: [],
    loading: false,
    totalCount: 0
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
      state.operatingRoomsCalendar.push(item);
    },

    deleteOperatingRoomCalendar(state, { payload }) {
      let index = state.operatingRoomCalendar.findIndex((item) => {
        return item.id === payload.id
      });

      state.operatingRoomsCalenadar.splice(index, 1);
    },

    updateOperatingRoomCalendar(state, payload) {
      state.operatingRoomCalendar.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    }
  },

  getters: {},

  actions: {
    getOperatingRoomsCalendar(context, params) {
      context.commit('setLoading', true);

      axios.get('OperatingRoomCalendar/GetOperatingRoomCalendar', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setOperatingRoomCalendar', response.data) //Set the Operating Rooms in the store
        }

        context.commit('setLoading', false);
      })

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
      axios.post('OperatingRoomCalendar/UpdateOperatingRoom', payload)
        .then(response => {
          context.commit('updateOperatingRoom', response.data.result) //Update the Operating Rooms in the store
        })
    }

   
  }
}

export default operatingRoomCalenadarModule;
