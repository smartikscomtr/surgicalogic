import axios from 'axios';

const branchsModule = {
    state: { branchs: []},
    mutations: {
        setBranchs(state, branchs) {
            state.branchs = branchs;
          }
    },
    getters: {},
    actions: {
        getBranchs(context) {
            axios.get('http://localhost:8080/static/branchs.json')
              .then(response => {
                context.commit('setBranchs', response.data) // set the Equipments in the store
              })
            }
    }
  }

  export default branchsModule;
