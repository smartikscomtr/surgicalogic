import axios from 'axios';

const usersModule = {
  state: {
    loading: false,
    totalCount: 0,
    users: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setUsers(state, users) {
      state.users = users;
    },

    insertUsers(state, { item }) {
      state.users.push(item);
    },

    deleteUser(state, { payload }) {
      let index = state.users.findIndex((item) => {
        return item.id === payload.id
      });

      state.users.splice(index, 1);
    },

    updateUser(state, payload) {
      if (payload){
        state.users.forEach(element => {
          if (element.id == payload.id)
            Object.assign(element, payload);
        });
      }
    }
  },

  getters: {},

  actions: {
    getUsers(context) {
      context.commit('setLoading', true);

      axios.post('User/GetUsers').then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setUsers', response.data.result) //Set the Users in the store
        }

        context.commit('setLoading', false);
      })
    },

    insertUser(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('User/InsertUser', payload)
          .then(response => {
            if (response.statusText == 'OK') {
              payload.id = response.data;
              context.commit('insertUser', {
                item: response.data.result
              }) //Insert the User in the store
            }

            resolve(response);
          })
      });
    },

    deleteUser(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('User/DeleteUser/' + payload.id)
          .then(response => {
            if (response.statusText == 'OK' && response.data.info.succeeded == true) {
              context.commit('deleteUser', {
                payload
              }); //Delete the User in the store
            }

            resolve(response);
          })
      });
    },

    updateUser(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('User/UpdateUser', payload)
          .then(response => {
            context.commit('updateUser', response.data.result) //Update the User in the store
            resolve(response);
          })
      });
    },

    resetPassword(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('User/ForgotPassword', payload)
          .then(response => {
            //context.commit('updateUser', {payload}) //Update the User in the store
            resolve(response);
          })
      });
    },

    updatePassword(context, payload) {
      return new Promise((resolve, reject) => {
      axios.post('User/ResetPassword', payload)
        .then(response => {
          //context.commit('updateUser', {payload}) //Update the User in the store
          resolve(response);
        })
    });
  },

    excelExportUser(context, payload) {
      axios.get('User/ExcelExport?langId=' + payload.langId)
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

export default usersModule;
