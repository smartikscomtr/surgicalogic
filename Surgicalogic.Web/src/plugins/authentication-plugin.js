export default {
  install(Vue, options = {}) {
    const authHandler = {

      setAuthentication(token, refreshToken, expiresIn) {
        localStorage.setItem('token', token);
        localStorage.setItem('refreshToken', refreshToken);
        localStorage.setItem('expiresIn', new Date(Date.parse(expiresIn)).getTime());
      },

      set(key, value) {
        localStorage.setItem(key, value);
      },

      get(key) {
        return localStorage.getItem(key);
      },

      getUserId() {
        return localStorage.getItem('userId');
      },

      getToken() {
        return localStorage.getItem('token');
      },

      getRefreshToken() {
        return localStorage.getItem('refreshToken');
      },

      isAuthenticated() { // Yetkili mi değil mi kontrolünü yapan metod
        const accessToken = localStorage.getItem('token');
        const expiresIn = localStorage.getItem('expiresIn');

        if (accessToken && expiresIn) {
          return +expiresIn > Date.now();
        } else {
          return false;
        }
        // if (accessToken) {
        //   return true;
        // } else {
        //   return false;
        // }
      },

      clearAuthentication() {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('expiresIn');
      }
    };

    const authHandlerProxy = new Proxy(authHandler, {
      get(target, property, receiver) {
        const value = target[property];

        if (typeof value === 'undefined') {
          throw new Error(`${property} is not a valid authentication operation.`);
        }

        return value;
      },

      set(target, property, value) {
        throw new Error(`${property} is a read-only value.`);
      },

      deleteProperty(target, property) {
        throw new Error(`You can not delete ${property}.`);
      }
    });

    Object.defineProperties(Vue.prototype, {
      auth: {
        get() {
          return authHandlerProxy;
        }
      }
    });
  }
};
