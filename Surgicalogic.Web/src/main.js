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

// Registering Components
import GridComponent from '@/components/GridComponent';
import OperationDividerComponent from '@/components/OperationDividerComponent';

Vue.component('grid-component', GridComponent);
Vue.component('operation-divider-component', OperationDividerComponent);

// Registering Calendar Rage Components
import OperatingRoomsCalendarComponent from '@/components/customcomponents/OperatingRoomsCalendarComponent';

Vue.component('operating-rooms-calendar-component', OperatingRoomsCalendarComponent);

// Registering Detail Components
import EquipmentsDetailComponent from '@/components/detailcomponents/EquipmentsDetailComponent';
import OperatingRoomsDetailComponent from '@/components/detailcomponents/OperatingRoomsDetailComponent';
import OperationDetailComponent from '@/components/detailcomponents/OperationDetailComponent';
import OperationTypesDetailComponent from '@/components/detailcomponents/OperationTypesDetailComponent';

Vue.component('equipments-detail-component', EquipmentsDetailComponent);
Vue.component('operating-rooms-detail-component', OperatingRoomsDetailComponent);
Vue.component('operation-detail-component', OperationDetailComponent);
Vue.component('operation-types-detail-component', OperationTypesDetailComponent);

// Registering Delete Components
import DeleteComponent from '@/components/DeleteComponent';

Vue.component('delete-component', DeleteComponent);

// Registering Reset Password Components
import ResetPasswordComponent from '@/components/customcomponents/ResetPasswordComponent';

Vue.component('reset-password-component', ResetPasswordComponent);

// Registering Edit Components
import BranchesEditComponent from '@/components/editcomponents/BranchesEditComponent';
import EquipmentsEditComponent from '@/components/editcomponents/EquipmentsEditComponent';
import EquipmentTypesEditComponent from '@/components/editcomponents/EquipmentTypesEditComponent';
import OperatingRoomsEditComponent from '@/components/editcomponents/OperatingRoomsEditComponent';
import OperationEditComponent from '@/components/editcomponents/OperationEditComponent';
import OperationTypesEditComponent from '@/components/editcomponents/OperationTypesEditComponent';
import PersonnelEditComponent from '@/components/editcomponents/PersonnelEditComponent';
import PersonnelTitleEditComponent from '@/components/editcomponents/PersonnelTitleEditComponent';
import UsersEditComponent from '@/components/editcomponents/UsersEditComponent';
import WorkTypesEditComponent from '@/components/editcomponents/WorkTypesEditComponent';
import OperatingRoomsCalendarEditComponent from '@/components/editcomponents/OperatingRoomsCalendarEditComponent';

Vue.component('branches-edit-component', BranchesEditComponent);
Vue.component('equipments-edit-component', EquipmentsEditComponent);
Vue.component('equipment-types-edit-component', EquipmentTypesEditComponent);
Vue.component('operating-rooms-edit-component', OperatingRoomsEditComponent);
Vue.component('operation-types-edit-component', OperationTypesEditComponent);
Vue.component('operation-edit-component', OperationEditComponent);
Vue.component('personnel-edit-component', PersonnelEditComponent);
Vue.component('personnel-title-edit-component', PersonnelTitleEditComponent);
Vue.component('users-edit-component', UsersEditComponent);
Vue.component('work-types-edit-component', WorkTypesEditComponent);
Vue.component('operating-rooms-calendar-edit-component', OperatingRoomsCalendarEditComponent);


// Registering Core Components
import SnackbarComponent from '@/components/coreComponents/SnackbarComponent';

Vue.component('snackbar-component', SnackbarComponent);

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
