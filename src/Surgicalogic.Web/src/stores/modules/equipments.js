import axios from 'axios';

const equipmentModule = {
  state: {
    totalCount: 0,
    loading: false,
    equipments: [],
    allEquipmentTypes: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setEquipments(state, data) {
      state.equipments = data.result;
      state.totalCount = data.totalCount;
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
      if (payload){
        state.equipments.forEach(element => {
          if (element.id == payload.id)
            Object.assign(element, payload);
        });
      }
    },

    setAllEquipmentTypes(state, payload) {
      state.allEquipmentTypes = payload;
    }
  },

  getters: {},

  actions: {
    getEquipments(context, params) {
      context.commit('setLoading', true);

      axios.get('Equipment/GetEquipments', {
          params: params
        })
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setEquipments', response.data) //Set the Equipments in the store
          }

          context.commit('setLoading', false);
        })
    },

    insertEquipment(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Equipment/InsertEquipment', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {
              context.commit('insertEquipment', {
                item: response.data.result
              }) //Insert the Equipments in the store
            }

            resolve(response);
          })
      });
    },

    deleteEquipment(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Equipment/DeleteEquipment/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deleteEquipment', {
                payload
              }); //Delete the Equipments in the store
            }

            resolve(response);
          })
      });
    },

    updateEquipment(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Equipment/UpdateEquipment', payload)
          .then(response => {
            context.commit('updateEquipment', response.data.result) //Update the Equipments in the store
            resolve(response);
          })
      });
    },

    getAllEquipments(context) {
      axios.post('Equipment/GetAllEquipments')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllEquipments', response.data.result) //Set the Equipments in the store
          }
        })
    },

    getAllEquipmentTypes(context) {
      axios.get('EquipmentType/GetAllEquipmentTypes')
        .then(response => {
          context.commit('setAllEquipmentTypes', response.data.result) //Set the All Equipment Types in the store
        })
    },

    excelExportEquipments(context, payload) {
      axios.get('Equipment/ExcelExport?langId=' + payload.langId)
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          setTimeout(function() {
            link.click();
          }, 1000);
        })
    }
  }
}

export default equipmentModule;
