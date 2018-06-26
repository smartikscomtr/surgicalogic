import Vue from 'vue'
import VueRouter from 'vue-router'
import Vuex from 'vuex'

import Vuetify from "vuetify/dist/vuetify.min.css";

Vue.use(Vuetify);
Vue.use(VueRouter);
Vue.use(Vuex);

import BranchsPage from "@/pages/index/BranchsPage";
import DashboardPage from "@/pages/DashboardPage";
import EquipmentsPage from "@/pages/index/EquipmentsPage";
import EquipmentTypesPage from "@/pages/index/EquipmentTypesPage";
import OperationTypesPage from "@/pages/index/OperationTypesPage";
import PersonnelPage from "@/pages/index/PersonnelPage";
import RoomsPage from "@/pages/index/RoomsPage";
import WorkTypesPage from "@/pages/index/WorkTypesPage";


export default new VueRouter({
  mode: 'history',
  routes: [
    {
      path: '/branchspage',
      name: 'BranchsPage',
      component: BranchsPage
    },
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
      path: '/equipmenttypespage',
      name: 'EquipmentTypesPage',
      component: EquipmentTypesPage
    },
    {
      path: '/operationtypespage',
      name: 'OperationTypesPage',
      component: OperationTypesPage
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
      path: '/worktypespage',
      name: 'WorkTypesPage',
      component: WorkTypesPage
    },
  ]
})
