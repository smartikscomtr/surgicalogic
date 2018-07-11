import axios from 'axios';

const equipmentModule = {
  state: {
    equipments: [],
    allEquipmentTypes: []
  },

  mutations: {
    setEquipments(state, equipments) {
      state.equipments = equipments;
    },

    insertEquipment(state, { item }) {
      state.equipments.push(item);
    },

    deleteEquipment(state, { payload }) {
      let index = state.equipments.findIndex((item) => {
        return item.id === payload.id
      });

      state.equipments.splice(index, 1);
    },

    updateEquipment(state, payload) {
    },

    setAllEquipmentTypes(state, payload) {
      state.allEquipmentTypes = payload;
    }
  },

  getters: {},

  actions: {
    getEquipments(context) {
      axios.get('Equipment/GetEquipments')
        .then(response => {
          if (response.data.info.succeeded == true){
            context.commit('setEquipments', response.data.result) //Set the Equipments in the store
          }
        })
    },

    insertEquipment(context, payload) {
      axios.post('Equipment/InsertEquipment', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertEquipment', { item: response.data.result }) //Insert the Equipments in the store
          }
        })
    },

    deleteEquipment(context, payload) {
      axios.post('Equipment/DeleteEquipment/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteEquipment', { payload }); //Delete the Equipments in the store
          }
        })
    },

    updateEquipment(context, payload){
      axios.post('Equipment/UpdateEquipment', payload)
          .then(response => {
            context.commit('updateEquipment', response.data.result) //Update the Equipments in the store
      })
    },

    getAllEquipmentTypes(context) {
      axios.get('EquipmentType/GetEquipmentTypes')
        .then(response => {
          context.commit('setAllEquipmentTypes', response.data.result) //Set the All Equipments in the store
        })
    }
  }
}

export default equipmentModule;
