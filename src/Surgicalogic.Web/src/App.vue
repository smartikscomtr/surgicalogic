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
              <v-list-group v-else-if="item.children" v-model="item.model" :key="item.text" :prepend-icon="item.model ? item.icon : item['icon-alt']" append-icon="">
                <v-list-tile slot="activator">
                  <v-list-tile-content>
                    <v-list-tile-title>
                      {{ item.text }}
                    </v-list-tile-title>
                  </v-list-tile-content>
                </v-list-tile>

                <v-list-tile v-for="(child, i) in item.children" :key="i" @click="changePages(child.route)">
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
              <v-list-tile v-else :key="item.text" @click="changePages(item.route)">
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
        <v-toolbar color="teal" dark app :clipped-left="$vuetify.breakpoint.mdAndUp" fixed>
          <v-toolbar-title style="width: 300px" class="ml-0">
            <v-toolbar-side-icon @click.stop="drawer = !drawer">
            </v-toolbar-side-icon>

            <!-- Toolbar text-->
            <span class="hidden-sm-and-down" style="cursor: pointer;" @click="changePages('/dashboardpage')">
              Surgicalogic
            </span>
          </v-toolbar-title>

          <v-spacer></v-spacer>

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

            return [
                {
                    icon: 'add_alarm',
                    text: vm.$i18n.t('menu.operations'),
                    route: '/operationpage'
                },
                {
                    icon: 'keyboard_arrow_up',
                    'icon-alt': 'keyboard_arrow_down',
                    text: vm.$i18n.t('menu.planManagement'),
                    children: [
                        {
                            icon: 'timeline',
                            text: vm.$i18n.t('menu.planArrangements'),
                            route: '/planarrangementspage'
                        }
                    ]
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
                    children: [
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
                    ]
                },
                {
                    icon: 'event',
                    text: vm.$i18n.t('menu.simulation')
                    // route: "/eventcalendarpage"
                },
                {
                    icon: 'keyboard_arrow_up',
                    'icon-alt': 'keyboard_arrow_down',
                    text: vm.$i18n.t('menu.managementPanel'),
                    children: [
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
                            icon: 'help',
                            text: vm.$i18n.t('menu.help')
                            // route: "/CreatePlan"
                        },
                        {
                            icon: 'settings',
                            text: vm.$i18n.t('menu.settings'),
                            route: '/settingspage'
                        }
                    ]
                },
                {
                    icon: 'keyboard_arrow_up',
                    'icon-alt': 'keyboard_arrow_down',
                    text: vm.$i18n.t('menu.definitions'),
                    children: [
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
                            route: '/PersonnelCategorypage'
                        },
                        {
                            icon: 'grade',
                            text: vm.$i18n.t('menu.PersonnelTitles'),
                            route: '/PersonnelTitlepage'
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
                    ]
                }
            ];
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

        changeLanguage(lang) {
            const vm = this;

            for (let index = 0; index < vm.languages.length; index++) {
                const element = vm.languages[index];
                element.selected = element.code == lang;
            }

            vm.$i18n.locale = lang;
            vm.$cookie.set('currentLanguage', lang, 365);
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
    }
};
</script>

<style>
.navigation {
    width: 275px;
    transform: translateX(5px);
    margin-top: 60px;
    background-color: #333 !important;
}
.navigation .v-list__tile__title,
.navigation .v-list__tile__action i,
.navigation .v-list__group__header i {
    color: #fff !important;
}
.v-list__group--active {
    background-color: #262626;
}
.v-list__group--active .v-list__group__header--active {
    background-color: #232222;
}
.v-grid-card .v-card__title {
    padding: 0;
}
.v-icon.close-wrap {
    background-color: #ff7107;
    color: #fff !important;
    border-radius: 3px;
    padding: 5px;
    font-size: 18px;
}
.v-dialog.v-dialog--active,
.v-dialog:not(.v-dialog--fullscreen) {
    max-width: 800px;
    width: 98%;
}
.headline-wrap {
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
}
.headline-wrap .text {
    flex: 1;
    font-size: 18px;
    color: #000;
    margin: 0;
}
.v-card__title {
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex-direction: row;
    margin-bottom: 5px;
}
.orange .v-btn__content {
    color: #fff;
}
.close-wrap {
    color: #ff7107 !important;
}
.orange {
    background-color: #ff7107 !important;
}
.v-dialog:not(.dialog--fullscreen) {
    max-height: inherit;
}
.v-dialog:not(.v-dialog--fullscreen) .v-card__text {
    max-height: 60vh;
    overflow-y: auto;
}
.updateplan-wrap {
    float: right;
}
.v-card__text .btn-wrap {
    float: right;
}
/* .v-dialog .v-card__title {
    padding: 0 20px !important;
} */
.v-card__title button + button {
    margin-left: 1%;
}

#language {
    cursor: pointer;
    margin: 5px;
}
.grid-card.v-card {
    padding: 0 20px;
    margin-bottom: 20px;
}
.appo-picker {
    box-shadow: 0 2px 1px -1px rgba(0, 0, 0, 0.2),
        0 1px 1px 0 rgba(0, 0, 0, 0.14), 0 1px 3px 0 rgba(0, 0, 0, 0.12);
}
.appo-picker.is-large {
    max-width: 100%;
    padding: 20px;
    border-radius: 5px;
}
#app .primary--text {
    color: #ff7107 !important;
}
#app .primary {
    background-color: #009688 !important;
    border-color: #009688 !important;
}
#app .primary--text {
    color: #ff7107 !important;
}
#app .primary--text input,
#app .primary--text textarea {
    caret-color: #009688 !important;
}
#app .primary--after::after {
    background: #009688 !important;
}
.vis-item.vis-range .vis-item-content {
    color: #fff;
}
.vis {
    margin: 20px 0;
}
#app .accent {
    background-color: #ff7107 !important;
    border-color: #ff7107 !important;
}
#app .accent--text {
    color: #ff7107 !important;
}
</style>

