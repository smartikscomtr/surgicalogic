import axios from 'axios';

const equipmentTypesModule = {
  state: {
    loading: false,
    totalCount:0,
    equipmentTypes: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setEquipmentTypes(state, data) {
      state.equipmentTypes = data.result;
      state.totalCount = data.totalCount;
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
      state.equipmentTypes.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    }
  },

  getters: {},

  actions: {
    getEquipmentTypes(context, params) {
      context.commit('setLoading', true);

      axios.get('EquipmentType/GetEquipmentTypes', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setEquipmentTypes', response.data) //Set the Equipment Types in the store
        }

        context.commit('setLoading', false);
      })

    },

    insertEquipmentType(context, payload) {
      axios.post('EquipmentType/InsertEquipmentType', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('insertEquipmentType', { item: response.data.result }) //Insert the Equipment Types in the store
          }
        })
    },

    deleteEquipmentType(context, payload) {
      axios.post('EquipmentType/DeleteEquipmentType/' + payload.id)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('deleteEquipmentType', { payload }); //Delete the Equipment Types in the store
          }
        })
    },

    updateEquipmentType(context, payload) {
      axios.post('EquipmentType/UpdateEquipmentType', payload)
        .then(response => {
          context.commit('updateEquipmentType', payload) //Update the Equipment Types in the store
        })
    },

    excelExportEquipmentTypes(context) {
      axios.get('EquipmentType/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href =  "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    }
  }
}

export default equipmentTypesModule;
