import axios from 'axios';

const simulationModule = {
    state : {
        loading: false,
        totalCount: 0,
        model: [],
        tomorrowList : [],
        tomorrowListTotalCount : 0
    },
    
    mutations : {
        setLoading(state, data) {
            state.loading = data;
          },
          setTomorrowOperationList(state, data) {
            state.tomorrowList = data.result;
            state.tomorrowListTotalCount = data.totalCount;
          },
    },

    getters: {},

    actions: {
        runSimulation(context){
            axios.get('Simulation/Run')
                .then(response => {
                    context.commit('setTomorrowOperationList', response.data) //Set the OperationPlanPlan in the store
                })
        },
    }


       
}

export default simulationModule;