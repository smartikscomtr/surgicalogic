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
          <v-text-field flat
                        solo-inverted
                        prepend-icon="search"
                        :label="$i18n.t('common.search')"
                        class="hidden-sm-and-down">
          </v-text-field>

          <v-spacer></v-spacer>

          <v-btn icon>
            <v-icon>
              apps
            </v-icon>
          </v-btn>

          <v-btn icon>
            <v-icon>
              notifications
            </v-icon>
          </v-btn>

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

  data: () => ({
    dialog: false,
    isMounted: false,
    drawer: null
  }),

  computed: {
    items() {
      const vm = this;

      return [
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          // model: true,
          text: vm.$i18n.t('menu.operationManagement'),
          children: [
            {
              icon: "add_comment",
              text: vm.$i18n.t('menu.operations'),
              route: "/operationpage"
            }
          ]
        },
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          text: vm.$i18n.t('menu.planManagement'),
          children: [
            {
              icon: "add_alarm",
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
          // route: "/eventcalendarpage"
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
              // route: "/CreatePlan"
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
              icon: "storage",
              text: vm.$i18n.t("menu.equipments"),
              route: "/equipmentspage"
            },
            {
              icon: "group",
              text: vm.$i18n.t("menu.personnel"),
              route: "/personnelpage"
            },
            {
              icon: "assignment",
              text: vm.$i18n.t("menu.operatingRooms"),
              route: "/operatingroomspage"
            },
            {
              icon: "assignment",
              text: vm.$i18n.t("menu.branches"),
              route: "/branchespage"
            },
            {
              icon: "assignment",
              text: vm.$i18n.t("menu.personnelTitles"),
              route: "/personneltitlepage"
            },
            {
              icon: "assignment",
              text: vm.$i18n.t("menu.equipmentTypes"),
              route: "/equipmenttypespage"
            },
            {
              icon: "announcement",
              text: vm.$i18n.t("menu.operationTypes"),
              route: "/operationtypespage"
            },
            {
              icon: "assignment",
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
        // {
        //   icon: "event",
        //   text: "Event Calendar",
        //   route: "/eventcalendarpage"
        // },
        // {
        //   icon: "history",
        //   text: "Operations Database",
        //   route: "History"
        // },
        // {
        //   icon: "content_copy",
        //   text: "Reports",
        //   route: "/reportspage"
        // },
        // {
        //   icon: "keyboard_arrow_up",
        //   "icon-alt": "keyboard_arrow_down",
        //   text: "Planning",
        //   children: [
        //     {
        //       icon: "add_alarm",
        //       text: "Create New Plan",
        //       route: "/CreatePlan"
        //     }
        //   ]
        // },
        // {
        //   icon: "keyboard_arrow_up",
        //   "icon-alt": "keyboard_arrow_down",
        //   text: "Inventory",
        //   children: [
        //     {
        //       icon: "domain",
        //       text: "Operation Room",
        //       route: "/operationroompage"
        //     },
        //     // { icon: "", text: "Operation Type", route: "/operationtypepage" },
        //     {
        //       icon: "storage",
        //       text: "Items",
        //       route: "itemspage"
        //     }
        //   ]
        // },
        // {
        //   icon: "group",
        //   text: "Users",
        //   route: "/userspage"
        //   // children: [
        //   //   { icon: "pageview", text: "View", route: "/viewpage" },
        //   //   { icon: "person_add", text: "Add New User", route: "/addnewuserpage" },
        //   //   { icon: "delete", text: "Delete User", route: "/deleteuserpage" }
        //   // ]
        // },
        // {
        //   icon: "settings",
        //   text: "Settings",
        //   route: "/settingspage"
        // },
        // {
        //   icon: "chat_bubble",
        //   text: "Send feedback",
        //   route: "SendFeedback"
        // },
        // {
        //   icon: "help",
        //   text: "Help",
        //   route: "/Help"
        // }
      ];
    }
  },

  methods: {
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

