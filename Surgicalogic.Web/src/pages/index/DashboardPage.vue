<template>
  <div>
    <v-layout row wrap class="cards-wrap">
      <v-flex xs12 sm6 md6>
        <v-card class="operation-wrap">
          <v-container>
            <v-card-title>
              <div>
                <v-icon dark x-large>add_alarm</v-icon>
                <p class="headline">{{ 'Operasyonlar' }} </p>
              </div>
              <p>{{ 'Operasyonlarınızı Yönetebilirsiniz' }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn small @click="$router.push('/operationpage')">Git</v-btn>
            </v-card-actions>
          </v-container>
        </v-card>
      </v-flex>

      <v-flex xs12 sm6 md6>
        <v-card class="plan-wrap">
          <v-container>
            <v-card-title>
              <div>
                <v-icon dark x-large>timeline</v-icon>
                <p class="headline">{{ 'Planlar' }} </p>
              </div>
              <p>{{ 'Planlarınızı Yönetebilirsiniz' }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn small @click="$router.push('/planarrangementspage')">Git</v-btn>
            </v-card-actions>
          </v-container>
        </v-card>
      </v-flex>

      <v-flex xs12 sm6 md6>
        <v-card class="clinic-wrap">
          <v-container>
            <v-card-title>
              <div>
                <v-icon dark x-large>domain</v-icon>
                <p class="headline">{{ 'Klinik Yönetimleri' }} </p>
              </div>
              <p>{{ 'Kliniklerinizi Yönetebilirsiniz' }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn small @click="$router.push('/clinicpage')">{{ $t('common.go') }}</v-btn>
            </v-card-actions>
          </v-container>
        </v-card>
      </v-flex>

      <v-flex xs12 sm6 md6>
        <v-card class="personnel-wrap">
          <v-container>
            <v-card-title>
              <div>
                <v-icon dark x-large>group</v-icon>
                <p class="headline">{{ 'Personel Yönetimleri' }} </p>
              </div>
              <p>{{ 'Personellerinizi Yönetebilirsiniz' }} </p>
            </v-card-title>

            <v-card-actions>
              <v-btn small @click="$router.push('/personnelpage')">{{ $t('common.go') }}</v-btn>
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
        return {};
    }
};
</script>

<style>
.layout.row {
    padding: 1%;
    margin: 1%;
}
.layout.row.wrap > div {
    padding: 1%;
}
.cards-wrap .v-btn__content{
    color: rgba(0,0,0,.87) !important;
}
.operation-wrap.v-card {
    background-color: #ff6767;
}
.plan-wrap.v-card {
    background-color: #ffba75;
}
.clinic-wrap.v-card {
    background-color: #56d67d;
}
.personnel-wrap {
  background-color: #9b63cd !important;
}
.v-card__actions {
    justify-content: flex-end;
}
.cards-wrap .v-card__title > div {
    display: flex;
    align-items: center;
    justify-content: center;
    margin-bottom: 20px;
}
.cards-wrap .v-card__title > div p {
    margin-left: 10px;
}
.cards-wrap .v-card__title p {
    margin: 0;
    color:#fff;
}
.cards-wrap .v-card__title {
    flex-direction: column !important;
    align-items: flex-start !important;
}

</style>
