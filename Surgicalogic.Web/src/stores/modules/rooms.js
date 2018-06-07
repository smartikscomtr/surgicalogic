import axios from 'axios';
const roomModule = {
    state: { 
        rooms: []
    },
    mutations: {
        setRooms(state, rooms) {
            state.rooms = rooms;
          }
    },  
    getters: {},
    actions: {
        getRooms(context) {
            axios.get('http://localhost:8080/static/rooms.json')
              .then(response => {        
                context.commit('setRooms', response.data) // set the Rooms in the store
              })
          }
    }
  }

  export default roomModule;


