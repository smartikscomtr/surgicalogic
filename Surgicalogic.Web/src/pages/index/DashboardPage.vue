<template>
  <div>
    <v-layout row wrap>
      <v-flex xs12 sm6 md6>
        <v-card class="operation-wrap">
          <v-container>
            <v-card-title>
              <div>
                <v-icon dark x-large>add_comment</v-icon>
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
                <v-icon dark x-large>add_alarm</v-icon>
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
              <v-btn small @click="">{{ $t('common.go') }}</v-btn>
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
              <v-btn small @click="">{{ $t('common.go') }}</v-btn>
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
    border: 9px solid #fff;
    padding: 1%;
    margin: 1%;
}
.layout.row.wrap > div {
    padding: 1%;
}
.operation-wrap.v-card {
    background-color: #f67171;
}
.plan-wrap.v-card {
    background-color: #f6bb7f;
}
.clinic-wrap.v-card {
    background-color: #36a157b5;
}
.personnel-wrap {
  background-color: #8754b5d1 !important;
}
.v-card__actions {
    justify-content: flex-end;
}
.v-card__title > div {
    display: flex;
    align-items: center;
    justify-content: center;
    margin-bottom: 20px;
}
.v-card__title > div p {
    margin-left: 10px;
}
.v-card__title p {
    margin: 0;
    color:#fff;
}
.v-card__title {
    flex-direction: column;
    align-items: flex-start;
}

</style>
