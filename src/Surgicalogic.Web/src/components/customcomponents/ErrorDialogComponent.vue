<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              persistent>
      <v-card style="text-align:center;" class="container fluid grid-list-md">
          <h2>{{ $t('common.apiErrorMessage') }} </h2>
      </v-card>
    </v-dialog>

  </div>
</template>

<script>

export default {
  data() {
    return {
      showModal: true
      }
  }
}
</script>

