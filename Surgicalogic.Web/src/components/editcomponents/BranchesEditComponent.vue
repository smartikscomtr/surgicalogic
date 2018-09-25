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
        <v-layout wrap edit-layout>
          <v-flex xs12 sm12 md12>
            <v-text-field v-model="editAction['name']"
                          :label="$t('branches.branchName')">
            </v-text-field>
          </v-flex>

          <v-flex xs12 sm12 md12>
            <v-textarea v-model="editAction['description']"
                        rows="3"
                        :label="$t('common.description')">
            </v-textarea>
          </v-flex>
        </v-layout>
      </v-card-text>

      <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right margin-bottom-none
              class="btn-wrap">
        <v-btn class="btnSave orange"
              @click.native="save">
          Kaydet
        </v-btn>
      </v-flex>
    </v-card>
  </v-dialog>

  <snackbar-component :snackbar-visible="snackbarVisible"
                      :savedMessage="savedMessage">
  </snackbar-component>
  </div>
</template>

<script>

export default {
  props: {
    editVisible: {
      type: Boolean,
      required: false
    },

    editAction: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    },

    editIndex: {
      type: Number,
      required: false
    }
  },

  data() {
    return {
      snackbarVisible: null,
      savedMessage: this.$i18n.t('branches.branchSaved')
    };
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1 ? vm.$i18n.t('branches.addBranchesInformation') : vm.$i18n.t('branches.editBranchesInformation');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the branches edit component
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
    },

    save() {
      const vm = this;

      vm.snackbarVisible = false;

      //Edit branch
      if (vm.editIndex > -1) {
        //We are accessing updateBranch in vuex store
        vm.$store.dispatch('updateBranch', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          description: vm.editAction.description
        }).then(() => {
          vm.snackbarVisible = true;

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300)
        })
      }

      //Add branch
      else {
        //We are accessing insertBranch in vuex store
        vm.$store.dispatch('insertBranch', {
          name: vm.editAction.name,
          description: vm.editAction.description
        }).then(() => {
          vm.snackbarVisible = true;

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300)
        })
      }

      vm.showModal = false;
    }
  }
};

</script>

