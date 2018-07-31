import axios from 'axios';

const operatingRoomModule = {
  state: {
    loading: false,
    totalCount: 0,
    operatingRooms: [],
    allOperatingRooms: [],
    nonPortableEquipments: [],
    allOperationTypes: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setOperatingRooms(state, data) {
      state.operatingRooms = data.result;
      state.totalCount = data.totalCount;
    },

    setAllOperatingRooms(state, data) {
      state.allOperatingRooms = data;
    },

    setAllOperationTypes(state, data) {
      state.allOperationTypes = data;
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
      context.commit('setLoading', true);

      axios.get('OperatingRoom/GetOperatingRooms', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setOperatingRooms', response.data) //Set the Operating Rooms in the store
        }

        context.commit('setLoading', false);
      })

    },

    getAllOperatingRooms(context) {
      axios.get('OperatingRoom/GetAllOperatingRooms')
          .then(response => {
            context.commit('setAllOperatingRooms', response.data.result) //Set the Operating Rooms in the store
        })
    },

    insertOperatingRoom(context, payload) {
      axios.post('OperatingRoom/InsertOperatingRoom', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertOperatingRoom', { item: response.data.result }) //Insert the Operating Rooms in the store
          }
        })
    },

    deleteOperatingRoom(context, payload) {
      axios.post('OperatingRoom/DeleteOperatingRoom/' + payload.id)
        .then(response => {
          if (response.data.info.succeeded == true) {
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
    },

    getAllOperationTypes(context) {
      axios.get('OperationType/GetAllOperationTypes')
          .then(response => {
            context.commit('setAllOperationTypes', response.data.result) //Set the Operation Types in the store
        })
    },
  }
}

export default operatingRoomModule;
