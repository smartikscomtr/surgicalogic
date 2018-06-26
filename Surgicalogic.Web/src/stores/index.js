import Vuex from 'vuex'
import state from './state'; // root's state
import actions from './actions'; //root's action
import mutations from './mutations'; // root's mutation
import getters from './getters'; // root's getters

//Module import
import branchsModule from './modules/branchs';
import equipmentModule from './modules/equipments';
import equipmentTypesModule from './modules/equipmentTypes';
import operationTypeModule from './modules/operationtypes';
import personnelModule from './modules/personnel';
import roomModule from './modules/rooms';
import workTypesModule from './modules/workTypes';

//New Registeration Of Store
export default  new Vuex.Store({
  state,
  getters,
  mutations,
  actions,
  modules : {
     branchsModule,
     equipmentModule,
     equipmentTypesModule,
     operationTypeModule,
     personnelModule,
     roomModule,
     workTypesModule
  }
});
