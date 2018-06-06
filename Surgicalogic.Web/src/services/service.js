export default {
  fetchEquipments() {
    return firebase.database().ref('/equipments').once('value');
  },

  fetchPersonnel() {
    return firebase.database().ref('/personnel').once('value');
  },

  fetchRooms() {
    return firebase.database().ref('/rooms').once('value');
  },

  updateEquipments(payload) {
    return firebase.database().ref('/equipments').child(payload.id);
  }
}
