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
import EquipmentsEditComponent from '@/components/editcomponents/EquipmentsEditComponent';
import GridComponent from '@/components/GridComponent';
import Loading from '@/components/Loading';
import OperationDividerComponent from '@/components/OperationDividerComponent';
import PersonnelEditComponent from '@/components/editcomponents/PersonnelEditComponent';
import RoomsEditComponent from '@/components/editcomponents/RoomsEditComponent';

Vue.component('equipments-edit-component', EquipmentsEditComponent);
Vue.component('grid-component', GridComponent);
Vue.component('loading', Loading);
Vue.component('operation-divider-component', OperationDividerComponent);
Vue.component('personnel-edit-component', PersonnelEditComponent);
Vue.component('rooms-edit-component', RoomsEditComponent);



new Vue({
  el: '#app',
  i18n,
  router,
  store,
  components: { App },
  template: '<App/>'
})
