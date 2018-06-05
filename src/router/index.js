import Vue from 'vue'
import VueRouter from 'vue-router'
import Vuex from 'vuex'

import Vuetify from "vuetify/dist/vuetify.min.css";

Vue.use(Vuetify);
Vue.use(VueRouter);
Vue.use(Vuex);

import DashboardPage from "@/pages/DashboardPage";
import EquipmentsPage from "@/pages/index/EquipmentsPage";
import PersonnelPage from "@/pages/index/PersonnelPage";
import RoomsPage from "@/pages/index/RoomsPage";


import EquipmentsEditPage from "@/pages/edit/EquipmentsEditPage";

export default new VueRouter({
  mode: 'history',
  routes: [
    {
      path: '/dashboardpage',
      name: 'DashboardPage',
      component: DashboardPage
    },
    {
      path: '/equipmentspage',
      name: 'EquipmentsPage',
      component: EquipmentsPage
    },
    {
      path: '/personnelpage',
      name: 'PersonnelPage',
      component: PersonnelPage
    },
    {
      path: '/roomspage',
      name: 'RoomsPage',
      component: RoomsPage
    },



    {
      path: '/equipmentseditpage',
      name: 'EquipmentsEditPage',
      component: EquipmentsEditPage
    }
  ]
})
