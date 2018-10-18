<template>
  <div>
    <v-snackbar v-model="snackbar"
                :timeout="timeout"
                :top="true">
      {{ savedMessage }}

      <v-btn class="close-wrap"
             flat
             @click="snackbar = false">
        {{ $t('common.close') }}
      </v-btn>
    </v-snackbar>
  </div>
</template>

<script>

import Vue from 'vue';

export default Vue.extend({
  props: {
    snackbarVisible: {
      type: Boolean,
      required: false
    },

    savedMessage: {
      type: String,
      required: false
    }
  },

  data() {
    return {
      timeout: 2000,
      snackbar: null
    };
  },

  mounted () {
    const vm = this;

    vm.$watch('snackbarVisible', (newValue, oldValue) => {
      if (newValue !== oldValue) {

        vm.snackbar = vm.snackbarVisible
        // vm.$emit('snackbarVisible', newValue);
      }
    });
  }
});

</script>
