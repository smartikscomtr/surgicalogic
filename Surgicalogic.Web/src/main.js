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
import { i18n } from './plugins/vue-i18n';
import router from './router';

/* ============
 * Main App
 * ============
 * we import the main application.
 */
import App from './App';
import store from './stores';

export const EventBus = new Vue();

Vue.use(Vuex);
Vue.use(Vuetify);

// Registering Components
import BranchsEditComponent from '@/components/editcomponents/BranchsEditComponent';
import EquipmentsEditComponent from '@/components/editcomponents/EquipmentsEditComponent';
import EquipmentTypesEditComponent from '@/components/editcomponents/EquipmentTypesEditComponent';
import GridComponent from '@/components/GridComponent';
import Loading from '@/components/Loading';
import OperationDividerComponent from '@/components/OperationDividerComponent';
import OperationTypesEditComponent from '@/components/editcomponents/OperationTypesEditComponent';
import PersonnelEditComponent from '@/components/editcomponents/PersonnelEditComponent';
import RoomsEditComponent from '@/components/editcomponents/RoomsEditComponent';
import WorkTypesEditComponent from '@/components/editcomponents/WorkTypesEditComponent';

Vue.component('branchs-edit-component', BranchsEditComponent);
Vue.component('equipments-edit-component', EquipmentsEditComponent);
Vue.component('equipment-types-edit-component', EquipmentTypesEditComponent);
Vue.component('grid-component', GridComponent);
Vue.component('loading', Loading);
Vue.component('operation-divider-component', OperationDividerComponent);
Vue.component('operation-types-edit-component', OperationTypesEditComponent);
Vue.component('personnel-edit-component', PersonnelEditComponent);
Vue.component('rooms-edit-component', RoomsEditComponent);
Vue.component('work-types-edit-component', WorkTypesEditComponent);



new Vue({
  el: '#app',
  i18n,
  router,
  store,
  components: { App },
  template: '<App/>'
})
