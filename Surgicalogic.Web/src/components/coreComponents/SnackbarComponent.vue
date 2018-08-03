<template>
  <div>
    <v-snackbar v-model="a"
                :timeout="timeout"
                :top="true">
      {{ savedMessage }}

      <v-btn color="pink"
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

  methods: {
    setSnackbarVisible() {
      const vm = this;
debugger
      vm.a = vm.snackbarVisible;
    }

  },

  data() {
    return {
      timeout: 2000,
      a: false
    };
  }
});

</script>
