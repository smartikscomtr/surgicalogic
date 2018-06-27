import axios from 'axios';

const equipmentModule = {
    state: { equipments: []},
    mutations: {
        setEquipments(state, equipments) {
            state.equipments = equipments;
        },
        updateEquipment(state, payload){
          state.equipments.payload = payload;
        }
    },
    getters: {},
    actions: {
        getEquipments(context) {
          axios.get('http://localhost:6632/Equipment/GetEquipments')
              .then(response => {
                context.commit('setEquipments', response.data.result) // set the Equipments in the store
          })
        },
        updateEquipment(context, payload){
          axios.delete('http://localhost:6632/Equipment/UpdateEquipment',{ equipment : payload })
              .then(response => {
                context.commit('updateEquipment', response.data.result) // update the Equipments in the store
          })
        }    
    }
  }

  export default equipmentModule;
