<template>
  <v-content>
    <v-container fluid fill-height>
      <v-layout align-center justify-center>
        <h1>{{ errorMessage }}</h1>
      </v-layout>
    </v-container>
  </v-content>
</template>

<script>

export default {
  data() {
  return {
    errorMessage: this.$i18n.t('common.apiErrorMessage')
    };
  }
}

</script>
