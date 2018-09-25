<template>
  <div>
    <v-dialog v-model="showModal">
      <v-card class="container fluid grid-list-md">
        <v-card-title class="headline">
           <div class="flex xs12 sm12 md12">
            {{ resetPasswordTitle }}
           </div>
        </v-card-title>

        <v-card-text>
            <v-layout wrap>
              <div class="flex xs12 sm12 md12">
                {{ resetPasswordText }}
              </div>
            </v-layout>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="darken-1"
                 flat="flat"
                 @click.native="btnYesResetPassword">
            {{ yesText }}
          </v-btn>

          <v-btn color="red darken-1"
                 flat="flat"
                 @click.native="btnNoResetPassword">
            {{ noText }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

export default {
  props: {
    resetPasswordValue: {
      type: Object,
      required: false
    },

    resetPasswordVisible: {
      type: Boolean,
      required: false
    },
    resetPasswordMethod:{
      type : Function,
      required:true
    }
  },

  data() {
    return {};
  },

  computed: {
    resetPasswordTitle() {
      const vm = this;

      return vm.$i18n.t('common.resetPassword');
    },

    resetPasswordText() {
      const vm = this;

      return vm.$i18n.t('common.doYouWantToResetPassword');
    },

    yesText() {
      const vm = this;

      return vm.$i18n.t('common.yes');
    },

    noText() {
      const vm = this;

      return vm.$i18n.t('common.no');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.resetPasswordVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the resetPassword component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    }
  },

  methods: {
    btnYesResetPassword() {
      const vm = this;

      //We are accessing resetPasswordEquipment in vuex store
      vm.$store.dispatch('resetPassword', {
          email: vm.resetPasswordValue.email
        }).then(() => {
          vm.snackbarVisible = true;

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300)
        })

      vm.showModal = false;
    },

    btnNoResetPassword() {
      const vm = this;

      vm.showModal = false;
    }
  }
};
</script>
