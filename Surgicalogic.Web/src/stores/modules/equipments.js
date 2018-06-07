import axios from 'axios';
const equipmentModule = {
    state: { equipments: []},
    mutations: {
        setEquipments(state, equipments) {
            state.equipments = equipments;
          }
    },  
    getters: {},
    actions: {
        getEquipments(context) {      
            axios.get('http://localhost:8080/static/equipments.json')
              .then(response => {        
                context.commit('setEquipments', response.data) // set the Equipments in the store
              })
            }
    }
  }

  export default equipmentModule;