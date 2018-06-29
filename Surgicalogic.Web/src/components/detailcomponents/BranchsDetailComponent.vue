<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">
            <v-btn class="backBtn"
                  flat
                  @click.native="cancel">
              <v-icon>arrow_back</v-icon>
            </v-btn>

            <div class="text">
              {{ formTitle }}
            </div>
          </span>
        </v-card-title>

        <v-card-text >
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <div class="label">{{ $t('branchs.branch') }}</div>
                <div class="value">{{ detailAction['name'] }}</div>
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

      return vm.$i18n.t("branchs.detailBranchs");
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

<style>
</style>
