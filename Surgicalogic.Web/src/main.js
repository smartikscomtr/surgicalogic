import Vue from 'vue';
import App from './App';
import Vuetify from 'vuetify';
import Vuex from 'vuex';
import router from './router';
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
  router,
  store,
  components: { App },
  template: '<App/>'
})
