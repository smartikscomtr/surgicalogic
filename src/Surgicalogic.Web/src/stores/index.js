import Vuex from 'vuex'
import state from './state'; // root's state
import actions from './actions'; //root's action
import mutations from './mutations'; // root's mutation
import getters from './getters'; // root's getters

//Module import
import appointmentCalendarModule from './modules/appointmentcalendar';
import branchesModule from './modules/branches';
import equipmentModule from './modules/equipments';
import equipmentTypesModule from './modules/equipmenttypes';
import feedbackModule from './modules/feedback';
import loginModule from './modules/login';
import operationModule from './modules/operation';
import operationTypeModule from './modules/operationtypes';
import operatingRoomCalendarModule from './modules/operatingroomscalendar'
import operatingRoomModule from './modules/operatingrooms';
import patientModule from './modules/patient';
import personnelModule from './modules/personnel';
import personnelCategoryModule from './modules/personnelCategory';
import personnelTitleModule from './modules/personnelTitle';
import planArrangementsModule from './modules/planarrangements';
import reportsModule from './modules/report';
import settingsModule from './modules/settings';
import usersModule from './modules/users';
import workTypesModule from './modules/worktypes';
import simulation from './modules/simulation';

//New Registeration Of Store
export default new Vuex.Store({
  state,
  getters,
  mutations,
  actions,
  modules : {
    appointmentCalendarModule,
    branchesModule,
    equipmentModule,
    equipmentTypesModule,
    feedbackModule,
    loginModule,
    operationModule,
    operationTypeModule,
    operatingRoomCalendarModule,
    patientModule,
    personnelCategoryModule,
    personnelTitleModule,
    personnelModule,
    planArrangementsModule,
    reportsModule,
    operatingRoomModule,
    settingsModule,
    usersModule,
    workTypesModule,
    simulation
  }
});
