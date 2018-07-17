<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <a class="backBtn"
               flat
               @click="cancel">
              <v-icon>
                arrow_back
              </v-icon>
            </a>

            <span class="text">
              {{ formTitle }}
            </span>
          </div>
        </v-card-title>

        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <div class="input-group readonly-wrap">
                  <div class="label">
                    {{ $t('equipments.equipments') }}
                  </div>

                  <div class="value">
                    {{ detailAction['operatingRoomEquipments.name'] }}
                  </div>
                </div>
              </v-flex>
            </v-layout>
          </v-container>
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
