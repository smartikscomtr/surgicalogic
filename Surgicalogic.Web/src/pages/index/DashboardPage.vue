<template>
  <div>
    <v-layout row>
    <v-flex xs12 sm6 md6>
      <v-card class="operation-wrap">
        <v-container>
          <v-card-title>
              <v-icon x-large>add_comment</v-icon>
              <p class="headline">{{ 'Operasyonlar' }} </p>
          </v-card-title>

        <v-card-actions>
          <v-btn flat @click="$router.push('/operationpage')">Operasyonlara Git</v-btn>
        </v-card-actions>
        </v-container>
      </v-card>
    </v-flex>

    <v-flex xs12 sm6 md6>
      <v-card class="plan-wrap">
        <v-container>
          <v-card-title>
              <v-icon x-large>add_alarm</v-icon>
              <p class="headline">{{ 'Planlar' }} </p>
          </v-card-title>

        <v-card-actions>
          <v-btn flat @click="$router.push('/planarrangementspage')">Plan Düzenlemelerine Git</v-btn>
        </v-card-actions>
        </v-container>
      </v-card>
    </v-flex>

    <v-flex xs12 sm6 md6>
      <v-card class="klinik-wrap">
        <v-container>
          <v-card-title>
              <v-icon x-large>domain</v-icon>
              <p class="headline">{{ 'Klinik Yönetimleri' }} </p>
          </v-card-title>

        <v-card-actions>
          <v-btn flat @click="">Klinik Yönetimine Git</v-btn>
        </v-card-actions>
        </v-container>
      </v-card>
    </v-flex>
    </v-layout>
  </div>
</template>

<script>

export default {
  data() {
    return {
    }
  }
}
</script>

<style>
.layout.row {
  border: 9px solid #fff;
  padding: 1% 1%;
  margin: 1%;
}
</style>
