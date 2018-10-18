import axios from 'axios';
import VueAxios from 'vue-axios';
import {
  debug
} from 'util';

const httpCodes = {
  badRequest: 401,
  serverError: 500
}

function cleanResponseError(err) {
  const error = (err instanceof Error) ? serializeError(err) : Object.assign({}, err);

  delete error['name']; // eslint-disable-line dot-notation
  delete error['config']; // eslint-disable-line dot-notation

  if (error.response) {
    delete error.response['config']; // eslint-disable-line dot-notation
    delete error.response['headers']; // eslint-disable-line dot-notation
    delete error.response['request']; // eslint-disable-line dot-notation
  }

  return error;
}

function setupAxios() {
  const axiosInstance = axios.create({
    headers: {
      'Accept': 'application/json' // eslint-disable-line quote-props
    }
  });

  return axiosInstance;
}

function setupHttpInterceptor(vm) { // eslint-disable-line consistent-this
  axios.interceptors.request.use(
    config => {
      config.url = process.env.ROOT_API + config.url;

      if (vm.auth.isAuthenticated()) {
        const token = vm.auth.getToken();
        config.headers['Authorization'] = `Bearer ${token}`; // eslint-disable-line dot-notation
      }

      return config;
    },
    error => {
      console.error('Http before request error:', error); // eslint-disable-line no-console

      const cleanError = cleanResponseError(error);

      vm.handleError.call(vm, cleanError, vm); // eslint-disable-line no-useless-call

      return Promise.reject(cleanError);
    });

  axios.interceptors.response.use(
    response => {
      return response;
    },
    error => {
      if (error.response) {
        if (error.response.status === httpCodes.badRequest &&
          error.response.statusText === "Unauthorized") {

          if (localStorage.getItem("refreshToken")) {
            axios.post('Account/RefreshToken', {
                token: localStorage.getItem("token"),
                refreshToken: localStorage.getItem("refreshToken")
              })
              .then(response => {
                if (response.statusText == 'OK') {
                  vm.auth.setAuthentication(response.data.token, response.data.refreshToken, response.data.expiresIn);
                  // localStorage.setItem("token", response.data.token);
                  // localStorage.setItem("refreshToken", response.data.refreshToken);
                  // localStorage.setItem("expiresIn", response.data.expiresIn);
                  vm.$router.push(vm.$router.currentRoute.name);
                }
                else {
                  return vm.$router.push({
                    name: 'LoginPage'
                  });
                }
              }).catch(error => {
                return vm.$router.push({
                  name: 'LoginPage'
                });
            });
          } else {
            return vm.$router.push({
              name: 'LoginPage'
            });
          }

        }
      }

      console.error('Http after request error: ', error);

      return Promise.reject(error);
    });
}

export {
  setupAxios,
  setupHttpInterceptor
};
