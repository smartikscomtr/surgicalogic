import axios from 'axios';

const equipmentTypesModule = {
    state: { equipmentTypes: []},
    mutations: {
        setEquipmentTypes(state, equipmentTypes) {
            state.equipmentTypes = equipmentTypes;
          }
    },
    getters: {},
    actions: {
        getEquipmentTypes(context) {
            axios.get('http://localhost:8080/static/equipmenttypes.json')
              .then(response => {
                context.commit('setEquipmentTypes', response.data) // set the Equipments in the store
              })
            }
    }
  }

  export default equipmentTypesModule;
