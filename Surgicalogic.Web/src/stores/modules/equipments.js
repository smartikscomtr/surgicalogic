import axios from 'axios';

const equipmentModule = {
    state: { equipments: []},
    mutations: {
        setEquipments(state, equipments) {
            state.equipments = equipments;
      },
      insertEquipment(state, { item }) {
        state.equipments.push(item);
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
      insertEquipment(context, payload) {

        axios.post('http://localhost:6632/Equipment/InsertEquipment', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data;

              context.commit('insertEquipment', { item: payload }) // insert the Equipments in the store
            }
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
