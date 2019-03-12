import axios from 'axios';
import router from '../../router/index';

const loginModule = {
  state: {
    loginBody: [],
    loginError: null
  },

  mutations: {
    userLogin(state, {
      item
    }) {
      state.loginBody.push(item);
    },

    userLogout(state) {}
  },

  getters: {},

  actions: {
    userLogin(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('User/Login', payload)
          .then(response => {
            if (response.statusText == "OK") {
              if (response.data) {
                localStorage.setItem("username", response.data.username);
                localStorage.setItem("token", response.data.token);
                localStorage.setItem("refreshToken", response.data.refreshToken);
                localStorage.setItem('expiresIn', new Date(Date.parse(response.data.expiresIn)).getTime());
                router.push("dashboardpage");
              }
              else {
                  context.state.loginError = true;
              }
            }
            else {
              router.push("LoginPage");
            }

            resolve(response);
          })
      })
    },

    userLogout(context, payload) {
      axios.post('User/LogOff')
        .then(response => {
          if (response.statusText == "OK") {
            localStorage.removeItem("username");
            localStorage.removeItem("token");
            localStorage.removeItem("refreshToken");
            localStorage.removeItem("expiresIn");
          }
        })
    }
  }
}

export default loginModule;
