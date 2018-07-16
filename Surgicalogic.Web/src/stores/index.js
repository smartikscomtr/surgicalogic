import Vuex from 'vuex'
import state from './state'; // root's state
import actions from './actions'; //root's action
import mutations from './mutations'; // root's mutation
import getters from './getters'; // root's getters

//Module import
import branchesModule from './modules/branches';
import equipmentModule from './modules/equipments';
import equipmentTypesModule from './modules/equipmentTypes';
import loginModule from './modules/login';
import operationTypeModule from './modules/operationtypes';
import personnelModule from './modules/personnel';
import personnelTitleModule from './modules/personnelTitle';
import operatingRoomModule from './modules/operatingrooms';
import usersModule from './modules/users';
import workTypesModule from './modules/workTypes';

//New Registeration Of Store
export default new Vuex.Store({
  state,
  getters,
  mutations,
  actions,
  modules : {
    branchesModule,
    equipmentModule,
    equipmentTypesModule,
    loginModule,
    operationTypeModule,
    personnelModule,
    personnelTitleModule,
    operatingRoomModule,
    usersModule,
    workTypesModule
  }
});
