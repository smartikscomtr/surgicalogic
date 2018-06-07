import axios from 'axios';
const personnelModule = {
    state: {
        personnel: []
    },
    mutations: {
        setPersonnel(state, personnel) {
            state.personnel = personnel;
          }
    },  
    getters: {},
    actions: {
        getPersonnel(context) {
            axios.get('http://localhost:8080/static/personnel.json')
              .then(response => {        
                context.commit('setPersonnel', response.data) // set the Personnel in the store
              })   
          }
    }
  }

  export default personnelModule;