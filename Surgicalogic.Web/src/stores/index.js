import Vuex from 'vuex'
import state from './state'; // root's state
import actions from './actions'; //root's action
import mutations from './mutations'; // root's mutation
import getters from './getters'; // root's getters

//Module import
import branchesModule from './modules/branches';
import clinicModule from './modules/clinic';
import equipmentModule from './modules/equipments';
import equipmentTypesModule from './modules/equipmenttypes';
import feedbackModule from './modules/feedback';
import historyPlanningModule from './modules/historyplanning';
import loginModule from './modules/login';
import operationModule from './modules/operation';
import operationTypeModule from './modules/operationtypes';
import operatingRoomCalendarModule from './modules/operatingroomscalendar'
import operatingRoomModule from './modules/operatingrooms';
import personnelModule from './modules/personnel';
import personnelCategoryModule from './modules/personnelCategory';
import personnelTitleModule from './modules/personnelTitle';
import planArrangementsModule from './modules/planarrangements';
import settingsModule from './modules/settings';
import usersModule from './modules/users';
import workTypesModule from './modules/worktypes';

//New Registeration Of Store
export default new Vuex.Store({
  state,
  getters,
  mutations,
  actions,
  modules : {
    branchesModule,
    clinicModule,
    equipmentModule,
    equipmentTypesModule,
    feedbackModule,
    historyPlanningModule,
    loginModule,
    operationModule,
    operationTypeModule,
    operatingRoomCalendarModule,
    personnelCategoryModule,
    personnelTitleModule,
    personnelModule,
    planArrangementsModule,
    operatingRoomModule,
    settingsModule,
    usersModule,
    workTypesModule
  }
});
