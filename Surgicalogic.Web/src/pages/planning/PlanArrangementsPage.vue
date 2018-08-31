<template>
  <div>
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
        // locale: 'tr-TR',
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
          console.log(item);
            //  item.content = prompt('Edit items text:', item.content);
            // if (item.content != null) {
            //   callback(item); // send back adjusted item
            // }
        }
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
.vis-item {
    border-color: teal;
    background-color: #2eb1b1;
}
.vis-item.vis-range .vis-item-content {
  color: #fff;
}
.vis-item .vis-item-overflow {
  background-color: #ea9759;
}
.vis-time-axis .vis-text {
  font-size: 11px;
}
.vis-timeline {
  border: 1px solid #60beb5;
}
</style>
