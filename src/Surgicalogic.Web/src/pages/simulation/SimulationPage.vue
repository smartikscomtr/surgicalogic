<template>
  <div class="container fluid grid-list-md">
    <simulation-run-component>
    </simulation-run-component>
  </div>
</template>



<script>

import { gridMixin } from './../../mixins/gridMixin';

export default {
  mixins: [
    gridMixin
  ],

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('simulation.simulation');
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.simulationModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.simulationModule.totalCount;
    }
  },

  methods: {
    // runSimulation() {
    //   const vm = this;

    //   vm.$store.dispatch('runSimulation');
    // }
  }
}

</script>
