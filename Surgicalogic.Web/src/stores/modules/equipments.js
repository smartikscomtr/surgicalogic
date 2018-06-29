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
      deleteEquipment(state, { payload }) {
                
        let index = state.equipments.findIndex((item) => {
          return item.id === payload.id
        });
        state.equipments.splice(index, 1);
      },
      updateEquipment(state, payload) {
        console.log(payload)
        //state.equipments.payload = payload;
        state.equipments.findIndex((item) => {
          return item.equipmentTypeId === payload.equipmentTypeId
        });
      }
    },
    getters: {},
    actions: {
        getEquipments(context) {
          
          axios.get('http://localhost/Surgicalogic.Api/Equipment/GetEquipments')
              .then(response => {                
                if (response.data.info.succeeded == true){
                  context.commit('setEquipments', response.data.result) // set the Equipments in the store
                }
          })

      },
      insertEquipment(context, payload) {
        
        axios.post('http://localhost/Surgicalogic.Api/Equipment/InsertEquipment', payload)
          .then(response => {            
            if (response.data.info.succeeded == true) {
              context.commit('insertEquipment', { item: response.data.result }) // insert the Equipments in the store
            }            
          })

      },
      deleteEquipment(context, payload) {

        axios.post('http://localhost/Surgicalogic.Api/Equipment/DeleteEquipment/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteEquipment', { payload }); // delete the Equipments in the store
            }              
          })

      },
      updateEquipment(context, payload){
        axios.post('http://localhost/Surgicalogic.Api/Equipment/UpdateEquipment', payload)
            .then(response => {
              context.commit('updateEquipment', response.data.result) // update the Equipments in the store
        })
      }    
    }
  }

  export default equipmentModule;
