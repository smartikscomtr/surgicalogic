import Vue from 'vue'
import VueRouter from 'vue-router'
import Vuex from 'vuex'

import Vuetify from "vuetify/dist/vuetify.min.css";

Vue.use(Vuetify);
Vue.use(VueRouter);
Vue.use(Vuex);


//Registering Definitions Pages
import BranchesPage from "@/pages/definitions/BranchesPage";
import EquipmentsPage from "@/pages/definitions/EquipmentsPage";
import EquipmentTypesPage from "@/pages/definitions/EquipmentTypesPage";
import OperationTypesPage from "@/pages/definitions/OperationTypesPage";
import PersonnelPage from "@/pages/definitions/PersonnelPage";
import PersonnelTitlePage from "@/pages/definitions/PersonnelTitlePage";
import OperatingRoomsPage from "@/pages/definitions/OperatingRoomsPage";
import OperatingRoomCalendarPage from "@/pages/definitions/OperatingRoomCalendarPage";
import WorkTypesPage from "@/pages/definitions/WorkTypesPage";


//Registering Index Pages
import DashboardPage from "@/pages/index/DashboardPage";


//Registering Login Pages
import LoginPage from "@/pages/login/LoginPage";

//Registering reset password Pages
import ResetPassword from "@/pages/login/ResetPassword"

//Registering forgot password Pages
import ForgotPassword from "@/pages/login/ForgotPassword"

//Registering Management Pages
import UsersPage from "@/pages/management/UsersPage";
import FeedbackPage from "@/pages/management/FeedbackPage";

//Registering Operation Pages
import OperationPage from "@/pages/operation/OperationPage";

//Registering Planning Pages
import HistoryPlanningPage from "@/pages/planning/HistoryPlanningPage";
import PlanArrangementsPage from "@/pages/planning/PlanArrangementsPage";

//Registering Clinic Pages
import ClinicPage from "@/pages/clinic/ClinicPage";

export default new VueRouter({
  mode: 'history',
  routes: [
    {
      path: '/branchespage',
      name: 'BranchesPage',
      component: BranchesPage
    },
    {
      path: '/clinicpage',
      name: 'ClinicPage',
      component: ClinicPage
    },
    {
      path: '/dashboardpage',
      name: 'DashboardPage',
      component: DashboardPage,
      alias: ['']
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
      path: '/feedbackpage',
      name: 'FeedbackPage',
      component: FeedbackPage
    },
    {
      path: '/loginpage',
      name: 'LoginPage',
      component: LoginPage,
      meta: {
        fullPage: true
      }
    },
    {
      path: '/resetpassword',
      name: 'ResetPassword',
      component: ResetPassword,
      meta: {
        fullPage: true
      }
    },
    {
      path: '/forgotpassword',
      name: 'ForgotPassword',
      component: ForgotPassword,
      meta: {
        fullPage: true
      }
    },
    {
      path: '/operationpage',
      name: 'OperationPage',
      component: OperationPage
    },
    {
      path: '/historyplanningpage',
      name: 'HistoryPlanningPage',
      component: HistoryPlanningPage
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
      path: '/personneltitlepage',
      name: 'PersonnelTitlePage',
      component: PersonnelTitlePage
    },
    {
      path: '/operatingroomcalendarpage',
      name: 'OperatingRoomCalendarPage',
      component: OperatingRoomCalendarPage
    },
    {
      path: '/operatingroomspage',
      name: 'OperatingRoomsPage',
      component: OperatingRoomsPage
    },
    {
      path: '/planarrangementspage',
      name: 'PlanArrangementsPage',
      component: PlanArrangementsPage
    },
    {
      path: '/userspage',
      name: 'UsersPage',
      component: UsersPage
    },
    {
      path: '/worktypespage',
      name: 'WorkTypesPage',
      component: WorkTypesPage
    },
  ]
})
