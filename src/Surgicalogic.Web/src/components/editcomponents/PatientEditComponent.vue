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
              <v-text-field v-model="editAction['identityNumber']"
                            :rules="identityRequired"
                            mask="###########"
                            :label="$t('patient.identityNumber')">
              </v-text-field>
            </v-flex>

             <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['firstName']"
                            :rules="required"
                            :label="$t('patient.firstName')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['lastName']"
                            :rules="required"
                            :label="$t('patient.lastName')">
              </v-text-field>
            </v-flex>

             <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['phone']"
                            :rules="required"
                            :label="$t('patient.phone')">
              </v-text-field>
            </v-flex>

             <v-flex xs12 sm12 md12>
              <v-text-field v-model="editAction['address']"
                            :label="$t('patient.address')">
              </v-text-field>
            </v-flex>

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
            savedMessage: this.$i18n.t('patient.patientSaved'),
            valid: true,
            required: [
              v => !!v || this.$i18n.t('common.required')
            ],
            identityRequired: [
              v => !!v && v.length == 11 || this.$i18n.t('common.identityRequired')
            ]
        };
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.editIndex === -1
                ? vm.$t('patient.addPatientInformation')
                : vm.$t('patient.editPatientInformation');
        },

        showModal: {
            get() {
                const vm = this;

                return vm.editVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the patient edit component
                if (!value) {
                    vm.$emit('cancel');
                }
            }
        }
    },

    methods: {
        customFilter(item, queryText, itemText) {
            const vm = this;

            const text = vm.replaceForAutoComplete(item.name);
            const searchText = vm.replaceForAutoComplete(queryText);

            return text.indexOf(searchText) > -1;
        },

        replaceForAutoComplete(text) {
            return text
                .replace(/İ/g, 'i')
                .replace(/I/g, 'ı')
                .toLowerCase();
        },

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
            //Edit patient
            if (vm.editIndex > -1) {
                vm.$store
                    .dispatch('updatePatient', {
                        id: vm.editAction.id,
                        identityNumber:  vm.editAction.identityNumber,
                        firstName:  vm.editAction.firstName,
                        lastName:  vm.editAction.lastName,
                        phone:  vm.editAction.phone,
                        address:  vm.editAction.address
                      })
                    .then(response => {
                        vm.processResult(response);
                    });
            }
            //Add patient
            else {
                //We are accessing insertPatient in vuex store
                vm.$store
                    .dispatch('insertPatient', {
                        identityNumber:  vm.editAction.identityNumber,
                        firstName:  vm.editAction.firstName,
                        lastName:  vm.editAction.lastName,
                        phone:  vm.editAction.phone,
                        address:  vm.editAction.address
                      })
                    .then(response => {
                      vm.processResult(response);
                    });
            }
        },

        processResult(response) {
          const vm = this;

            if (response.data.info.succeeded){
                vm.savedMessage= vm.$i18n.t('patient.patientSaved');
                vm.snackbarVisible = true;
            }
            else if (response.data.info.message == errorMessages.CodeIsNotUnique){
              vm.savedMessage= vm.$i18n.t('common.codeIsNotUnique');
              vm.snackbarVisible = true;
              setTimeout(() => {
                vm.snackbarVisible = false;
              }, 2300);

              return false;
            }

            vm.snackbarVisible = true;
            vm.$parent.getPatients();

            setTimeout(() => {
                vm.snackbarVisible = false;
            }, 2300);

          vm.showModal = false;
          vm.clear();
        }
    }
};
</script>
