<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
              {{ formTitle }}
            </span>

            <v-icon @click="cancel"
                    class="close-wrap">
              close
            </v-icon>
          </div>
        </v-card-title>

        <v-card-text>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operation.operationName') }}
                  </div>

                  <div class="value">
                    {{ detailAction['name'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operation.branch') }}
                  </div>

                  <div class="value">
                    {{ detailAction['branchName'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operation.operationType') }}
                  </div>

                  <div class="value">
                    {{ detailAction['operationTypeName'] }}
                  </div>
                </div>
              </v-flex>

               <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('personnel.personnel') }}
                  </div>

                  <div class="value">
                    {{ detailAction['doctorNames'] }}
                  </div>
                </div>
              </v-flex>

               <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operatingrooms.blockedOperatingRooms') }}
                  </div>

                  <div class="value">
                    {{ detailAction['operatingRoomNames'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operation.operationDate') }}
                  </div>

                  <div class="value">
                    {{ detailAction['date'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operation.operationTime') }}
                  </div>

                  <div class="value">
                    {{ detailAction['operationTime'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('common.description') }}
                  </div>

                  <div class="value">
                    {{ detailAction['description'] }}
                  </div>
                </div>
              </v-flex>
            </v-layout>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

export default {
  props: {
    detailVisible: {
      type: Boolean,
      required: false
    },

    detailAction: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    }
  },

  data() {
    return {};
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.$i18n.t('operation.detailOperation');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.detailVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the equipments detail component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    }
  },

  methods: {
    cancel() {
      const vm = this;

      vm.showModal = false;
    }
  }
};

</script>
