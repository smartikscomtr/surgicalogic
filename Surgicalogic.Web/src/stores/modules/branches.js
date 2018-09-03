import axios from 'axios';

const branchesModule = {
  state: {
    loading: false,
    totalCount: 0,
    branches: [],
    allBranches: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

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
      context.commit('setLoading', true);

      axios.get('Branch/GetBranches', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setBranches', response.data) //Set the Branches in the store
        }

        context.commit('setLoading', false);
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
      return new Promise((resolve, reject) => {
        axios.post('Branch/DeleteBranch/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deleteBranch', { payload }); //Delete the Branches in the store
            }

            resolve(response);
          })
      });
    },

    updateBranch(context, payload) {
      axios.post('Branch/UpdateBranch', payload)
        .then(response => {
          context.commit('updateBranch', response.data.result) //Update the Branches in the store
        })
    },

    excelExportBranches(context) {
      axios.get('Branch/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    }
  }
}

export default branchesModule;
