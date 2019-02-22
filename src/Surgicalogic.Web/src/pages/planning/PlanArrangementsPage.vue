<template>
  <div class="container fluid grid-list-md">
    <div class="v-card__text">
      <loading-component :loading="getLoading">
      </loading-component>

      <v-dialog v-model="drawPlanConfirm"
                persistent>
        <v-card class="container fluid grid-list-md">
          <v-card-title class="headline">
            <div class="flex xs12 sm12 md12">
              {{ $t('planarrangements.drawPlanConfirmTitle') }}
            </div>
          </v-card-title>

          <v-card-text>
            <v-layout wrap>
              <div class="flex xs12 sm12 md12">
                {{ $t('planarrangements.drawPlanConfirmText') }}
              </div>
            </v-layout>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>

            <v-btn color="darken-1"
                   flat="flat"
                   @click="drawPlan">
              {{ $t('common.yes') }}
            </v-btn>

            <v-btn color="red darken-1"
                   flat="flat"
                   @click="drawPlanConfirm = false">
              {{ $t('common.no') }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-btn class="orange"
             @click.native="drawPlanConfirm = true">
        {{ $t('planarrangements.drawPlan')}}
      </v-btn>

      <v-btn :disabled="!moving === true"
             class="orange updateplan-wrap"
             @click.native="updatePlan()">
        {{ $t('planarrangements.updatePlan')}}
      </v-btn>

      <div id="visualization"
           class="vis">
      </div>

      <div id="serializedTimeline"
           style="display:none;">
      </div>

      <div>
        <b>{{ $t('planarrangements.sumOvertime')}} :</b> <span id="overtime"></span> {{ $t('planarrangements.minute')}} <br />
        <b>{{ $t('planarrangements.sumUtilization')}} :</b> %<span id="utilization"></span>
      </div>

      <snackbar-component :snackbar-visible="snackbarVisible"
                          :savedMessage="savedMessage">
      </snackbar-component>
    </div>

    <v-tabs v-model="tab"
            color="teal"
            align-with-title>
      <v-tabs-slider color="orange"></v-tabs-slider>

      <v-tab v-for="item in items"
             :key="item.name">
        <span class="span-color"
              lang="tr">
          {{ $t(`${item.name}`) }}
        </span>
      </v-tab>
    </v-tabs>

    <v-tabs-items v-model="tab">
      <v-tab-item v-for="item in items"
                  :key="item.name">
        <v-card flat>
          <v-card-text>
            <tomorrow-planning-component v-if="item.content=='operation'"
                                         ref="operationComponent">
            </tomorrow-planning-component>

            <overtime-utilization-component v-if="item.content=='overtimeUtilization'"
                                            ref="overtimeUtilizationComponent">
            </overtime-utilization-component>

            <simulation-run-component v-if="item.content=='simulation'">
            </simulation-run-component>
          </v-card-text>
        </v-card>
      </v-tab-item>
    </v-tabs-items>
  </div>
</template>

<script>

import Vis from "vis/dist/vis.js";

export default {
  data() {
    const vm = this;

    return {
      tab: null,
      items: [
        { name: "operation.operations", content: "operation" },
        {
          name: "planarrangements.overtimeUtilization",
          content: "overtimeUtilization"
        },
        { name: "simulation.simulation", content: "simulation" }
      ],
      drawPlanConfirm: false,
      snackbarVisible: null,
      savedMessage: vm.$i18n.t("planarrangements.notGenerated"),
      moving: null,
      customParameters: {}
    };
  },

  computed: {
    getLoading() {
      const vm = this;

      return vm.$store.state.planArrangementsModule.loading;
    }
  },

  methods: {
    drawPlan() {
      const vm = this;

      vm.drawPlanConfirm = false;
      vm.$store.dispatch("getGenerateOperationPlan").then(response => {
        if (response.data.hasSolution) {
          vm.getOperationPlan();
          var child = vm.$refs.operationComponent;
          child[0].getOperations()
          child = vm.$refs.overtimeUtilizationComponent;
          child[0].getOvertimeAndUtilizations();
        } else {
          vm.savedMessage = vm.$i18n.t("planarrangements.notGenerated");
          vm.snackbarVisible = true;

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 5000);
        }
      });
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
          vm.savedMessage = vm.$i18n.t("planarrangements.planUpdated");
          vm.snackbarVisible = true;
          vm.moving = false;
          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300);

          var child = vm.$refs.operationComponent;
          child[0].getOperations()
          child = vm.$refs.overtimeUtilizationComponent;
          child[0].getOvertimeAndUtilizations();
        });
    },

    getOperationPlan() {
      const vm = this;

      var container = document.getElementById("visualization");

      vm.$store
        .dispatch("getDashboardTimelinePlans", {
          selectDate: vm.getTomorrowDate()
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

          calcuteOvertimeAndUtilization(
            vm.$store.state.planArrangementsModule.model.startDate,
            groups.length,
            workingHourStart,
            workingHourEnd
          );

          var options = {
            orientation: {
              axis: "top"
            },
            // timeAxis: { scale: 'minute', step: vm.$store.state.planArrangementsModule.model.period },
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

              calcuteOvertimeAndUtilization(
                options.start,
                groups.length,
                workingHourStart,
                workingHourEnd
              );

              callback(item);
            }
          };

          container.innerHTML = "";
          var timeline = new Vis.Timeline(container, items, groups, options);
        });
    },

    getTomorrowDate() {
      const toTwoDigits = num => (num < 10 ? "0" + num : num);
      let selectDay = new Date();

      selectDay.setDate(selectDay.getDate() + 1);

      let year = selectDay.getFullYear();
      let month = toTwoDigits(selectDay.getMonth() + 1);
      let day = toTwoDigits(selectDay.getDate());

      return `${year}-${month}-${day}`;
    }
  },

  mounted() {
    const vm = this;

    vm.getOperationPlan();
  }
};

