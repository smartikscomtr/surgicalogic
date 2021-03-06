import axios from 'axios';

const operationTypeModule = {
  state: {
    loading: false,
    totalCount: 0,
    operationTypes: [],
    allBranches: [],
    allEquipments: [],
    allOperatingRooms: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setOperationTypes(state, data) {
      state.operationTypes = data.result;
      state.totalCount = data.totalCount;
    },

    insertOperationType(state, { item }) {
      state.operationTypes.push(item);
    },

    deleteOperationType(state, { payload }) {
      let index = state.operationTypes.findIndex((item) => {
        return item.id === payload.id
      });

      state.operationTypes.splice(index, 1);
    },

    updateOperationType(state, payload) {
      state.operationTypes.forEach(element => {
        if (payload && element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setAllBranchesForOperationType(state, payload) {
      state.allBranches = payload;
    },

    setAllEquipmentsForOperationType(state, payload) {
      state.allEquipments = payload;
    },

    setAllOperatingRoomsForOperationType(state, payload) {
      state.allOperatingRooms = payload;
    }
  },

  getters: {},

  actions: {
    getOperationTypes(context, params) {
      context.commit('setLoading', true);

      axios.get('OperationType/GetOperationTypes', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setOperationTypes', response.data) //Set the Operation Types in the store
        }

        context.commit('setLoading', false);
      })

    },

    insertOperationType(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('OperationType/InsertOperationType', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data.result.id;
              context.commit('insertOperationType', {
                item: payload
              }) //Insert the Operation Types in the store
            }

            resolve(response);
          })
      });
    },

    deleteOperationType(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('OperationType/DeleteOperationType/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deleteOperationType', {
                payload
              }); //Delete the Operation Types in the store
            }

            resolve(response);
          })
      });
    },

    updateOperationType(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('OperationType/UpdateOperationType', payload)
          .then(response => {
            context.commit('updateOperationType', response.data.result) //Update the OperationTypes in the store
            resolve(response);
          })
      });
    },

    getAllBranchesForOperationType(context) {
      axios.get('Branch/GetAllBranches')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllBranchesForOperationType', response.data.result) //Set the All Branches in the store
          }
        })
    },

    getAllEquipmentsForOperationType(context) {
      axios.get('Equipment/GetAllEquipments')
        .then(response => {
          context.commit('setAllEquipmentsForOperationType', response.data.result) //Set the All Equipments in the store
        })
    },

    getAllOperatingRoomsForOperationType(context) {
      axios.get('OperatingRoom/GetAllOperatingRooms')
        .then(response => {
          context.commit('setAllOperatingRoomsForOperationType', response.data.result) //Set the All Operating Rooms in the store
        })
    },

    exportOperationTypeToExcel(context, payload) {
      axios.get('OperationType/ExcelExport?langId='+ payload.langId)
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          setTimeout(function() {
            link.click();
          }, 1000);        })
    }
  }
}

export default operationTypeModule;
