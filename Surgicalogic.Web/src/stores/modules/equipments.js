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
        axios.get('http://localhost:6632/Equipment/GetEquipments')
          .then(response => {
            if (response.data.info.succeeded == true){
              context.commit('setEquipments', response.data.result) // set the Equipments in the store
            }
          })
      },

      insertEquipment(context, payload) {
        axios.post('http://localhost:6632/Equipment/InsertEquipment', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {
             // console.log(payload)
              context.commit('insertEquipment', { item: response.data.result }) // insert the Equipments in the store
            }
          })
      },

      deleteEquipment(context, payload) {
        axios.post('http://localhost:6632/Equipment/DeleteEquipment/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteEquipment', { payload }); // delete the Equipments in the store
            }
          })
      },

      updateEquipment(context, payload){
        axios.post('http://localhost:6632/Equipment/UpdateEquipment', payload)
            .then(response => {
              context.commit('updateEquipment', response.data.result) // update the Equipments in the store
        })
      },
      getAllEquipmentTypes(context) {
        axios.get('http://localhost:6632/EquipmentType/GetEquipmentTypes')
            .then(response => {
              context.commit('setAllEquipmentTypes', response.data.result) // set the Equipments in the store
          })
      }
    }
  }

  export default equipmentModule;
