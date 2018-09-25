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
                  {{ $t('operationtypes.operationType') }}
                </div>

                <div class="value">
                  {{ detailAction['name'] }}
                </div>
              </div>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <div class="input-group readonly-wrap">
                <div class="label">
                  {{ $t('branches.branch') }}
                </div>

                <div class="value">
                  {{ detailAction['branchName'] }}
                </div>
              </div>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <div class="input-group readonly-wrap">
                <div class="label">
                  {{ $t('equipments.equipment') }}
                </div>

                <div class="value">
                  {{ detailAction['equipmentName'] }}
                </div>
              </div>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <div class="input-group readonly-wrap">
                <div class="label">
                  {{ $t('operatingrooms.operatingRoom') }}
                </div>

                <div class="value">
                  {{ detailAction['operatingRoomName'] }}
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

      return vm.$i18n.t('operationtypes.detailOperationType');
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
