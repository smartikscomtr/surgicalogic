import axios from 'axios';

const branchesModule = {
  state: {
    branches: [],
    allBranches: [],
    totalCount: 0,
  },

  mutations: {
    setBranches(state, data) {
      state.branches = data.result;
      state.totalCount = data.totalCount;
    },

    setAllBranches(state, data) {
      state.allBranches = data;
    },

    insertBranch(state, { item }) {
      state.branches.push(item);
    },

    deleteBranch(state, { payload }) {
      let index = state.branches.findIndex((item) => {
        return item.id === payload.id
      });

      state.branches.splice(index, 1);
    },

    updateBranch(state, payload) {
      state.branches.forEach(element => {
        if(element.id == payload.id)
          Object.assign(element, payload);
      });
    }
  },

  getters: {},

  actions: {
    getBranches(context,params) {
      axios.get('Branch/GetBranches', {
        params: params
      })
        .then(response => {
          if (response.data.info.succeeded == true){
            context.commit('setBranches', response.data) //Set the Branches in the store
          }
        })
    },

    getAllBranches(context) {
      axios.get('Branch/GetAllBranches')
        .then(response => {
          context.commit('setAllBranches', response.data.result) //Set the Branches in the store
        })
    },

    insertBranch(context, payload) {
      axios.post('Branch/InsertBranch', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertBranch', { item: response.data.result }) //Insert the Branches in the store
          }
        })
    },

    deleteBranch(context, payload) {
      axios.post('Branch/DeleteBranch/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteBranch', { payload }); //Delete the Branches in the store
          }
        })
    },

    updateBranch(context, payload) {
      axios.post('Branch/UpdateBranch', payload)
        .then(response => {
          context.commit('updateBranch', response.data.result) //Update the Branches in the store
        })
    }
  }
}

export default branchesModule;
