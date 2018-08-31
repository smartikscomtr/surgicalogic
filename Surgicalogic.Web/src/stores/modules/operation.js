import axios from 'axios';

const operationModule = {
  state: {
    loading: false,
    totalCount: 0,
    operation: [],
    allOperations: [],
    allOperationTypes: [],
    allBranches: [],
    filteredOperationTypes:[],
    filteredDoctors:[],
    filteredOperatingRooms: [],
    globalDate: null
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setOperation(state, data) {
      state.operation = data.result;
      state.totalCount = data.totalCount;
    },

    setAllOperations(state, data) {
      state.allOperations = data;
    },

    insertOperation(state, { item }) {
      state.operation.push(item);
    },

    deleteOperation(state, { payload }) {
      let index = state.operation.findIndex((item) => {
        return item.id === payload.id
      });

      state.operation.splice(index, 1);
    },

    updateOperation(state, payload) {
      state.operation.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setAllOperationType(state, payload) {
      state.allOperationTypes = payload;
    },

    setOperationTypesByBranchId(state,payload)
    {
      state.filteredOperationTypes = payload;
    },

    setDoctorsByBranchId(state,payload)
    {
      state.filteredDoctors = payload;
    },

    setOperatingRoomsByOperationTypeId(state, payload)
    {
      state.filteredOperatingRooms = payload;
    },

    setAllBranches(state, payload) {
      state.allBranches = payload;
    },

    saveGlobalDate(state, newValue){
      state.globalDate = newValue;
    }
  },

  getters: {},

  actions: {
    getOperations(context, params) {
      context.commit('setLoading', true);

      axios.get('Operation/GetOperations', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setOperation', response.data) //Set the Operation in the store
        }

        context.commit('setLoading', false);
      })

    },

    getAllOperations(context) {
      axios.get('Operation/GetAllOperations')
          .then(response => {
            context.commit('setAllOperations', response.data.result) //Set the Operation in the store
        })
    },

    insertOperation(context, payload) {
      axios.post('Operation/InsertOperation', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertOperation', { item: response.data.result }) //Insert the Operation in the store
          }
        })
    },

    deleteOperation(context, payload) {
      axios.post('Operation/DeleteOperation/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK' && response.data.info.succeeded == true) {
            context.commit('deleteOperation', { payload }); //Delete the Operation in the store
          }
        })
    },

    updateOperation(context, payload) {
      axios.post('Operation/UpdateOperation', payload)
        .then(response => {
          context.commit('updateOperation', response.data.result) //Update the Operation in the store
        })
    },

    getAllOperationTypes(context) {
      axios.get('OperationType/GetAllOperationTypes').then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setAllOperationType', response.data.result) //Set the Operation Types in the store
        }
      })
    },

    getOperationTypesByBranchId(context, payload) {
      axios.get('OperationType/GetOperationTypesByBranchId/'+ payload.branchId).then(response => {
        if (response.data) {
          context.commit('setOperationTypesByBranchId', response.data) //Set the Operation Types in the store
        }
      })
    },

    getDoctorsByBranchId(context, payload) {
      axios.get('Personnel/GetDoctorsByBranchIdAsync/'+ payload.branchId).then(response => {
        if (response.data) {
          context.commit('setDoctorsByBranchId', response.data) //Set the Operation Types in the store
        }
      })
    },

    getOperatingRoomsByOperationTypeId(context, payload) {
      axios.get('OperatingRoom/GetOperatingRoomsByOperationTypeId/'+ payload.operationTypeId).then(response => {
        if (response.data) {
          context.commit('setOperatingRoomsByOperationTypeId', response.data) //Set the Operation Types in the store
        }
      })
    },

    getAllBranches(context) {
      axios.get('Branch/GetAllBranches').then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setAllBranches', response.data.result) //Set the Operation Types in the store
        }
      })
    },

    excelExportOperation(context) {
      axios.get('Operation/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href =  "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    }
  }
}

export default operationModule;
