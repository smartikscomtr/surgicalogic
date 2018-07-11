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
      axios.get('http://localhost/Surgicalogic.Api/EquipmentType/GetEquipmentTypes')
        .then(response => {
          context.commit('setEquipmentTypes', response.data.result) //Set the Equipment Types in the store
        })
    },

    insertEquipmentType(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/EquipmentType/InsertEquipmentType', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('insertEquipmentType', { item: response.data.result }) //Insert the Equipment Types in the store
          }
        })
    },

    deleteEquipmentType(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/EquipmentType/DeleteEquipmentType/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteEquipmentType', { payload }); //Delete the Equipment Types in the store
          }
        })
    },

    updateEquipmentType(context, payload) {
      axios.post('http://localhost/Surgicalogic.Api/EquipmentType/UpdateEquipmentType', payload)
        .then(response => {
          //context.commit('updateEquipmentType', {payload}) //Update the Equipment Types in the store
        })
    }
  }
}

export default equipmentTypesModule;
