import axios from 'axios';
import router from '../../router/index';

const loginModule = {
  state: {
    loginBody: []
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
      axios.post('User/Login', payload)
        .then(response => {
          debugger;
          if (response.statusText == "OK") {
            debugger;
            localStorage.setItem("token", response.data.token);
            localStorage.setItem("refreshToken", response.data.refreshToken);
            localStorage.setItem('expiresIn', new Date(Date.parse(response.data.expiresIn)).getTime());
            router.push("DashboardPage");
          }
          else
          {
            router.push("LoginPage");
          }
        })
        .catch(error => {
          router.push("LoginPage");
        })
    },

    userLogout(context, payload) {
      axios.post('User/LogOff')
        .then(response => {
          if (response.statusText == "OK") {
            localStorage.removeItem("token");
            localStorage.removeItem("refreshToken");
            localStorage.removeItem("expiresIn");
          }
        })
    }
  }
}

export default loginModule;
