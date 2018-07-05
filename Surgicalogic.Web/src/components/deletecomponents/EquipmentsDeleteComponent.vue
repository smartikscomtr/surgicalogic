<template>
  <div>
    <v-dialog v-model="showModal">
      <v-card>
        <v-card-title class="headline">
          {{ deleteTitle }}
        </v-card-title>

        <v-card-text>
          {{ deleteText }}
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="green darken-1"
                 flat="flat"
                 @click.native="btnYesDelete">
            Evet
          </v-btn>

          <v-btn color="green darken-1"
                 flat="flat"
                 @click.native="btnNoDelete">
            Hayır
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

export default {
  props: {
    deleteValue: {
      type: Object,
      required: false
    },

    deleteVisible: {
      type: Boolean,
      required: false
    }
  },

  data() {
    return {};
  },

  computed: {
    deleteTitle() {
      const vm = this;

      return vm.$i18n.t('common.deleteOperation');
    },

    deleteText() {
      const vm = this;

      return vm.$i18n.t('common.doYouWantToDeleteTheRecord');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.deleteVisible;
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
    btnYesDelete() {
      const vm = this;

      vm.$store.dispatch('deleteEquipment', {
        id: vm.deleteValue.id
      });

      vm.showModal = false;
    },

    btnNoDelete() {
      const vm = this;

      vm.showModal = false;
    }
  }
}

</script>
