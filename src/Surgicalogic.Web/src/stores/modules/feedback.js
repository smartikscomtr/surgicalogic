import axios from 'axios';

const feedbackModule = {
  state: {
    loading: false,
    totalCount: 0,
    feedbacks: [],
    allFeedbacks: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setFeedbacks(state, data) {
      state.feedbacks = data.result;
      state.totalCount = data.totalCount;
    },

    setAllFeedbacks(state, data) {
      state.allFeedbacks = data;
    },

    insertFeedback(state, { item }) {
      state.feedbacks.push(item);
    },

    deleteFeedback(state, { payload }) {
      let index = state.feedbacks.findIndex((item) => {
        return item.id === payload.id
      });

      state.feedbacks.splice(index, 1);
    },
  },

  getters: {},

  actions: {
    getFeedbacks(context,params) {
      context.commit('setLoading', true);

      axios.get('Feedback/GetFeedbacks', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setFeedbacks', response.data) //Set the Feedback in the store
        }

        context.commit('setLoading', false);
      })
    },

    getAllFeedbacks(context) {
      axios.get('Feedback/GetAllFeedbacks')
        .then(response => {
          context.commit('setAllFeedbacks', response.data.result) //Set the Feedback in the store
        })
    },

    insertFeedback(context, payload) {
      axios.post('Feedback/InsertFeedback', payload)
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('insertFeedback', { item: response.data.result }) //Insert the Feedback in the store
          }
        })
    },

    excelExportFeedbacks(context) {
      axios.get('Feedback/ExcelExport')
        .then(response => {
          const link = document.createElement('a');

          link.href = "/static/" + response.data;
          document.body.appendChild(link);
          link.click();
        })
    },

    deleteFeedback(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Feedback/DeleteFeedback/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deleteFeedback', {
                payload
              }); //Delete the Feedbacks in the store
            }

            resolve(response);
          })
      });
    },
  }
}

export default feedbackModule;
