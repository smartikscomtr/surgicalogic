<template>
  <div>
    <!-- Login Page -->
    <div id="app" v-show=isMounted v-if="$route.meta.fullPage">
      <v-app id="insipere">
        <router-view></router-view>
      </v-app>
    </div>

    <!-- Dashboard Page -->
    <div id="app" v-show=isMounted v-else>
      <v-app id="insipere">
        <!-- Navigation -->
        <v-navigation-drawer class="navigation" fixed :clipped="$vuetify.breakpoint.mdAndUp" app v-model="drawer">
          <v-list dense>
            <template v-for="item in items">
              <v-layout row v-if="item.heading" align-center :key="item.heading">
                <v-flex xs6>
                  <v-subheader v-if="item.heading">
                    {{ item.heading }}
                  </v-subheader>
                </v-flex>

                <v-flex xs6 class="text-xs-center">
                  <a href="#!" class="body-2 black--text">
                    EDIT
                  </a>
                </v-flex>
              </v-layout>

              <!-- Subgroup -->
              <v-list-group v-else-if="item.children" v-model="item.active" :key="item.text" :prepend-icon="item.model ? item.icon : item['icon-alt']" append-icon="">
                <v-list-tile slot="activator">
                  <v-list-tile-content>
                    <v-list-tile-title>
                      {{ item.text }}
                    </v-list-tile-title>
                  </v-list-tile-content>
                </v-list-tile>

                <v-list-tile :class="child.route === $route.path ? 'highlighted' : ''" v-for="(child, i) in item.children" :key="i" @click="changePages(child.route)" @mousedown.middle="newTab(child.route)">
                  <v-list-tile-action v-if="child.icon">
                    <v-icon>
                      {{ child.icon }}
                    </v-icon>
                  </v-list-tile-action>

                  <v-list-tile-content>
                    <v-list-tile-title>
                      {{ child.text }}
                    </v-list-tile-title>
                  </v-list-tile-content>
                </v-list-tile>
              </v-list-group>

              <!-- Not Subgroup -->
              <v-list-tile :class="item.route === $route.path ? 'highlighted' : ''" v-else :key="item.text" @click="changePages(item.route)" @mousedown.middle="newTab(item.route)">
                <v-list-tile-action>
                  <v-icon>
                    {{ item.icon }}
                  </v-icon>
                </v-list-tile-action>

                <v-list-tile-content>
                  <v-list-tile-title>
                    {{ item.text }}
                  </v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
          </v-list>
        </v-navigation-drawer>

        <!-- Toolbar -->
        <v-toolbar color="teal" height="48px" dark app :clipped-left="$vuetify.breakpoint.mdAndUp" fixed>
          <v-toolbar-title style="width: 300px" class="ml-0">
            <v-toolbar-side-icon @click.stop="drawer = !drawer">
            </v-toolbar-side-icon>

            <!-- Toolbar text-->
            <span class="hidden-sm-and-down" style="cursor: pointer;" @click="changePages('/dashboardpage')">
              SurgicaLogic
            </span>
          </v-toolbar-title>

          <v-spacer></v-spacer>

          <v-menu offset-y>
            <span slot="activator">
              <span class="userName">
                {{ username }}
              </span>

              <v-btn flat icon dark>
                  <v-icon>arrow_drop_down</v-icon>
              </v-btn>
            </span>

            <v-list>
              <v-list-tile @click="logOut">{{ $t('common.logout') }}</v-list-tile>
            </v-list>
          </v-menu>

          <v-tooltip bottom>
            <v-btn icon @click="showFeedback = true" slot="activator">
              <v-icon>
                chat_bubble
              </v-icon>
            </v-btn>

            <span>{{ $t('feedbacks.feedback') }}</span>
          </v-tooltip>

          <feedback-component v-if="showFeedback" @showModal="showFeedbackMethod">
          </feedback-component>

          <error-dialog-component v-if="showErrorDialog">
          </error-dialog-component>

          <v-menu bottom offset-y origin="center center" transition="scale-transition">

            <!-- <v-tooltip z-index="10" bottom slot="activator"> -->
            <v-btn icon flat slot="activator" dark>
              <v-icon>language</v-icon>
            </v-btn>
            <!-- <span>{{ $t('menu.changeLanguage') }}</span>
        </v-tooltip> -->

            <v-list>
              <v-list-tile :color="item.selected ? 'teal' : ''" v-for="(item, index) in languages" :key="index" @click="changeLanguage(item.code)">
                <v-list-tile-title><img align="center" :src="'/static/images/languages/' + item.icon " /> {{item.name}}</v-list-tile-title>
              </v-list-tile>
              <!-- <v-list-tile @click="changeLanguage('en')">
                <v-list-tile-title><img align="center" src="/static/images/languages/en.png" /> EN</v-list-tile-title>
              </v-list-tile> -->
            </v-list>
          </v-menu>

        </v-toolbar>

        <v-content>
          <main>
            <router-view>
            </router-view>
          </main>
        </v-content>
      </v-app>
    </div>
  </div>
</template>

<script>
import EventBus from './event-bus';

