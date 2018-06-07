import Vuex from 'vuex'
import state from './state'; // root's state
import actions from './actions'; //root's action
import mutations from './mutations'; // root's mutation
import getters from './getters'; // root's getters

//Module import
import equipmentModule from './modules/equipments';
import personnelModule from './modules/personnel';
import roomModule from './modules/rooms';

//New Registeration Of Store
export default  new Vuex.Store({
  state,
  getters,
  mutations,
  actions,
  modules : {
     equipmentModule,
     personnelModule,
     roomModule
  }
});
