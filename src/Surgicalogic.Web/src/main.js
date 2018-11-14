/* ============
 * Main File
 * ============
 * Will initialize the application.
 */
import Vue from 'vue';
/* ============
 * Plugins
 * ============
 * Import and bootstrap the plugins.
 */

import Vuetify from 'vuetify';
import Vuex from 'vuex';
import VueAxios from 'vue-axios';
import { i18n } from './plugins/vue-i18n';
import router from './router';
import Authentication from './plugins/authentication-plugin.js';
import VueChartkick from 'vue-chartkick';

var VueCookie = require('vue-cookie');
/* ============
 * Main App
 * ============
 * we import the main application.
 */

import App from './App';
import store from './stores';

Vue.use(Vuex);
Vue.use(Vuetify);
Vue.use(Authentication);
Vue.use(require('vue-moment'));
Vue.use(VueCookie);
Vue.use(VueChartkick);

// Registering Components
import DeleteComponent from '@/components/DeleteComponent';
import FeedbackComponent from '@/components/FeedbackComponent';
import GridComponent from '@/components/GridComponent';

Vue.component('delete-component', DeleteComponent);
Vue.component('feedback-component', FeedbackComponent);
Vue.component('grid-component', GridComponent);

// Registering Calendar Rage Components
import OperatingRoomsCalendarComponent from '@/components/customcomponents/OperatingRoomsCalendarComponent';

Vue.component('operating-rooms-calendar-component', OperatingRoomsCalendarComponent);

// Registering Detail Components
import EquipmentsDetailComponent from '@/components/detailcomponents/EquipmentsDetailComponent';
import FeedbackDetailComponent from '@/components/detailcomponents/FeedbackDetailComponent';
import OperatingRoomsDetailComponent from '@/components/detailcomponents/OperatingRoomsDetailComponent';
import OperationDetailComponent from '@/components/detailcomponents/OperationDetailComponent';
import OperationTypesDetailComponent from '@/components/detailcomponents/OperationTypesDetailComponent';

Vue.component('equipments-detail-component', EquipmentsDetailComponent);
Vue.component('feedback-detail-component', FeedbackDetailComponent);
Vue.component('operating-rooms-detail-component', OperatingRoomsDetailComponent);
Vue.component('operation-detail-component', OperationDetailComponent);
Vue.component('operation-types-detail-component', OperationTypesDetailComponent);

// Registering Tomorrow Planning Components
import TomorrowPlanningComponent from '@/components/customcomponents/TomorrowPlanningComponent';

Vue.component('tomorrow-planning-component', TomorrowPlanningComponent);

// Registering Edit Components
import BranchesEditComponent from '@/components/editcomponents/BranchesEditComponent';
import EquipmentsEditComponent from '@/components/editcomponents/EquipmentsEditComponent';
import EquipmentTypesEditComponent from '@/components/editcomponents/EquipmentTypesEditComponent';
import OperatingRoomsEditComponent from '@/components/editcomponents/OperatingRoomsEditComponent';
import OperationEditComponent from '@/components/editcomponents/OperationEditComponent';
import OperationTypesEditComponent from '@/components/editcomponents/OperationTypesEditComponent';
import PersonnelEditComponent from '@/components/editcomponents/PersonnelEditComponent';
import PersonnelCategoryEditComponent from '@/components/editcomponents/PersonnelCategoryEditComponent';
import PersonnelTitleEditComponent from '@/components/editcomponents/PersonnelTitleEditComponent';
import UsersEditComponent from '@/components/editcomponents/UsersEditComponent';
import WorkTypesEditComponent from '@/components/editcomponents/WorkTypesEditComponent';
import OperatingRoomsCalendarEditComponent from '@/components/editcomponents/OperatingRoomsCalendarEditComponent';
import SettingsEditComponent from '@/components/editcomponents/SettingsEditComponent';

Vue.component('branches-edit-component', BranchesEditComponent);
Vue.component('equipments-edit-component', EquipmentsEditComponent);
Vue.component('equipment-types-edit-component', EquipmentTypesEditComponent);
Vue.component('operating-rooms-edit-component', OperatingRoomsEditComponent);
Vue.component('operation-types-edit-component', OperationTypesEditComponent);
Vue.component('operation-edit-component', OperationEditComponent);
Vue.component('personnel-edit-component', PersonnelEditComponent);
Vue.component('personnel-category-edit-component', PersonnelCategoryEditComponent);
Vue.component('personnel-title-edit-component', PersonnelTitleEditComponent);
Vue.component('users-edit-component', UsersEditComponent);
Vue.component('work-types-edit-component', WorkTypesEditComponent);
Vue.component('operating-rooms-calendar-edit-component', OperatingRoomsCalendarEditComponent);
Vue.component('settings-edit-component', SettingsEditComponent);


// Registering Core Components
import LoadingComponent from '@/components/coreComponents/LoadingComponent';
import SnackbarComponent from '@/components/coreComponents/SnackbarComponent';

Vue.component('snackbar-component', SnackbarComponent);
Vue.component('loading-component', LoadingComponent);

import {
  setupAxios,
  setupHttpInterceptor
} from './plugins/setup-axios';

const axios = setupAxios();

Vue.use(VueAxios,axios);

new Vue({
  el: '#app',
  i18n,
  router,
  store,
  components: { App },
  template: '<App/>',

  created() {
    const vm = this;
    setupHttpInterceptor(vm);
  }
})
