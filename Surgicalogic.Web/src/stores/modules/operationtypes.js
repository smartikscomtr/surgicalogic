import axios from 'axios';

const operatingTypeModule = {
  state: {
    totalCount: 0,
    operationTypes: [],
    allBranches: [],
    allEquipments: [],
    allOperatingRooms: [],
    allPersonnels: []
  },

  mutations: {
    setOperationTypes(state, data) {
      state.operationTypes = data.result;
      state.totalCount = data.totalCount;
    },

    insertOperationType(state, { item }) {
      state.operatingTypes.push(item);
    },

    deleteOperationType(state, { payload }) {
      let index = state.operatingTypes.findIndex((item) => {
        return item.id === payload.id
      });

      state.operatingTypes.splice(index, 1);
    },

    updateOperationTypes(state, payload) {
      state.operatingTypes.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setAllBranches(state, payload) {
      state.allBranches = payload;
    },

    setAllEquipments(state, payload) {
      state.allEquipments = payload;
    },

    setAllOperatingRooms(state, payload) {
      state.allOperatingRooms = payload;
    },

    setAllPersonnels(state, payload) {
      state.allPersonnels = payload;
    }
  },

  getters: {},

  actions: {
    getOperationTypes(context, params) {
      axios.get('OperationType/GetOperationTypes', {
        params: params
      })
          .then(response => {
            context.commit('setOperationTypes', response.data) //Set the Operation Types in the store
        })
    },

    insertOperationType(context, payload) {
      axios.post('OperationType/InsertOperationType', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            payload.id = response.data;
            context.commit('insertOperationType', { item: payload }) //Insert the Operation Types in the store
          }
        })
    },

    deleteOperationType(context, payload) {
      axios.post('OperationType/DeleteOperationType/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteOperationType', { payload }); //Delete the Operation Types in the store
          }
        })
    },

    updateOperationType(context, payload) {
      axios.post('OperationType/UpdateOperationType',  payload)
        .then(response => {
          context.commit('updateOperationType', response.data.result) //Update the OperationTypes in the store
        })
    },

    getAllBranches(context) {
      axios.get('Branch/GetAllBranches')
        .then(response => {
          context.commit('setAllBranches', response.data.result) //Set the All Branches in the store
        })
    },

    getAllEquipments(context) {
      axios.get('Equipment/GetAllEquipments')
        .then(response => {
          context.commit('setAllEquipments', response.data.result) //Set the All Equipments in the store
        })
    },

    getAllOperatingRooms(context) {
      axios.get('OperatingRoom/GetAllOperatingRooms')
        .then(response => {
          context.commit('setAllOperatingRooms', response.data.result) //Set the All Operating Rooms in the store
        })
    },

    getAllPersonnels(context) {
      axios.get('Personnel/GetAllPersonnels')
      .then(response => {
        context.commit('setAllPersonnels', response.data.result) //Set the All Personnels in the store
      })
    }
  }
}

export default operatingTypeModule;
