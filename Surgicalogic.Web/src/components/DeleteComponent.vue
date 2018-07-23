<template>
  <div>
    <v-dialog v-model="showModal">
      <v-card class="container fluid grid-list-md">
        <v-card-title class="headline">
           <div class="flex xs12 sm12 md12">
            {{ deleteTitle }}
           </div>
        </v-card-title>

        <v-card-text>
            <v-layout wrap>
              <div class="flex xs12 sm12 md12">
                {{ deleteText }}
              </div>
            </v-layout>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="darken-1"
                 flat="flat"
                 @click.native="btnYesDelete">
            Evet
          </v-btn>

          <v-btn color="red darken-1"
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
    },
    deleteMethode:{
      type : Function,
      required:true
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

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the delete component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    }
  },

  methods: {
    btnYesDelete() {
      const vm = this;

      //We are accessing deleteEquipment in vuex store

      vm.$store.dispatch(vm.deleteMethode(), {
        id: vm.deleteValue.id
      });

      vm.showModal = false;
    },

    btnNoDelete() {
      const vm = this;

      vm.showModal = false;
    }
  }
};
</script>
