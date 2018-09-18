<template>
  <div>
    <!-- Login Page -->
    <div id="app"
         v-show= isMounted
         v-if="$route.meta.fullPage">
      <v-app id="insipere">
        <router-view></router-view>
      </v-app>
    </div>

    <!-- Dashboard Page -->
    <div id="app"
         v-show= isMounted
         v-else>
      <v-app id="insipere">
        <!-- Navigation -->
        <v-navigation-drawer class="navigation"
                             fixed
                             :clipped="$vuetify.breakpoint.mdAndUp"
                             app
                             v-model="drawer">
          <v-list dense>
            <template v-for="item in items">
              <v-layout row
                        v-if="item.heading"
                        align-center
                        :key="item.heading">
                <v-flex xs6>
                  <v-subheader v-if="item.heading">
                    {{ item.heading }}
                  </v-subheader>
                </v-flex>

                <v-flex xs6 class="text-xs-center">
                  <a href="#!"
                     class="body-2 black--text">
                    EDIT
                  </a>
                </v-flex>
              </v-layout>

              <!-- Subgroup -->
              <v-list-group v-else-if="item.children"
                            v-model="item.model"
                            :key="item.text"
                            :prepend-icon="item.model ? item.icon : item['icon-alt']"
                            append-icon="">
                <v-list-tile slot="activator">
                  <v-list-tile-content>
                    <v-list-tile-title>
                      {{ item.text }}
                    </v-list-tile-title>
                  </v-list-tile-content>
                </v-list-tile>

                <v-list-tile v-for="(child, i) in item.children"
                            :key="i"
                            @click="changePages(child.route)">
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
              <v-list-tile v-else
                          :key="item.text"
                          @click="changePages(item.route)">
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
        <v-toolbar color="teal"
                  dark
                  app
                  :clipped-left="$vuetify.breakpoint.mdAndUp"
                  fixed>
          <v-toolbar-title style="width: 300px"
                          class="ml-0">
            <v-toolbar-side-icon @click.stop="drawer = !drawer">
            </v-toolbar-side-icon>

            <!-- Toolbar text-->
            <span class="hidden-sm-and-down"
                  @click="changePages('/dashboardpage')">
                  Surgicalogic
            </span>
          </v-toolbar-title>

          <!-- Dashboard icon -->
          <v-btn icon @click="changePages('/dashboardpage')">
            <v-icon>
              home
            </v-icon>
          </v-btn>

          <!-- Search -->
          <v-text-field class="search-wrap"
                        flat
                        solo-inverted
                        append-icon="search"
                        :label="$i18n.t('common.search')"
                        single-line hide-details>
          </v-text-field>

          <v-spacer></v-spacer>

          <v-btn icon>
            <v-icon>
              notifications
            </v-icon>
          </v-btn>

          <v-tooltip bottom>
            <v-btn icon
                  @click="showFeedback = true"
                  slot="activator">
              <v-icon>
                chat_bubble
              </v-icon>
            </v-btn>

            <span>{{ $t('feedbacks.feedback') }}</span>
          </v-tooltip>

          <feedback-component v-if="showFeedback"
                              @showModal="showFeedbackMethod">
          </feedback-component>

          <v-btn icon>
            <v-icon>
              language
            </v-icon>
          </v-btn>
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
      showFeedback: false
    }
  },

  computed: {
    items() {
      const vm = this;

      return [
        {
          icon: "add_alarm",
          text: "Operasyonlar",
          route: "/operationpage"
        },
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          text: vm.$i18n.t('menu.planManagement'),
          children: [
            {
              icon: "timeline",
              text: vm.$i18n.t('menu.planArrangements'),
              route: "/planarrangementspage"
            },
            {
              icon: "history",
              text: vm.$i18n.t('menu.planningHistory'),
              route: "/historyplanningpage"
            }
          ]
        },
        {
          icon: "domain",
          text: vm.$i18n.t('menu.clinicManagement'),
          route: "/clinicpage"
        },
        {
          icon: "content_copy",
          text: vm.$i18n.t('menu.reports'),
          // route: "/eventcalendarpage"
        },
        {
          icon: "event",
          text: vm.$i18n.t('menu.simulation'),
          // route: "/eventcalendarpage"
        },
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          text: vm.$i18n.t('menu.managementPanel'),
          children: [
            {
              icon: "group",
              text: vm.$i18n.t("menu.users"),
              route: "/userspage"
            },
            {
              icon: "chat_bubble",
              text: vm.$i18n.t("menu.feedback"),
              route: "/feedbackpage"
            },
            {
              icon: "help",
              text: vm.$i18n.t("menu.help"),
              // route: "/CreatePlan"
            },
            {
              icon: "settings",
              text: vm.$i18n.t("menu.settings"),
              // route: "/CreatePlan"
            }
          ]
        },
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          text: vm.$i18n.t("menu.definitions"),
          children: [
            {
              icon: "build",
              text: vm.$i18n.t("menu.equipments"),
              route: "/equipmentspage"
            },
            {
              icon: "group",
              text: vm.$i18n.t("menu.personnel"),
              route: "/personnelpage"
            },
            {
              icon: "meeting_room",
              text: vm.$i18n.t("menu.operatingRooms"),
              route: "/operatingroomspage"
            },
            {
              icon: "bookmarks",
              text: vm.$i18n.t("menu.branches"),
              route: "/branchespage"
            },
            {
              icon: "person_pin",
              text: vm.$i18n.t("menu.personnelTitles"),
              route: "/personneltitlepage"
            },
            {
              icon: "business_center",
              text: vm.$i18n.t("menu.equipmentTypes"),
              route: "/equipmenttypespage"
            },
            {
              icon: "work_outline",
              text: vm.$i18n.t("menu.operationTypes"),
              route: "/operationtypespage"
            },
            {
              icon: "group_work",
              text: vm.$i18n.t("menu.workTypes"),
              route: "/worktypespage"
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

      return vm.showFeedback = false;
    },

    changePages(route) {
      const vm = this;

      return vm.$router.push(route);
    }
  },

  mounted() {
    const vm = this;

    vm.isMounted = true;
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
</style>

