import axios from 'axios';

const equipmentTypesModule = {
    state: { equipmentTypes: [], allEquipmentTypes: []},
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
        //state.equipments.payload = payload;
      },
      setAllEquipmentTypes(state, payload) {
        state.allEquipmentTypes = payload;
      },
    },
    getters: {},
    actions: {
      getEquipmentTypes(context) {

        axios.get('http://localhost/Surgicalogic.Api/EquipmentType/GetEquipmentTypes')
            .then(response => {
              context.commit('setEquipmentTypes', response.data.result) // set the Equipments in the store
          })

      },
      insertEquipmentType(context, payload) {
        
        axios.post('http://localhost/Surgicalogic.Api/EquipmentType/InsertEquipmentType', payload)
          .then(response => {
            if (response.statusText == 'OK') {              
              context.commit('insertEquipmentType', { item: response.data.result }) // insert the Equipments in the store
            }            
          })

      },
      deleteEquipmentType(context, payload) {

        axios.post('http://localhost/Surgicalogic.Api/EquipmentType/DeleteEquipmentType/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteEquipmentType', { payload }); // delete the Equipments in the store
            }              
          })

      },
      updateEquipmentType(context, payload) {
        
        axios.post('http://localhost/Surgicalogic.Api/EquipmentType/UpdateEquipmentType',  payload)
          .then(response => {            
              //context.commit('updateEquipmentType', {payload}) // update the Equipments in the store                        
          })

      },
      getAllEquipmentTypes(context) {

        axios.get('http://localhost/Surgicalogic.Api/EquipmentType/GetEquipmentTypes')
            .then(response => {
              context.commit('setAllEquipmentTypes', response.data.result) // set the Equipments in the store
          })

      },
      
    }
  }

  export default equipmentTypesModule;
