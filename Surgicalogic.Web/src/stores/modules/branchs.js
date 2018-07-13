import axios from 'axios';

const branchsModule = {
  state: {
    branchs: [],
    totalCount:0
  },

  mutations: {
    setBranchs(state, data) {
        state.branchs = data.result;
        state.totalCount = data.totalCount;
    },

    insertBranch(state, { item }) {
      state.branchs.push(item);
    },

    deleteBranch(state, { payload }) {
      let index = state.branchs.findIndex((item) => {
        return item.id === payload.id
      });

      state.branchs.splice(index, 1);
    },

    updateBranch(state, payload) {
      //state.branchs.payload = payload;
    }
  },

  getters: {},

  actions: {
    getBranchs(context,payload) {
      axios.post('Branch/GetBranchs', payload)
        .then(response => {
          if (response.data.info.succeeded == true){
            context.commit('setBranchs', response.data) //Set the Branchs in the store
          }
        })
    },

    insertBranch(context, payload) {
      axios.post('Branch/InsertBranch', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertBranch', { item: response.data.result }) //Insert the Branchs in the store
          }
        })
    },

    deleteBranch(context, payload) {
      axios.post('Branch/DeleteBranch/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteBranch', { payload }); //Delete the Branchs in the store
          }
        })
    },

    updateBranch(context, payload) {
      axios.post('Branch/UpdateBranch', payload)
        .then(response => {
          //context.commit('updateBranch', {payload}) //Update the Branchs in the store
        })
    }
  }
}

export default branchsModule;
