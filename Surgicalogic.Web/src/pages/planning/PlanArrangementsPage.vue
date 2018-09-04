<template>
  <div>
    <v-btn class="drawplan-wrap"
           @click.native="drawPlan()">
      {{ $t('planarrangements.drawPlan')}}
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

  methods: {
    drawPlan() {
      const vm = this;

      vm.$store.dispatch('getGenerateOperationPlan');
    }
  },

  mounted () {
    const vm = this;

      var container = document.getElementById('visualization');

      vm.$store.dispatch('getPlanArrangements');

      setTimeout(() => {
        var items = new Vis.DataSet(vm.$store.state.planArrangementsModule.model.plan);
        var groups = new Vis.DataSet(vm.$store.state.planArrangementsModule.model.rooms);

        document.getElementById("serializedTimeline").innerHTML = JSON.stringify(items);

        calcuteOvertimeAndUtilization(vm.$store.state.planArrangementsModule.model.startDate, groups.length);

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

          onMove(item, callback) {
            var timelineItems = JSON.parse(document.getElementById("serializedTimeline").innerHTML);

            timelineItems._data[item.id].start = new Date(item.start);
            timelineItems._data[item.id].end = new Date(item.end);

            document.getElementById("serializedTimeline").innerHTML = JSON.stringify(timelineItems);

            calcuteOvertimeAndUtilization(options.start, groups.length);
          }
        };

      var timeline = new Vis.Timeline(container, items, groups, options);

    }, 500);
  }
}

function setUtilization(value)
{
   document.getElementById("utilization").innerHTML = parseInt(value);
}

function setOvertime(value)
{
   document.getElementById("overtime").innerHTML = value;
}

function calcuteOvertimeAndUtilization(start, roomsCount)
{
      var timelineItems = JSON.parse(document.getElementById("serializedTimeline").innerHTML);

      var moreThanOvertime = 0;
      var totalOperationLengths = 0;

      var startDate = new Date(start);
      //mesai başlangıcı
      var firstOvertime = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate(), 9,0,0);
      //mesai bitişi
      var lastOvertime = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate(), 10,0,0);

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
</style>
