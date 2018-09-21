<template>
  <div>
    <v-layout row justify-center>
      <v-dialog v-model="loading"  persistent content-class="loading-dialog">
        <v-container fill-height>
          <v-layout row justify-center align-center>
            <v-progress-linear :indeterminate="true"></v-progress-linear>
          </v-layout>
        </v-container>
      </v-dialog>
    </v-layout>
  </div>
</template>

<script>
export default {
  props: {
    loading: {
      type: Boolean,
      required: false
    }
  },

  data() {
    return {
    };
  }
}
</script>

<style>
.loading-dialog{
  background-color: #232222;
}
</style>
