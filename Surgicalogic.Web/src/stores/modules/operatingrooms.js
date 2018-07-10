import axios from 'axios';

const operatingRoomModule = {
    state: {
      operatingRooms: []
    },

    mutations: {
      setOperatingRooms(state, operatingRooms) {
        state.operatingRooms = operatingRooms;
      },

      insertOperatingRoom(state, { item }) {
        state.operatingRooms.push(item);
      },

      deleteOperatingRoom(state, { payload }) {
        let index = state.operatingRooms.findIndex((item) => {
          return item.id === payload.id
        });
        state.operatingRooms.splice(index, 1);
      },

      updateOperatingRoom(state, payload) {
        //state.operatingRooms.payload = payload;
      }
    },

    getters: {},

    actions: {
      getOperatingRooms(context) {
        axios.get('http://localhost:6632/OperatingRoom/GetOperatingRooms')
            .then(response => {
              context.commit('setOperatingRooms', response.data.result) // set the OperatingRooms in the store
          })
      },

      insertOperatingRoom(context, payload) {
        axios.post('http://localhost:6632/OperatingRoom/InsertOperatingRoom', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data;

              context.commit('insertOperatingRoom', { item: payload }) // insert the OperatingRooms in the store
            }
          })
      },

      deleteOperatingRoom(context, payload) {
        axios.post('http://localhost:6632/OperatingRoom/DeleteOperatingRoom/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteOperatingRoom', { payload }); // delete the OperatingRooms in the store
            }
          })
      },

      updateOperatingRoom(context, payload) {
        axios.post('http://localhost:6632/OperatingRoom/UpdateOperatingRoom',  payload)
          .then(response => {
            //context.commit('updateOperatingRoom', {payload}) // update the OperatingRooms in the store
          })
      }
    }
  }

  export default operatingRoomModule;
