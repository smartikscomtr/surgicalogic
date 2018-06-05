import Vue from 'vue';
import App from './App';
import Vuetify from 'vuetify';
import Vuex from 'vuex';
import router from './router';
import storeOptions from './stores/store';

export const EventBus = new Vue();

Vue.use(Vuex);
Vue.use(Vuetify);
const store = new Vuex.Store(storeOptions);

// Registering Components
import EditPageComponent from '@/components/EditPageComponent';
import GridComponent from '@/components/GridComponent';
import HelloWorld from '@/components/HelloWorld';
import Loading from '@/components/Loading';
import OperationDividerComponent from '@/components/OperationDividerComponent';

Vue.component('edit-page-component', EditPageComponent);
Vue.component('grid-component', GridComponent);
Vue.component('hello-world', HelloWorld);
Vue.component('loading', Loading);
Vue.component('operation-divider-component', OperationDividerComponent);


new Vue({
  el: '#app',
  router,
  store,
  components: { App },
  template: '<App/>'
})
