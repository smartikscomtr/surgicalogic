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
                <v-text-field v-model="editAction['patientIdentityNumber']"
                              mask="###########"
                              :label="$t('operation.patientIdentityNumber')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['eventNumber']" :rules="required" :label="$t('operation.eventNo')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['patientFirstName']" :rules="required" :label="$t('operation.patientFirstName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['patientLastName']" :rules="required" :label="$t('operation.patientLastName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="operationName" disabled :rules="required" :label="$t('operation.operationName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectOperationType" :rules="required" :items="operationTypes" :label="$t('operation.operationType')" :filter="customFilter"
                              clearable @change="operationTypeChanged()" item-text="name" item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectPersonnel" :items="filterPersonnels" :label="$t('personnel.personnel')" :filter="customFilter"
                              clearable multiple chips deletable-chips item-text="personnelCategoryName" item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-menu ref="menu" :close-on-content-click="false" v-model="menu" :nudge-right="40" :return-value.sync="date" lazy transition="scale-transition" offset-y full-width min-width="290px">
                  <v-text-field readonly clearable slot="activator" :rules="required" v-model="dateFormatted" :label="$t('operation.operationDate')">
                  </v-text-field>

                  <v-date-picker v-model="date" no-title @input="$refs.menu.save(date)"
                    locale="tr-TR" :min="getMinDate()">
                  </v-date-picker>
                </v-menu>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="selectOperationTime" :rules="required" :label="$t('operation.operationTime')" :value="editAction['operationTime']" type="time">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectOperatingRoom" :items="filteredOperatingRooms" :label="$t('operatingrooms.blockedOperatingRooms')" :filter="customFilter"
                              clearable multiple chips deletable-chips item-text="name" item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm12 md12>
                <v-textarea v-model="editAction['description']" rows="3" :label="$t('common.description')">
                </v-textarea>
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
        const vm = this;

        return {
            filterOperationTypes: [],
            filterPersonnels: [],
            filteredOperatingRooms: [],
            snackbarVisible: null,
            savedMessage: vm.$i18n.t('operation.operationSaved'),
            menu: false,
            dateFormatted: null,
            valid: true,
            required: [v => !!v || this.$i18n.t('common.required')],
            multipleRequired: [
                v => !!v && v.length > 0 || this.$i18n.t('common.required')
            ]
        };
    },

    watch: {
        showModal(val) {
            val || this.cancel();
        },

        date(val) {
            this.dateFormatted = this.formatDate(this.date);
        }
    },

    computed: {
        operationName: {
          get() {
            const vm = this;

            return (
              vm.editAction['eventNumber'] ?
              vm.editAction['eventNumber'] : "") + (vm.editAction['patientFirstName'] ?
              " " + vm.editAction['patientFirstName'] : "") + (vm.editAction['patientLastName'] ?
              " " + vm.editAction['patientLastName'] :
              ""
            );
          },

          set(val) {
            const vm = this;

            vm.editAction.operationName = val;
          }
        },

        formTitle() {
          const vm = this;

          return vm.editIndex === -1 ?
            vm.$i18n.t('operation.addOperationInformation') : vm.$i18n.t('operation.editOperationInformation');
        },

        showModal: {
            get() {
                const vm = this;

                return vm.editVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the operations edit component
                if (!value) {
                    vm.$emit('cancel');
                }
            }
        },

        operationTypes() {
          const vm = this;

          return vm.$store.state.operationModule.filteredOperationTypes;
        },

        selectOperationType: {
          get() {
            const vm = this;

            return vm.editAction.operationTypeId;
          },

          set(val) {
            const vm = this;

            vm.editAction.operationTypeId = val;
          }
        },

        selectPersonnel: {
          get() {
            const vm = this;

            return vm.editAction.personnelIds;
          },

          set(val) {
            const vm = this;

            vm.editAction.personnelIds = val;
          }
        },

        selectOperatingRoom: {
          get() {
            const vm = this;

            return vm.editAction.blockedOperatingRoomIds;
          },

          set(val) {
            const vm = this;

            vm.editAction.blockedOperatingRoomIds = val;
          }
        },

        selectOperationTime: {
          get() {
            const vm = this;

            return vm.editAction.operationTime;
          },

          set(val) {
            const vm = this;

            vm.editAction.operationTime = val;
          }
        },

        branches() {
          const vm = this;

          return vm.$store.state.operationModule.allBranches;
        },

        date: {
          get() {
            const vm = this;

            if (vm.editAction.date) {
              vm.$store.commit('saveGlobalDate', vm.editAction.date);
            }

            vm.dateFormatted = this.formatDate(vm.$store.state.operationModule.globalDate);
            return vm.$store.state.operationModule.globalDate;
          },

          set(newValue) {
            const vm = this;

            if (newValue) {
              vm.editAction.date = newValue;
              vm.$store.commit('saveGlobalDate', vm.editAction.date);
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

        clear() {
          const vm = this;

            vm.$refs.form.reset();

            vm.editAction.date = null;
            vm.$store.commit('saveGlobalDate', null);
            vm.dateFormatted = null;
        },

        save() {
            const vm = this;

            if (!vm.$refs.form.validate()) {
                return;
            }

            vm.snackbarVisible = false;

            //Edit operation
            if (vm.editIndex > -1) {
                //We are accessing updateOperation in vuex store
                vm.$store
                    .dispatch('updateOperation', {
                        id: vm.editAction.id,
                        name: vm.operationName,
                        date: vm.editAction.date,
                        operationTime: vm.editAction.operationTime,
                        description: vm.editAction.description,
                        operationTypeId: vm.editAction.operationTypeId,
                        personnelIds: vm.editAction.personnelIds,
                        operatingRoomIds: vm.editAction.blockedOperatingRoomIds,
                        patientIdentityNumber: vm.editAction.patientIdentityNumber,
                        patientFirstName: vm.editAction.patientFirstName,
                        patientLastName: vm.editAction.patientLastName,
                        eventNumber: vm.editAction.eventNumber
                    })
                    .then(response => {
                        vm.processResult(response);
                    });
            }

            //Add operation
            else {
                //We are accessing insertOperation in vuex store
                vm.$store
                    .dispatch('insertOperation', {
                        name: vm.operationName,
                        date: vm.editAction.date,
                        operationTime: vm.editAction.operationTime,
                        description: vm.editAction.description,
                        operationTypeId: vm.editAction.operationTypeId,
                        personnelIds: vm.editAction.personnelIds,
                        operatingRoomIds: vm.editAction.blockedOperatingRoomIds,
                        patientIdentityNumber: vm.editAction.patientIdentityNumber,
                        patientFirstName: vm.editAction.patientFirstName,
                        patientLastName: vm.editAction.patientLastName,
                        eventNumber: vm.editAction.eventNumber
                    })
                    .then(response => {
                        vm.processResult(response);
                    });
            }

            vm.showModal = false;
            vm.clear();
        },

        processResult(response) {
          const vm = this;

          if (response.data.info.succeeded){
              vm.savedMessage= vm.$i18n.t('operation.operationSaved');
              vm.snackbarVisible = true;
          }
          else if (response.data.info.message == errorMessages.EventNumberIsNotDifferent){
            vm.savedMessage= vm.$i18n.t('common.eventNumberIsNotDifferent');
            vm.snackbarVisible = true;
            setTimeout(() => {
              vm.snackbarVisible = false;
            }, 2300);

            return false;
          }

          vm.snackbarVisible = true;
          vm.$parent.getOperations();

          setTimeout(() => {
              vm.snackbarVisible = false;
          }, 2300);
        },

        getMinDate() {
            const toTwoDigits = num => (num < 10 ? '0' + num : num);
            let tomorrow = new Date();

            tomorrow.setDate(tomorrow.getDate() + 1);

            let year = tomorrow.getFullYear();
            let month = toTwoDigits(tomorrow.getMonth() + 1);
            let day = toTwoDigits(tomorrow.getDate());

            return `${year}-${month}-${day}`;
        },

        operationTypeChanged() {
            const vm = this;

            if (vm.editAction.operationTypeId) {
                vm.$store
                    .dispatch('getPersonnelsByOperationTypeId', {
                        operationTypeId: vm.editAction.operationTypeId
                    })
                    .then(() => {
                        vm.filterPersonnels =
                            vm.$store.state.operationModule.filteredPersonnels;
                    });
            }

            if (vm.editAction.operationTypeId) {
                vm.$store
                    .dispatch('getOperatingRoomsByOperationTypeId', {
                        operationTypeId: vm.editAction.operationTypeId
                    })
                    .then(() => {
                        vm.filteredOperatingRooms =
                            vm.$store.state.operationModule.filteredOperatingRooms;
                    });
            }
        },

        formatDate(date) {
            if (!date || date.indexOf('.') > -1) return null;

            const [year, month, day] = date.split('-');

            return `${day}.${month}.${year}`;
        }
    }
};
</script>