function setUtilization(value) {
  var result = parseInt(value);

  if (isNaN(result)) {
    result = 0;
  }

  document.getElementById("utilization").innerHTML = result;
}

function setOvertime(value) {
  document.getElementById("overtime").innerHTML = value;
}

function calcuteOvertimeAndUtilization(
  start,
  roomsCount,
  workingHourStart,
  workingHourEnd
) {
  var timelineItems = JSON.parse(
    document.getElementById("serializedTimeline").innerHTML
  );

  var moreThanOvertime = 0;
  var totalOperationLengths = 0;

  var startDate = new Date(start);
  //mesai başlangıcı
  var firstOvertime = new Date(
    startDate.getFullYear(),
    startDate.getMonth(),
    startDate.getDate(),
    workingHourStart.getHours(),
    workingHourStart.getMinutes(),
    0
  );
  //mesai bitişi
  var lastOvertime = new Date(
    startDate.getFullYear(),
    startDate.getMonth(),
    startDate.getDate(),
    workingHourEnd.getHours(),
    workingHourEnd.getMinutes(),
    0
  );

  for (var data in timelineItems._data) {
    var operation = timelineItems._data[data];
    //ilk ve son başlangıç ve bitiş tarihleri
    var newStart = new Date(operation.start);
    var newEnd = new Date(operation.end);

    var operationLength = (newEnd.getTime() - newStart.getTime()) / 60000;

    totalOperationLengths += operationLength;

    var newFirstOverTime =
      newStart < firstOvertime
        ? (firstOvertime - newStart.getTime()) / 60000 > operationLength
          ? operationLength
          : (firstOvertime - newStart.getTime()) / 60000
        : 0;
    var newLastOverTime =
      newEnd > lastOvertime
        ? (newEnd.getTime() - lastOvertime) / 60000 > operationLength
          ? operationLength
          : (newEnd.getTime() - lastOvertime) / 60000
        : 0;

    moreThanOvertime += newFirstOverTime + newLastOverTime;
  }

  var totalAvailableDuration =
    roomsCount * ((lastOvertime - firstOvertime) / 60000);

  setUtilization(totalOperationLengths / totalAvailableDuration * 100);
  setOvertime(moreThanOvertime);
}
</script>

<style>
.vis-item.vis-range.vis-selected.vis-editable {
  background-color: #ff7107 !important;
}
.vis-time-axis .vis-text {
  font-size: 11px;
}
.vis-item.vis-range.vis-editable {
  position: absolute;
  background-color: #ea9759;
  border: none;
  border-radius: 0;
}
div.vis-tooltip {
  background: #616161 !important;
  border-radius: 2px !important;
  color: #fff !important;
  font-size: 12px !important;
  display: inline-block !important;
  padding: 5px 8px !important;
}
.unavailable {
  background-color: darkgray;
}
.span-color {
  color: #fff;
}
</style>
