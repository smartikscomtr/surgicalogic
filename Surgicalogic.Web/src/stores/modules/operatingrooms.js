import axios from 'axios';

const operatingRoomModule = {
  state: {
    operatingRooms: [],
    totalCount: 0,
    nonPortableEquipments: []
  },

  mutations: {
    setOperatingRooms(state, data) {
      state.operatingRooms = data.result;
      state.totalCount = data.totalCount;
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
      state.operatingRooms.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setNonPortableEquipments(state, data) {
      state.nonPortableEquipments = data;
    }
  },

  getters: {},

  actions: {
    getOperatingRooms(context, params) {
      axios.get('OperatingRoom/GetOperatingRooms', {
        params: params
      })
        .then(response => {
          context.commit('setOperatingRooms', response.data) //Set the Operating Rooms in the store
      })
    },

    insertOperatingRoom(context, payload) {
      axios.post('OperatingRoom/InsertOperatingRoom', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            payload.id = response.data;

            context.commit('insertOperatingRoom', { item: payload }) //Insert the Operating Rooms in the store
          }
        })
    },

    deleteOperatingRoom(context, payload) {
      axios.post('OperatingRoom/DeleteOperatingRoom' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteOperatingRoom', { payload }); //Delete the Operating Rooms in the store
          }
        })
    },

    updateOperatingRoom(context, payload) {
      axios.post('OperatingRoom/UpdateOperatingRoom', payload)
        .then(response => {
          context.commit('updateOperatingRoom', response.data.result) //Update the Operating Rooms in the store
        })
    },

    getNonPortableEquipments(context) {
      axios.get('Equipment/GetNonPortableEquipments')
        .then(response => {
          context.commit("setNonPortableEquipments", response.data.result) //Set the Operating Rooms in the store
        })
    }
  }
}

export default operatingRoomModule;
