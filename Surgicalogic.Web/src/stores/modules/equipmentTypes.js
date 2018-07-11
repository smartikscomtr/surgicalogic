import axios from 'axios';

const equipmentTypesModule = {
  state: {
    equipmentTypes: []
  },

  mutations: {
    setEquipmentTypes(state, equipmentTypes) {
      state.equipmentTypes = equipmentTypes;
    },

    insertEquipmentType(state, { item }) {
      state.equipmentTypes.push(item);
    },

    deleteEquipmentType(state, { payload }) {
      let index = state.equipmentTypes.findIndex((item) => {
        return item.id === payload.id
      });

      state.equipmentTypes.splice(index, 1);
    },

    updateEquipmentType(state, payload) {
    }
  },

  getters: {},

  actions: {
    getEquipmentTypes(context) {
      axios.get('EquipmentType/GetEquipmentTypes')
        .then(response => {
          context.commit('setEquipmentTypes', response.data.result) // set the Equipment Types in the store
        })
    },

    insertEquipmentType(context, payload) {
      axios.post('EquipmentType/InsertEquipmentType', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('insertEquipmentType', { item: response.data.result }) // insert the Equipment Types in the store
          }
        })
    },

    deleteEquipmentType(context, payload) {
      axios.post('EquipmentType/DeleteEquipmentType/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteEquipmentType', { payload }); // delete the Equipment Types in the store
          }
        })
    },

    updateEquipmentType(context, payload) {
      axios.post('EquipmentType/UpdateEquipmentType',  payload)
        .then(response => {
          //context.commit('updateEquipmentType', {payload}) // update the Equipment Types in the store
        })
    }
  }
}

export default equipmentTypesModule;
