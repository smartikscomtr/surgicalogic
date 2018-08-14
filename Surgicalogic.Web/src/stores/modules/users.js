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

    deleteUsers(state, { payload }) {
      let index = state.users.findIndex((item) => {
        return item.id === payload.id
      });

      state.users.splice(index, 1);
    },

    updateUsers(state, payload) {
      //state.users.payload = payload;
    }
  },

  getters: {},

  actions: {
    getUsers(context) {
      context.commit('setLoading', true);

      axios.post('User/GetUsers').then(response => {
        if (response.data.info.succeeded == true){
          context.commit('setUsers', response.data.result) //Set the Users in the store
        }

        context.commit('setLoading', false);
      })
    },

    insertUser(context, payload) {
      axios.post('User/InsertUser', payload)
        .then(response => {
          if (response.statusText == 'OK') {
            payload.id = response.data;
            context.commit('insertUser', { item: response.data.result }) //Insert the User in the store
          }
        })
    },

    deleteUser(context, payload) {
      axios.post('User/DeleteUser/' + payload.id)
        .then(response => {
          if (response.statusText == 'OK') {
            context.commit('deleteUser', { payload }); //Delete the User in the store
          }
        })
    },

    updateUser(context, payload) {
      axios.post('User/UpdateUser', payload)
        .then(response => {
          //context.commit('updateUser', {payload}) //Update the User in the store
        })
    },

    resetPassword(context, payload) {
      axios.post('User/ForgotPassword', payload)
        .then(response => {
          //context.commit('updateUser', {payload}) //Update the User in the store
        })
    },

    updatePassword(context, payload) {
      axios.post('User/ResetPassword', payload)
        .then(response => {
          //context.commit('updateUser', {payload}) //Update the User in the store
        })
    },
  }
}

export default usersModule;
