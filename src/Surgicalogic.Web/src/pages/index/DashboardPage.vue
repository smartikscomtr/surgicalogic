<template>
  <div>
    <v-container grid-list-md>
      <v-layout
        row
        wrap
        class="cards-wrap"
      >
        <v-flex
          xs12
          sm6
          md3
        >
          <v-card class="operation-wrap">
            <v-card-title>
              <div>
                <v-icon
                  dark
                  x-large
                >add_alarm</v-icon>
                <p class="headline">{{ $t('menu.operations') }} </p>
              </div>
              <p>{{ $t('menu.youCanManageYourOperations') }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn
                small
                @click="$router.push('/operationpage')"
              >
                <span class="text text-wrap"
                      lang="tr">
                  {{ $t('common.go') }}
                </span>
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-flex>

        <v-flex
          xs12
          sm6
          md3
        >
          <v-card class="plan-wrap">
            <v-card-title>
              <div>
                <v-icon
                  dark
                  x-large
                >timeline</v-icon>
                <p class="headline">{{ $t('menu.plans') }} </p>
              </div>
              <p>{{ $t('menu.youCanManageYourPlans') }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn
                small
                @click="$router.push('/planarrangementspage')"
              >
                <span class="text text-wrap"
                      lang="tr">
                  {{ $t('common.go') }}
                </span>
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-flex>

        <v-flex
          xs12
          sm6
          md3
        >
          <v-card class="clinic-wrap">
            <v-card-title>
              <div>
                <v-icon
                  dark
                  x-large
                >domain</v-icon>
                <p class="headline">{{ $t('menu.clinicManagement') }} </p>
              </div>
              <p>{{ $t('menu.youCanManageYourClinics') }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn
                small
                @click="$router.push('/clinicpage')"
              >
                <span class="text text-wrap"
                      lang="tr">
                  {{ $t('common.go') }}
                </span>
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-flex>

        <v-flex
          xs12
          sm6
          md3
        >
          <v-card class="personnel-wrap">
            <v-card-title>
              <div>
                <v-icon
                  dark
                  x-large
                >group</v-icon>
                <p class="headline">{{ $t('menu.personnelManagement') }} </p>
              </div>
              <p>{{ $t('menu.youCanManageYourPersonnels') }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn
                small
                @click="$router.push('/personnelpage')"
              >
                <span class="text text-wrap"
                      lang="tr">
                  {{ $t('common.go') }}
                </span>
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-flex>

        <v-flex
          xs12
          sm6
          md3
        >
          <v-menu
            ref="menu"
            :close-on-content-click="false"
            v-model="menu"
            :nudge-right="40"
            :return-value.sync="date"
            lazy
            transition="scale-transition"
            offset-y
            full-width
            min-width="290px"
          >
            <v-text-field
              append-icon="keyboard_arrow_down"
              readonly
              slot="activator"
              v-model="dateFormatted"
              :label="$t('operation.operationDate')"
            >
            </v-text-field>

            <v-date-picker
              v-model="date"
              no-title
              :locale="getLocale()"
              @input="$refs.menu.save(date)"
              :max="getMaxDate()"
            >
            </v-date-picker>
          </v-menu>
        </v-flex>


        <v-flex
          xs12
          sm6
          md9
          class="dashboard-page-btn">
            <v-btn
              :disabled="!moving === true"
              v-show="date"
              class="updateplan-wrap orangeButton"
              @click.native="updatePlan()"
            >
              {{ $t('planarrangements.updatePlan')}}
            </v-btn>
        </v-flex>
      </v-layout>

      <div
        id="visualization"
        class="vis"
      >
      </div>

      <div
        id="serializedTimeline"
        style="display:none;"
      >
      </div>
    </v-container>

    <snackbar-component
      :snackbar-visible="snackbarVisible"
      :savedMessage="savedMessage"
    >
    </snackbar-component>

  </div>
</template>

<script>
import Vis from "vis/dist/vis.js";
import { localizationMixin } from './../../mixins/localizationMixin';

export default {
  mixins: [
    localizationMixin
  ],

  data() {
    const vm = this;

    return {
      menu: false,
      dateFormatted: null,
      date: null,
      snackbarVisible: null,
      savedMessage: this.$i18n.t("planarrangements.planUpdated"),
      moving: null
    };
  },

  watch: {
    showModal(val) {
      val || this.cancel();
    },

    date(val) {
      this.dateFormatted = this.formatDate(this.date);
    }
  },

  methods: {
    getMaxDate() {
      const toTwoDigits = num => (num < 10 ? "0" + num : num);
      let selectDay = new Date();

      selectDay.setDate(selectDay.getDate());

      let year = selectDay.getFullYear();
      let month = toTwoDigits(selectDay.getMonth() + 1);
      let day = toTwoDigits(selectDay.getDate());

      return `${year}-${month}-${day}`;
    },

    getDate() {
      const toTwoDigits = num => (num < 10 ? "0" + num : num);
      let today = new Date();
      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let day = toTwoDigits(today.getDate());

      return `${year}-${month}-${day}`;
    },

    formatDate(date) {
      if (!date || date.indexOf(".") > -1) return null;

      const [year, month, day] = date.split("-");

      return `${day}.${month}.${year}`;
    },

    updatePlan() {
      const vm = this;

      vm.snackbarVisible = false;

      var timelineItems = JSON.parse(
        document.getElementById("serializedTimeline").innerHTML
      );

      var operations = [];

      for (var data in timelineItems._data) {
        var item = timelineItems._data[data];
        //ilk ve son başlangıç ve bitiş tarihleri
        var newStart = new Date(item.start);
        var newEnd = new Date(item.end);

        var operationLength = (newEnd.getTime() - newStart.getTime()) / 60000;

        var operation = {
          id: item.operationPlanId,
          operationId: data,
          start: newStart,
          roomId: item.group,
          length: operationLength
        };

        operations.push(operation);
      }

      vm.$store
        .dispatch("updatePlanArrangements", JSON.stringify(operations))
        .then(response => {
          vm.snackbarVisible = true;
          vm.moving = false;

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300);
        });
    }
  },

  mounted() {
    const vm = this;

    vm.$watch("date", (newValue, oldValue) => {
      if (newValue !== oldValue) {
        var container = document.getElementById("visualization");

        vm.$store
          .dispatch("getDashboardTimelinePlans", {
            selectDate: vm.date
          })
          .then(response => {
            var items = new Vis.DataSet(
              vm.$store.state.planArrangementsModule.model.plan
            );
            var groups = new Vis.DataSet(
              vm.$store.state.planArrangementsModule.model.rooms
            );

            var workingHourStart = new Date(
              vm.$store.state.planArrangementsModule.model.workingHourStart
            );
            var workingHourEnd = new Date(
              vm.$store.state.planArrangementsModule.model.workingHourEnd
            );

            document.getElementById(
              "serializedTimeline"
            ).innerHTML = JSON.stringify(items);

            var options = {
              orientation: {
                axis: "top"
              },
              // timeAxis: { scale: 'minute', step: vm.$store.state.planArrangementsModule.date.period },
              locale: vm.$cookie.get("currentLanguage"),
              moveable: true,
              zoomMax: 86400000,
              zoomMin: 3600000,
              horizontalScroll: true,
              min: vm.$store.state.planArrangementsModule.model.minDate,
              max: vm.$store.state.planArrangementsModule.model.maxDate,
              start: vm.$store.state.planArrangementsModule.model.startDate,
              end: vm.$store.state.planArrangementsModule.model.endDate,
              editable: {
                updateTime: true,
                updateGroup: true
              },
              selectable: true,

              onMoving(item, callback) {
                vm.moving = true;
                var timelineItems = JSON.parse(
                  document.getElementById("serializedTimeline").innerHTML
                );

                timelineItems._data[item.id].start = new Date(item.start);
                timelineItems._data[item.id].end = new Date(item.end);
                timelineItems._data[item.id].group = item.group;

                document.getElementById(
                  "serializedTimeline"
                ).innerHTML = JSON.stringify(timelineItems);

                callback(item);
              }
            };

            container.innerHTML = "";
            var timeline = new Vis.Timeline(container, items, groups, options);
          });
      }
    });

    vm.date = vm.getDate();
  }
};
</script>
