<template>
<div>
  <div id="app"
       v-show= isMounted
       v-if="$route.meta.fullPage">
    <v-app id="insipere">
      <router-view></router-view>
    </v-app>
  </div>

  <div id="app"
       v-show= isMounted
       v-else>
    <v-app id="insipere">
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
                <a href="#!" class="body-2 black--text">EDIT</a>
              </v-flex>
            </v-layout>
<!-- Alt gruplama varsa -->
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
                  <v-icon color="primary"> {{ child.icon }} </v-icon>
                </v-list-tile-action>

                <v-list-tile-content>
                  <v-list-tile-title>
                    {{ child.text }}
                  </v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </v-list-group>
<!-- Alt gruplama yoksa -->
            <v-list-tile v-else
                         :key="item.text"
                         @click="changePages(item.route)">
              <v-list-tile-action>
                <v-icon color="primary"> {{ item.icon }} </v-icon>
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

      <v-toolbar color="blue darken-3"
                 dark
                 app
                 :clipped-left="$vuetify.breakpoint.mdAndUp"
                 fixed>
        <v-toolbar-title style="width: 300px"
                         class="ml-0 pl-3">
          <v-toolbar-side-icon @click.stop="drawer = !drawer">
          </v-toolbar-side-icon>
<!-- Toolbardaki yazı -->
          <span class="hidden-sm-and-down"
                @click="changePages('/dashboardpage')">
                Surgicalogic
          </span>
        </v-toolbar-title>
<!-- Anasayfa iconu -->
        <v-btn icon @click="changePages('/dashboardpage')">
          <v-icon>home</v-icon>
        </v-btn>
<!-- Search -->
        <v-text-field flat
                      solo-inverted
                      prepend-icon="search"
                      label="Search"
                      class="hidden-sm-and-down">
        </v-text-field>
<!-- Searchden sonra boşluk bırakma -->
        <v-spacer></v-spacer>

        <v-btn icon>
          <v-icon>apps</v-icon>
        </v-btn>

        <v-btn icon>
          <v-icon>notifications</v-icon>
        </v-btn>

        <v-btn icon>
          <v-icon>language</v-icon>
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
          text: "Tanımlar",
          model: true,
          children: [
            {
              icon: "storage",
              text: "Ekipmanlar",
              text: vm.$i18n.t("equipments.equipments"),
              route: "/equipmentspage"
            },
            {
              icon: "group",
              text: vm.$i18n.t("personnel.personnel"),
              route: "/personnelpage"
            },
            {
              icon: "assignment",
              text: vm.$i18n.t("rooms.rooms"),
              route: "/roomspage",
            },
            {
              icon: "announcement",
              text: "Koşullar"
              // route: "/CreatePlan"
            },
            {
              icon: "domain",
              text: "Klinik"
              // route: "/CreatePlan"
            }
          ]
        },
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          text: "Operasyon Yönetimi",
          children: [
            {
              icon: "add_comment",
              text: "Yeni Operasyon",
              // route: "/CreatePlan"
            },
            {
              icon: "edit",
              text: "Operayon Düzenleme",
              // route: "/CreatePlan"
            }
          ]
        },
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          text: "Plan Yönetimi",
          children: [
            {
              icon: "add_alarm",
              text: "Plan Düzenlemeleri",
              // route: "/CreatePlan"
            },
            {
              icon: "history",
              text: "Plan Tarihçesi",
              // route: "/CreatePlan"
            }
          ]
        },
        {
          icon: "domain",
          text: "Klinik Yönetimi",
          // route: "/eventcalendarpage"
        },
        {
          icon: "content_copy",
          text: "Raporlar",
          // route: "/eventcalendarpage"
        },
        {
          icon: "event",
          text: "Simülasyon",
          // route: "/eventcalendarpage"
        },
        {
          icon: "keyboard_arrow_up",
          "icon-alt": "keyboard_arrow_down",
          text: "Yönetim Paneli",
          children: [
            {
              icon: "group",
              text: "Kullanıcılar",
              // route: "/CreatePlan"
            },
            {
              icon: "chat_bubble",
              text: "Geri Bildirim",
              // route: "/CreatePlan"
            },
            {
              icon: "help",
              text: "Yardım",
              // route: "/CreatePlan"
            },
            {
              icon: "settings",
              text: "Ayarlar",
              // route: "/CreatePlan"
            }
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
  }
</style>

