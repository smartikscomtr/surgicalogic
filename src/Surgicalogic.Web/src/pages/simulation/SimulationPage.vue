<template>
  <div class="container fluid grid-list-md">
      <div class="grid-card v-card">
        <div class="v-card__title">
          <h2>
            {{ title }}
          </h2>
        </div>

        <v-btn  class="orange"  @click="runSimulation">          
          {{ $t('simulation.simulate') }}
        </v-btn>
        <br/>
        <br/>
      

      </div>
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
      headers() {
        const vm = this;        
      }
  },

  methods: {
    runSimulation() {
      const vm = this;

       vm.$store.dispatch('runSimulation');

    }

  }


}

</script>