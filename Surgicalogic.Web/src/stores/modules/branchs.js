import axios from 'axios';

const branchsModule = {
    state: { branchs: []},
    mutations: {
      setBranchs(state, branchs) {
          state.branchs = branchs;
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
      getBranchs(context) {
        axios.get('http://localhost/Surgicalogic.Api/Branch/GetBranchs')
            .then(response => {
              if (response.data.info.succeeded == true){  
                context.commit('setBranchs', response.data.result) // set the Branchs in the store
              }
              
          })

      },

      insertBranch(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/Branch/InsertBranch', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {              
              context.commit('insertBranch', { item: response.data.result[0] }) // insert the Branchs in the store
            }
          })

      },
      deleteBranch(context, payload) {
        axios.post('http://localhost/Surgicalogic.Api/Branch/DeleteBranch/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK') {
              context.commit('deleteBranch', { payload }); // delete the Branchs in the store
            }
          })

      },
      updateBranch(context, payload) {

        axios.post('http://localhost/Surgicalogic.Api/Branch/UpdateBranch',  payload)
          .then(response => {
              //context.commit('updateBranch', {payload}) // update the Branchs in the store
          })
      }
    }
  }

  export default branchsModule;
