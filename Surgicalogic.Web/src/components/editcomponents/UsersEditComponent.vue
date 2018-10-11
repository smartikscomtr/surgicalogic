<template>
  <div>
    <v-dialog v-model="showModal" slot="activator" persistent>
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
              {{ formTitle }}
            </span>

            <v-icon @click="cancel" class="close-wrap">
              close
            </v-icon>
          </div>
        </v-card-title>

        <v-card-text>
          <v-layout wrap edit-layout>
            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['userName']" :label="$t('users.userName')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6 input-group-checkbox>
              <v-checkbox v-model="editAction['isAdmin']" :label="$t('users.isAdmin')" color="primary">
              </v-checkbox>
            </v-flex>

            <v-flex xs12 sm12 md12>
              <v-text-field v-model="editAction['email']" :label="$t('users.email')">
              </v-text-field>
            </v-flex>

            <!-- <v-flex xs6 sm6 md6 text-lg-left text-md-left text-sm-left text-xs-left>
                <v-btn class="btnSave blue"
                      @click="resetPassword(editAction['email'])">
                   Şifreyi Sıfırla
                  </v-btn>
              </v-flex> -->

          </v-layout>
        </v-card-text>

        <v-card-text>
          <div class="margin-bottom-none btn-wrap">
            <v-btn class="btnSave orange" @click.native="save">
              Kaydet
            </v-btn>
          </div>
        </v-card-text>
      </v-card>
    </v-dialog>

    <snackbar-component :snackbar-visible="snackbarVisible" :savedMessage="savedMessage">
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
            savedMessage: this.$i18n.t('users.userSaved')
        };
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.editIndex === -1
                ? vm.$i18n.t('users.addUsersInformation')
                : vm.$i18n.t('users.editUsersInformation');
        },

        showModal: {
            get() {
                const vm = this;

                return vm.editVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the users edit component
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

            //Edit work type
            if (vm.editIndex > -1) {
                //We are accessing updateWorkType in vuex store
                vm.$store
                    .dispatch('updateUser', {
                        id: vm.editAction.id,
                        userName: vm.editAction.userName,
                        email: vm.editAction.email,
                        isAdmin: vm.editAction.isAdmin
                    })
                    .then(() => {
                        vm.snackbarVisible = true;
                        vm.$store.dispatch('getUsers');

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }
            //Add work type
            else {
                //We are accessing insertWorkType in vuex store
                vm.$store
                    .dispatch('insertUser', {
                        userName: vm.editAction.userName,
                        email: vm.editAction.email,
                        isAdmin: vm.editAction.isAdmin
                    })
                    .then(() => {
                        vm.snackbarVisible = true;
                        vm.$store.dispatch('getUsers');

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }

            vm.showModal = false;
        },

        resetPassword(item) {
            const vm = this;

            //When the reset password button is clicked, the event is sent to the grid component
            vm.$store
                .dispatch('resetPassword', {
                    email: item
                })
                .then(() => {
                    vm.snackbarVisible = true;

                    setTimeout(() => {
                        vm.snackbarVisible = false;
                    }, 2300);
                });
        }
    }
};
</script>
