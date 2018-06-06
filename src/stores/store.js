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
  },

  setUpdateEquipments(state, payload) {
    const equipment = state.equipments.find(equipment => {
      return equipment.id = payload.id
    });

    if (payload.name) {
      equipment.name = payload.name;
    }
    if (payload.type) {
      equipment.type = payload.type;
    }
    if (payload.portable) {
      equipment.portable = payload.portable;
    }
    if (payload.description) {
      equipment.description = payload.description;
    }
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
  },

  updateEquipments(context, payload) {
    const updateObj = {}

    if (payload.name) {
      updateObj.name = payload.name;
    }
    if (payload.type) {
      updateObj.type = payload.type;
    }
    if (payload.portable) {
      updateObj.portable = payload.portable;
    }
    if (payload.description) {
      updateObj.description = payload.description;
    }

    // firebase.database().ref('/equipments').child(payload.id).update(updateObj).
    // then(() => {debugger
    //   context.commit('setUpdateEquipments', payload);
    // });
    return service.updateEquipments(payload).update(updateObj).then(() => {
      context.commit('setUpdateEquipments', payload);
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
