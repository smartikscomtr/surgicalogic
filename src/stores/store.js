import service from '../services/service';

const state = {
    equipments: [],
    personnel: [],
    rooms: [] //apiden service'in içinden rooms içinden 1,2,3,.. bos bir Array dönüyor
}

const getters = {}

const mutations = {
  setEquipments(state, equipments) {
    state.equipments = equipments;
  },

  setPersonnel(state, personnel) {
    state.personnel = personnel;
  },

  setRooms(state, rooms) {
    state.rooms = rooms;
  }
}

const actions = {
  fetchEquipments(context) {
    return service.fetchEquipments().then((snapshot) => {
      context.commit('setEquipments', snapshot.val());
    });
  },

  fetchPersonnel(context) {
    return service.fetchPersonnel().then((snapshot) => {
      context.commit('setPersonnel', snapshot.val());
    });
  },

  fetchRooms(context) {
      return service.fetchRooms().then((snapshot) => {
        context.commit('setRooms', snapshot.val());
      });
  }
}

//Bu store'u main.js de ayağa kaldırıyoruz
export default {
    state,
    getters,
    mutations,
    actions
}
