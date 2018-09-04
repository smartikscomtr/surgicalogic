<template>
  <div>
    <v-btn class="drawplan-wrap"
           @click.native="drawPlan()">
      {{ $t('planarrangements.drawPlan')}}
    </v-btn>

    <div id="visualization" class="vis">
    </div>

    <tomorrow-planning-component>
    </tomorrow-planning-component>
  </div>
</template>

 <script>

import Vis from 'vis/dist/vis.js';

export default {
  data() {
    const vm = this;

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
          selectable: true

          // onMove(item, callback) {
          //   console.log('hell�');
          //     //  item.content = prompt('Edit items text:', item.content);
          //     // if (item.content != null) {
          //     //   callback(item); // send back adjusted item
          //     // }
          // }
        };

      var timeline = new Vis.Timeline(container, items, groups, options);

    }, 500);
  }
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
