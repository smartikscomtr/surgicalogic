<template>
  <div>
    <loading-component :loading="getLoading">
    </loading-component>

    <v-btn class="drawplan-wrap"
           @click.native="drawPlan()">
      {{ $t('planarrangements.drawPlan')}}
    </v-btn>
        <v-btn class="updateplan-wrap"
           @click.native="updatePlan()">
      {{ $t('planarrangements.updatePlan')}}
    </v-btn>

    <div id="visualization" class="vis">
    </div>

    <div id="serializedTimeline" style="display:none;">

    </div>

    <div class="overtimeInfo">
     {{ $t('planarrangements.overtime')}}: <span id="overtime"></span> {{ $t('planarrangements.minute')}}
    </div>

    <div class="utilizationInfo">
      {{ $t('planarrangements.utilization')}}: %<span id="utilization"></span>
    </div>

    <tomorrow-planning-component>
    </tomorrow-planning-component>
  </div>
</template>

 <script>

import Vis from 'vis/dist/vis.js';

export default {
  data() {
    return {};
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

      vm.$store.dispatch('getGenerateOperationPlan').then(response => {
        vm.getOperationPlan();
        vm.$store.dispatch('getTomorrowOperationList');
      });
    },

    updatePlan() {
      const vm = this;

       var timelineItems = JSON.parse(document.getElementById("serializedTimeline").innerHTML);

       var operations = [];

      for (var data in timelineItems._data)
      {
          var item = timelineItems._data[data];
          //ilk ve son başlangıç ve bitiş tarihleri
          var newStart = new Date(item.start);
          var newEnd = new Date(item.end);

          var operationLength = (newEnd.getTime() - newStart.getTime()) / 60000;

          var operation = {
             id: item.operationPlanId,
             operationId: data,
             start:newStart,
             roomId: item.group,
             length:operationLength
             };

          operations.push(operation);
      }

      vm.$store.dispatch('updatePlanArrangements', JSON.stringify(operations)).then(response => {
        vm.$store.dispatch('getTomorrowOperationList');
      });;
    },

    getOperationPlan() {
      const vm = this;

      var container = document.getElementById('visualization');
      container.innerHTML = "";

      vm.$store.dispatch('getPlanArrangements').then(response => {
      var items = new Vis.DataSet(vm.$store.state.planArrangementsModule.model.plan);
      var groups = new Vis.DataSet(vm.$store.state.planArrangementsModule.model.rooms);

      var workingHourStart = new Date(vm.$store.state.planArrangementsModule.model.workingHourStart);
      var workingHourEnd = new Date(vm.$store.state.planArrangementsModule.model.workingHourEnd);

      document.getElementById("serializedTimeline").innerHTML = JSON.stringify(items);

      calcuteOvertimeAndUtilization(vm.$store.state.planArrangementsModule.model.startDate, groups.length, workingHourStart, workingHourEnd);

      var options = {
        orientation: {
          axis: 'top'
        },
        // timeAxis: { scale: 'minute', step: vm.$store.state.planArrangementsModule.model.period },
        locale: 'tr',
        moveable: true,
        zoomMax: 86400000,
        zoomMin: 3600000,
        horizontalScroll: true,
        min: vm.$store.state.planArrangementsModule.model.minDate,
        max:vm.$store.state.planArrangementsModule.model.maxDate,
        start: vm.$store.state.planArrangementsModule.model.startDate,
        end: vm.$store.state.planArrangementsModule.model.endDate,
        editable: {
          updateTime: true,
          updateGroup: true
        },
        selectable:true,

        onMoving(item, callback) {
          var timelineItems = JSON.parse(document.getElementById("serializedTimeline").innerHTML);

          timelineItems._data[item.id].start = new Date(item.start);
          timelineItems._data[item.id].end = new Date(item.end);
          timelineItems._data[item.id].group = item.group;

          document.getElementById("serializedTimeline").innerHTML = JSON.stringify(timelineItems);

          calcuteOvertimeAndUtilization(options.start, groups.length, workingHourStart, workingHourEnd);

          callback(item);
        }
      };

      var timeline = new Vis.Timeline(container, items, groups, options);

      });
    }
  },

  mounted () {
    const vm = this;

    vm.getOperationPlan();
  }
}

function setUtilization(value)
{
  var result = parseInt(value);

  if (isNaN(result))
  {
    result = 0;
  }

   document.getElementById("utilization").innerHTML = result;
}

function setOvertime(value)
{
   document.getElementById("overtime").innerHTML = value;
}

function calcuteOvertimeAndUtilization(start, roomsCount, workingHourStart, workingHourEnd)
{
      var timelineItems = JSON.parse(document.getElementById("serializedTimeline").innerHTML);

      var moreThanOvertime = 0;
      var totalOperationLengths = 0;

      var startDate = new Date(start);
      //mesai başlangıcı
      var firstOvertime = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate(), workingHourStart.getHours(),workingHourStart.getMinutes(),0);
      //mesai bitişi
      var lastOvertime = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate(), workingHourEnd.getHours(),workingHourEnd.getMinutes(),0);

      for (var data in timelineItems._data)
      {
          var operation = timelineItems._data[data];
          //ilk ve son başlangıç ve bitiş tarihleri
          var newStart = new Date(operation.start);
          var newEnd = new Date(operation.end);

          var operationLength = (newEnd.getTime() - newStart.getTime()) / 60000;

          totalOperationLengths += operationLength;

          var newFirstOverTime = newStart < firstOvertime ? ((firstOvertime - newStart.getTime()) / 60000) > operationLength ? operationLength : ((firstOvertime - newStart.getTime()) / 60000): 0;
          var newLastOverTime = newEnd > lastOvertime ? ((newEnd.getTime() - lastOvertime) / 60000) > operationLength ? operationLength : ((newEnd.getTime() - lastOvertime) / 60000): 0;

          moreThanOvertime += (newFirstOverTime + newLastOverTime);
      }

      var totalAvailableDuration = roomsCount * ((lastOvertime - firstOvertime) / 60000);

      setUtilization((totalOperationLengths / totalAvailableDuration) * 100);
      setOvertime(moreThanOvertime);
}
 </script>

<style>
.vis {
  padding: 25px;
}
.vis-item.vis-range .vis-item-content {
  color: #fff;
}
.vis-item.vis-range.vis-selected.vis-editable {
  background-color: #ff7107 !important
}
.vis-time-axis .vis-text {
  font-size: 11px;
}
.drawplan-wrap {
  left: 25px;
  top: 11px;
  padding: 0;
  margin: 0;
  min-width: 140px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
}
.drawplan-wrap .v-btn__content {
  color: #fff;
}
.updateplan-wrap {
  left: 25px;
  top: 11px;
  padding: 0;
  float:right;
  margin: 0 5% 0 0;
  min-width: 200px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
}
.updateplan-wrap .v-btn__content {
  color: #fff;
}
.vis-item.vis-range.vis-editable {
  position: absolute;
  background-color: #ea9759;
}
div.vis-tooltip{
  background: #616161 !important;
  border-radius: 2px !important;
  color: #fff !important;
  font-size: 12px !important;
  display: inline-block !important;
  padding: 5px 8px !important;
}
.overtimeInfo {
margin-left: 2%
}
.utilizationInfo {
margin-left: 2%
}
.unavailable {
  background-color:darkgray;
}
</style>
