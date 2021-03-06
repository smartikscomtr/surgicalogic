<template>
  <div>
    <v-dialog v-model="showModal"
              persistent>
      <v-card class="container fluid grid-list-md">
        <v-card-title class="headline">
           <div class="flex xs12 sm12 md12">
            {{ deleteTitle }}
           </div>
        </v-card-title>

        <v-card-text>
            <v-layout wrap>
              <div class="flex xs12 sm12 md12"
                   v-html="deleteText">
              </div>
            </v-layout>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="darken-1"
                 flat="flat"
                 @click.native="btnYesDelete">
            {{ $t('common.yes') }}
          </v-btn>

          <v-btn color="red darken-1"
                 flat="flat"
                 @click.native="btnNoDelete">
            {{ $t('common.no') }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <snackbar-component :snackbar-visible="snackbarVisible"
                        :savedMessage="showDeleteMessage">
    </snackbar-component>
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
    return {
      snackbarVisible: null,
      showDeleteMessage: null
    };
  },

  computed: {
    deleteTitle() {
      const vm = this;

      return vm.$i18n.t('common.deleteOperation');
    },

    deleteText() {
      const vm = this;

      if (vm.deleteValue.code) {
        return '<b>' + vm.deleteValue.code + '</b>' + ' ' + vm.$i18n.t('common.doYouWantToDeleteTheCode');
      } else if (vm.deleteValue.name){
        return '<b>' + vm.deleteValue.name + '</b>' + ' ' + vm.$i18n.t('common.doYouWantToDeleteTheName');
      } else if (vm.deleteValue.firstName && vm.deleteValue.lastName) {
        return '<b>' + vm.deleteValue.firstName + ' ' + vm.deleteValue.lastName + '</b>' + ' ' + vm.$i18n.t('common.doYouWantToDeleteTheName');
      } else if (vm.deleteValue.userName) {
        return '<b>' + vm.deleteValue.userName + '</b>' + ' ' + vm.$i18n.t('common.doYouWantToDeleteTheName');
      } else {
        return vm.$i18n.t('common.doYouWantToDeleteTheRecord');
      }
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

      //We are accessing delete in vuex store

      vm.$store.dispatch(vm.deleteMethode(), {
        id: vm.deleteValue.id
      }).then(response => {
        switch (response.data.info.message) {
          case 2: {
            vm.snackbarVisible = true;

            return vm.showDeleteMessage = vm.$i18n.t('common.theItemCanNotBeDeletedBecauseItIsAssociatedWithAnotherData');
          }
          case 9: {
            vm.snackbarVisible = true;

            return vm.showDeleteMessage = vm.$i18n.t('common.operatingRoomHasOperation');
          }
          default: {
            vm.snackbarVisible = true;

            return vm.showDeleteMessage = vm.$i18n.t('common.theDataWasSuccessfullyDeleted');
          }
        }
      })

      setTimeout(() => {
        vm.snackbarVisible = false;
      }, 2300)

      vm.showModal = false;
    },

    btnNoDelete() {
      const vm = this;

      vm.showModal = false;
    }
  }
};
</script>
