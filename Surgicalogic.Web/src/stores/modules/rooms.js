import axios from 'axios';

const roomModule = {
    state: {
      rooms: []
    },

    mutations: {
      setRooms(state, rooms) {
        state.rooms = rooms;
      },

      insertRoom(state, { item }) {
        state.rooms.push(item);
      },

      deleteRoom(state, { payload }) {
        let index = state.rooms.findIndex((item) => {
          return item.id === payload.id
        });
        state.rooms.splice(index, 1);
      },

      updateRoom(state, payload) {
        //state.rooms.payload = payload;
      }
    },

    getters: {},

    actions: {
      getRooms(context) {
        axios.get('http://localhost/Surgicalogic.Api/Room/GetRooms')
            .then(response => {
              context.commit('setRooms', response.data.result) // set the Rooms in the store
          })
      },

      insertRoom(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/Room/InsertRoom', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data;

              context.commit('insertRoom', { item: payload }) // insert the Rooms in the store
            }
          })
      },

      deleteRoom(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/Room/DeleteRoom/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteRoom', { payload }); // delete the Rooms in the store
            }
          })
      },

      updateRoom(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/Room/UpdateRoom',  payload)
          .then(response => {
            //context.commit('updateRoom', {payload}) // update the Rooms in the store
          })
      }
    }
  }

  export default roomModule;
