<template>
  <div>
    <v-btn class="drawplan-wrap"
           @click.native="drawPlan()">
      {{ $t('planarrangements.drawPlan')}}
    </v-btn>

    <div id="visualization" class="vis">
    </div>

    <div id="overtime">

    </div>

    <div id="utilization">

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

        setOvertime(vm.$store.state.planArrangementsModule.model.overtime);

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
          //   var oldItem = null;

          //   items.forEach(element => {
          //       if (element.id == item.id)
          //       {
          //         oldItem = element;
          //       }
          //   });

          //   var overtime = 10;
          //   var moreThanOvertime = parseInt(document.getElementById("overtime").innerHTML);

          //   var newStart = new Date(item.start);
          //   var newEnd = new Date(item.end);
          //   var oldStart = new Date(oldItem.start);
          //   var oldEnd = new Date(oldItem.end);

          //   if(oldStart !=newStart)
          //   {
          //       if (newStart.getHours() >= overtime)
          //       {
          //           setOvertime(parseInt(moreThanOvertime + newStart.getMinutes()));
          //       }
          //   }

          //   if(oldEnd != newEnd)
          //   {
          //       if (newEnd.getHours() >= overtime)
          //       {
          //           setOvertime(parseInt(moreThanOvertime + newEnd.getMinutes()));
          //       }
          //   }
          selectable: true,

          //   debugger;
          //   setUtilization("very good")
          //  // setOvertime("very good")
          }
        };

      var timeline = new Vis.Timeline(container, items, groups, options);

    }, 500);
  }
}

function setUtilization(value)
{
   document.getElementById("utilization").innerHTML = value;
}

function setOvertime(value)
{
   document.getElementById("overtime").innerHTML = value;
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
</style>