export default {
    props: {
        source: String
    },

    data() {
        return {
            dialog: false,
            isMounted: false,
            drawer: null,
            showFeedback: false,
            showErrorDialog: false,
            languages: [
                {
                    code: 'tr',
                    name: 'TR',
                    icon: 'tr.png',
                    selected: false
                },
                {
                    code: 'en',
                    name: 'EN',
                    icon: 'en.png',
                    selected: false
                }
            ]
        };
    },

    computed: {
        items() {
            const vm = this;

            let definitions = [
                        {
                            icon: 'build',
                            text: vm.$i18n.t('menu.equipments'),
                            route: '/equipmentspage'
                        },
                        {
                            icon: 'group',
                            text: vm.$i18n.t('menu.personnel'),
                            route: '/personnelpage'
                        },
                        {
                            icon: 'meeting_room',
                            text: vm.$i18n.t('menu.operatingRooms'),
                            route: '/operatingroomspage'
                        },
                        {
                            icon: 'bookmarks',
                            text: vm.$i18n.t('menu.branches'),
                            route: '/branchespage'
                        },
                        {
                            icon: 'person_pin',
                            text: vm.$i18n.t('menu.PersonnelCategories'),
                            route: '/personnelcategorypage'
                        },
                        {
                            icon: 'grade',
                            text: vm.$i18n.t('menu.PersonnelTitles'),
                            route: '/personneltitlepage'
                        },
                        {
                            icon: 'business_center',
                            text: vm.$i18n.t('menu.equipmentTypes'),
                            route: '/equipmenttypespage'
                        },
                        {
                            icon: 'work_outline',
                            text: vm.$i18n.t('menu.operationTypes'),
                            route: '/operationtypespage'
                        },
                        {
                            icon: 'group_work',
                            text: vm.$i18n.t('menu.workTypes'),
                            route: '/worktypespage'
                        }
                        // {
                        //   icon: "domain",
                        //   text: "Klinik"
                        //   // route: "/CreatePlan"
                        // }
                    ];

            let reports = [
                        {
                            icon: 'timer_off',
                            text: vm.$i18n.t('menu.overtimeReport'),
                            route: '/overtimereportpage'
                        },
                        {
                            icon: 'history',
                            text: vm.$i18n.t('menu.planningHistoryReport'),
                            route: '/historyplanningpage'
                        },
                        {
                            icon: 'history',
                            text: vm.$i18n.t('menu.clinicHistoryReport'),
                            route: '/historyclinicpage'
                        },
                        {
                            icon: 'hourglass_full',
                            text: vm.$i18n.t('menu.overtimeUtilizationReport'),
                            route: '/overtimeutilizationpage'
                        }
                    ];

            let managements = [
                        {
                            icon: 'group',
                            text: vm.$i18n.t('menu.users'),
                            route: '/userspage'
                        },
                        {
                            icon: 'chat_bubble',
                            text: vm.$i18n.t('menu.feedback'),
                            route: '/feedbackpage'
                        },
                        {
                            icon: 'settings',
                            text: vm.$i18n.t('menu.settings'),
                            route: '/settingspage'
                        }
                    ];



            return [
                {
                    icon: 'add_alarm',
                    text: vm.$i18n.t('menu.operations'),
                    route: '/operationpage'
                },
                {
                    icon: 'group',
                    text: vm.$i18n.t('menu.patient'),
                    route: '/patientpage'
                },
                {
                  icon: 'timeline',
                  text: vm.$i18n.t('menu.planArrangementsAndSimulation'),
                  route: '/planarrangementspage'
                },
                {
                    icon: 'domain',
                    text: vm.$i18n.t('menu.clinicManagement'),
                    route: '/clinicpage'
                },
                {
                    icon: 'keyboard_arrow_up',
                    'icon-alt': 'keyboard_arrow_down',
                    text: vm.$i18n.t('menu.reports'),
                    children: reports,
                    active: reports.some(x => x.route === vm.$route.path)
                },
                // {
                //     icon: 'event',
                //     text: vm.$i18n.t('menu.simulation'),
                //     route: "/simulationpage"
                // },
                {
                    icon: 'keyboard_arrow_up',
                    'icon-alt': 'keyboard_arrow_down',
                    text: vm.$i18n.t('menu.managementPanel'),
                    children: managements,
                    active: managements.some(x => x.route === vm.$route.path)
                },
                {
                    icon: 'keyboard_arrow_up',
                    'icon-alt': 'keyboard_arrow_down',
                    text: vm.$i18n.t('menu.definitions'),
                    children: definitions,
                    active: definitions.some(x => x.route === vm.$route.path)
                }
            ];
        },

        username() {
          return localStorage.getItem("username");
        }
    },

    methods: {
        showFeedbackMethod() {
            const vm = this;

            return (vm.showFeedback = false);
        },

        changePages(route) {
            const vm = this;

            return vm.$router.push(route);
        },

        newTab(route) {
          window.open(route, "_blank");
        },

        changeLanguage(lang) {
            const vm = this;

            for (let index = 0; index < vm.languages.length; index++) {
                const element = vm.languages[index];
                element.selected = element.code == lang;
            }

            vm.$i18n.locale = lang;
            vm.$cookie.set('currentLanguage', lang, 365);
        },

        logOut() {
          const vm = this;

          vm.$store.dispatch("userLogout").then(() => {
            vm.$router.push("/loginpage");
          });
        },

      emitGlobalClickEvent() {
      this.clickCount++;
      // Send the event on a channel (i-got-clicked) with a payload (the click count.)
      EventBus.$emit('i-got-clicked', this.clickCount);
      }
    },

    mounted() {
        const vm = this;

        vm.isMounted = true;

        var currentLanguage = vm.$cookie.get('currentLanguage');

        if (currentLanguage == null) {
            vm.$cookie.set('currentLanguage', vm.$i18n.locale, 365);
        } else {
            vm.changeLanguage(currentLanguage);
        }

        EventBus.$on('apiErrorDialog', function(number){
          vm.showErrorDialog = true;
        })
    }
};
</script>
