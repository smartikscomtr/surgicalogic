<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              persistent>
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
                    {{ $t('operatingrooms.operatingRoom') }}
                  </div>

                  <div class="value">
                    {{ detailAction['name'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operatingrooms.isAvailable') }}
                  </div>

                  <div class="value">
                    {{ getAvailability() }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm3 md3>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operatingrooms.location') }}
                  </div>

                  <div class="value">
                    {{ detailAction['location'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm3 md3>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operatingrooms.width') }}
                  </div>

                  <div class="value">
                    {{ detailAction['width'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm3 md3>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operatingrooms.height') }}
                  </div>

                  <div class="value">
                    {{ detailAction['height'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm3 md3>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operatingrooms.length') }}
                  </div>

                  <div class="value">
                    {{ detailAction['length'] }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('operationtypes.operationType') }}
                  </div>

                  <div class="value">
                    {{ selectOperationType }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('equipments.equipments') }}
                  </div>

                  <div class="value">
                    {{ selectEquipment }}
                  </div>
                </div>
              </v-flex>

              <v-flex xs12 sm12 md12>
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

      return vm.$i18n.t('operatingrooms.detailOperatingRooms');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.detailVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the operating rooms detail component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    selectEquipment() {
      const vm = this;

      let selectedEquipment = [];

      if (vm.detailAction.operatingRoomEquipments)
      {
        vm.detailAction.operatingRoomEquipments.forEach(item => {
          selectedEquipment.push(item.equipment.name);
        });
      }

      return selectedEquipment.join();
    },

    selectOperationType() {
      const vm = this;

      let selectedOperationTypes = [];

      if (vm.detailAction.operatingRoomOperationTypes)
      {
        vm.detailAction.operatingRoomOperationTypes.forEach(item => {
          selectedOperationTypes.push(item.operationType.name);
        });
      }

      return selectedOperationTypes.join();
    }

  },

  methods: {
    cancel() {
      const vm = this;

      vm.showModal = false;
    },

    getAvailability() {
      const vm = this;

      if(vm.detailAction.isAvailable)
      {
          return vm.$i18n.t("common.yes");
      }
      else
      {
           return vm.$i18n.t("common.no");
      }
    }
  }
};

</script>
