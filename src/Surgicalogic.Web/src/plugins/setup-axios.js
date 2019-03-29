import axios from 'axios';
import VueAxios from 'vue-axios';
import {
  debug
} from 'util';
import EventBus from '../event-bus';

const httpCodes = {
  badRequest: 401,
  unAuthorized: 403,
  serverError: 500
}

let isAlreadyFetchingAccessToken = false
let subscribers = []

function onAccessTokenFetched(access_token) {
  subscribers = subscribers.filter(callback => callback(access_token))
}

function addSubscriber(callback) {
  subscribers.push(callback)
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
      if (config.url.indexOf(process.env.ROOT_API) <= -1) {
        config.url = process.env.ROOT_API + config.url;
      }

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
        const {
          config,
          response: {
            status
          }
        } = error
        const originalRequest = config
debugger;
        if (status === 401) {
          if (localStorage.getItem("refreshToken")) {
            if (!isAlreadyFetchingAccessToken) {
              isAlreadyFetchingAccessToken = true
              axios.post('User/RefreshToken', {
                token: localStorage.getItem("token"),
                refreshToken: localStorage.getItem("refreshToken")
              }).then(response => {
                if (response.statusText == 'OK') {
                  vm.auth.setAuthentication(response.data.token, response.data.refreshToken, response.data.expiresIn);
                  isAlreadyFetchingAccessToken = false;
                  onAccessTokenFetched(response.data.token);
                } else {
                  return vm.$router.push({
                    name: 'LoginPage'
                  });
                }
              });

              const retryOriginalRequest = new Promise((resolve) => {
                addSubscriber(access_token => {
                  originalRequest.headers.Authorization = 'Bearer ' + access_token
                  resolve(axios(originalRequest))
                })
              })
              return retryOriginalRequest
            }
          } else {
            return vm.$router.push({
              name: 'LoginPage'
            });
          }
        } else if (error.response.status === httpCodes.unAuthorized || error.response.status === 400) {
          return vm.$router.push({
            name: 'LoginPage'
          });
        } else {
          EventBus.$emit("apiErrorDialog");
        }

      } else {
        EventBus.$emit("apiErrorDialog");
      }

      return Promise.reject(error);

    });
}

export {
  setupAxios,
  setupHttpInterceptor
};
