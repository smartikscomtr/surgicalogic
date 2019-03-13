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
import PatientPage from "@/pages/patient/PatientPage";
import PersonnelCategoryPage from "@/pages/definitions/PersonnelCategoryPage";
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
import SettingsPage from "@/pages/management/SettingsPage";

//Registering Operation Pages
import OperationPage from "@/pages/operation/OperationPage";

//Registering Planning Pages
import PlanArrangementsPage from "@/pages/planning/PlanArrangementsPage";

//Registering Clinic Pages
import AppointmentListPage from "@/pages/clinic/AppointmentListPage";
import ClinicPage from "@/pages/clinic/ClinicPage";
import AppointmentCalendarPage from "@/pages/clinic/AppointmentCalendarPage";

//Registering report Pages
import HistoryClinicPage from "@/pages/report/HistoryClinicPage";
import HistoryPlanningPage from "@/pages/report/HistoryPlanningPage";
import OvertimeUtilizationPage from "@/pages/report/OvertimeUtilizationPage";
import OvertimeReportPage from "@/pages/report/OvertimeReportPage";

//Registering error Pages
import ApiErrorPage from "@/pages/error/ApiErrorPage";
import ForbiddenErrorPage from "@/pages/error/ForbiddenErrorPage";

export default new VueRouter({
  mode: 'history',
  routes: [
    {
      path: '/appointmentListPage',
      name: 'AppointmentListPage',
      component: AppointmentListPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/appointmentcalendarpage',
      name: 'AppointmentCalendarPage',
      component: AppointmentCalendarPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/branchespage',
      name: 'BranchesPage',
      component: BranchesPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/clinicpage',
      name: 'ClinicPage',
      component: ClinicPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/dashboardpage',
      name: 'DashboardPage',
      component: DashboardPage,
      alias: [''],
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/equipmentspage',
      name: 'EquipmentsPage',
      component: EquipmentsPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/equipmenttypespage',
      name: 'EquipmentTypesPage',
      component: EquipmentTypesPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/feedbackpage',
      name: 'FeedbackPage',
      component: FeedbackPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/overtimeutilizationpage',
      name: 'OvertimeUtilizationPage',
      component: OvertimeUtilizationPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/overtimereportpage',
      name: 'OvertimeReportPage',
      component: OvertimeReportPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/historyclinicpage',
      name: 'HistoryClinicPage',
      component: HistoryClinicPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/historyplanningpage',
      name: 'HistoryPlanningPage',
      component: HistoryPlanningPage,
      meta: {
        requiresAuth: true
      }
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
      path: '/apierrorpage',
      name: 'ApiErrorPage',
      component: ApiErrorPage,
      meta: {
        fullPage: true
      }
    },
    {
      path: '/forbiddenerrorpage',
      name: 'ForbiddenErrorPage',
      component: ForbiddenErrorPage,
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
      component: OperationPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/operationtypespage',
      name: 'OperationTypesPage',
      component: OperationTypesPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/personnelpage',
      name: 'PersonnelPage',
      component: PersonnelPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/patientpage',
      name: 'PatientPage',
      component: PatientPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/personnelCategorypage',
      name: 'PersonnelCategoryPage',
      component: PersonnelCategoryPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/personnelTitlepage',
      name: 'PersonnelTitlePage',
      component: PersonnelTitlePage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/operatingroomcalendarpage',
      name: 'OperatingRoomCalendarPage',
      component: OperatingRoomCalendarPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/operatingroomspage',
      name: 'OperatingRoomsPage',
      component: OperatingRoomsPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/planarrangementspage',
      name: 'PlanArrangementsPage',
      component: PlanArrangementsPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/userspage',
      name: 'UsersPage',
      component: UsersPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/settingspage',
      name: 'SettingsPage',
      component: SettingsPage,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/worktypespage',
      name: 'WorkTypesPage',
      component: WorkTypesPage,
      meta: {
        requiresAuth: true
      }
    }
  ]
})
