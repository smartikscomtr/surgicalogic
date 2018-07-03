import axios from 'axios';

const operatingRoomModule = {
    state: {
      operatingRooms: []
    },

    mutations: {
      setOperatingRooms(state, operatingRooms) {
        state.operatingRooms = operatingRooms;
      },

      insertOperatingRooms(state, { item }) {
        state.operatingRooms.push(item);
      },

      deleteOperatingRooms(state, { payload }) {
        let index = state.operatingRooms.findIndex((item) => {
          return item.id === payload.id
        });
        state.operatingRooms.splice(index, 1);
      },

      updateOperatingRooms(state, payload) {
        //state.operatingRooms.payload = payload;
      }
    },

    getters: {},

    actions: {
      getOperatingRooms(context) {
        axios.get('http://localhost/Surgicalogic.Api/OperatingRoom/GetOperatingRooms')
            .then(response => {
              context.commit('setOperatingRooms', response.data.result) // set the OperatingRooms in the store
          })
      },

      insertOperatingRoom(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/OperatingRoom/InsertOperatingRoom', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data;

              context.commit('insertOperatingRoom', { item: payload }) // insert the OperatingRooms in the store
            }
          })
      },

      deleteOperatingRooms(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/OperatingRoom/DeleteOperatingRoom/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteOperatingRoom', { payload }); // delete the OperatingRooms in the store
            }
          })
      },

      updateOperatingRooms(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/OperatingRoom/UpdateOperatingRoom',  payload)
          .then(response => {
            //context.commit('updateOperatingRoom', {payload}) // update the OperatingRooms in the store
          })
      }
    }
  }

  export default operatingRoomModule;
