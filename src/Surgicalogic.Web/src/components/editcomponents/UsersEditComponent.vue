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

        <v-form ref="form" v-model="valid" lazy-validation>
        <v-card-text>
          <v-layout wrap edit-layout>
            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['userName']"  :rules="[rules.required]" :label="$t('users.userName')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6 input-group-checkbox>
              <v-checkbox v-model="editAction['isAdmin']" :label="$t('users.isAdmin')" color="primary">
              </v-checkbox>
            </v-flex>

            <v-flex xs12 sm12 md12>
              <v-text-field v-model="editAction['email']"  :rules="[rules.required, rules.email]" :label="$t('users.email')">
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

        <v-card-actions class="justify-end flex">
              <v-btn class="orangeButton" @click.native="save">
              {{ $t('menu.save') }}
              </v-btn>
          </v-card-actions>
        </v-form>
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
            savedMessage: this.$i18n.t('users.userSaved'),
            valid: true,
            rules: {
              required: value => !!value || this.$i18n.t('common.required'),
              multipleRequired: v =>!!v && v.length > 0 || this.$i18n.t('common.required'),
              email: value => {
                const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                return pattern.test(value) || this.$i18n.t('common.invalidEmail')
              }
            }
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

            vm.clear();
            vm.showModal = false;
          },

          clear () {
              this.$refs.form.reset()
          },

          save() {
            const vm = this;

            if (!vm.$refs.form.validate()) {
              return;
            }

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
                    .then(response => {
                        vm.processResult(response);
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
                    .then(response => {
                        vm.processResult(response);
                    });
            }
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
        },

         processResult(response) {
          const vm = this;

            if (response.data.info.succeeded){
                vm.savedMessage= vm.$i18n.t('users.userSaved');
                vm.snackbarVisible = true;
            }
            else if (response.data.info.message == errorMessages.CodeIsNotUnique){
              vm.savedMessage= vm.$i18n.t('common.emailIsNotUnique');
              vm.snackbarVisible = true;
              setTimeout(() => {
                vm.snackbarVisible = false;
              }, 2300);

              return false;
            }

            if (response.data.info.infoType == infoTypes.userSelfUpdated)
            {
              localStorage.setItem("username", response.data.result.userName);
              vm.$router.go();
            }

            vm.snackbarVisible = true;
            vm.$parent.getUsers();

            setTimeout(() => {
                vm.snackbarVisible = false;
            }, 2300);

          vm.showModal = false;
          vm.clear();
        },
    }
};
</script>
