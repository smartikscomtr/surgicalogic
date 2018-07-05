<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
      <v-card>
        <v-card-title>
          <span class="headline-wrap">
            <a class="backBtn"
                flat
                @click="cancel">
              <v-icon>arrow_back</v-icon>
            </a>

            <span class="text">
              {{ formTitle }}
            </span>
          </span>
        </v-card-title>

        <v-card-text >
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <div class="label">{{ $t('equipments.equipment') }}</div>
                <div class="value">{{ detailAction['name'] }}</div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="label">{{ $t('equipments.type') }}</div>
                <div class="value">{{ detailAction['equipmentTypes'] }}</div>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <div class="label">{{ $t('equipments.portable') }}</div>
                <div class="value">{{ detailAction['isPortable'] }}</div>
              </v-flex>
              <v-flex xs12 sm6 md6>
                <div class="label">{{ $t('common.description') }}</div>
                <div class="value">{{ detailAction['description'] }}</div>
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

      return vm.$i18n.t("equipments.detailEquipments");
    },

    showModal: {
      get() {
        const vm = this;

        return vm.detailVisible;
      },
      set (value) {
        const vm = this;

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
}

</script>
