import axios from 'axios';

const settingsModule = {
  state: {
    settings: [],
    loading: false,
    totalCount: 0,
    allSettings: [],
    settingDataTypes: []
  },

  mutations: {
    setLoading(state, data) {
      state.loading = data;
    },

    setSettings(state, data) {
      state.settings = data.result;
      state.totalCount = data.totalCount;
    },

    setSettingByName(state, data) {
      debugger;
      state.settings = data.result;
    },

    setAllSettings(state, data) {
      state.allSettings = data;
    },

    updateSetting(state, payload) {
      state.settings.forEach(element => {
        if (element.id == payload.id)
          Object.assign(element, payload);
      });
    },

    setSettingDataTypes(state, data) {
      state.settingDataTypes = data;
    }
  },

  getters: {},

  actions: {
    getSettings(context, params) {
      context.commit('setLoading', true);

      axios.get('Setting/GetSettings', {
        params: params
      }).then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setSettings', response.data)
        }

        context.commit('setLoading', false);
      })
    },

    getAppointmentDays(context, params) {
      context.commit('setLoading', true);

      axios.get('Setting/GetSettingByName?name=ClinicAppointmentDays').then(response => {
        if (response.data.info.succeeded == true) {
          context.commit('setSettingByName', response.data)
        }

        context.commit('setLoading', false);
      })
    },

    getAllSettings(context) {
      axios.get('Setting/GetAllSettings')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setAllSettings', response.data.result)
          }
        })
    },

    getSettingDataTypes(context) {
      axios.get('Setting/GetSettingDataTypes')
        .then(response => {
          if (response.data.info.succeeded == true) {
            context.commit('setSettingDataTypes', response.data.result)
          }
        })
    },

    updateSetting(context, payload) {
      return new Promise((resolve, reject) => {
        axios.post('Setting/UpdateSetting', payload)
          .then(response => {
            if (response.data.info.succeeded == true) {
              context.commit('updateSetting', payload)
            }

            resolve(response);
          })
      });
    }
  }
}

export default settingsModule;
