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
                <v-text-field v-model="planOperationAction['eventNumber']" :rules="required" :label="$t('operation.eventNo')">
                </v-text-field>
                 <!-- @change="generateOperationName" -->
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="planOperationAction['fullName']" :label="$t('personnel.patient')" disabled>
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
                    :locale="getLocale()" :min="getMinDate()">
                  </v-date-picker>
                </v-menu>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="selectOperationTime" :rules="required" :label="$t('operation.operationTime')" :value="planOperationAction['operationTime']" type="time">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectOperatingRoom" :items="filteredOperatingRooms" :label="$t('operatingrooms.blockedOperatingRooms')" :filter="customFilter"
                              clearable multiple chips deletable-chips item-text="name" item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm12 md12>
                <v-textarea v-model="planOperationAction['description']" rows="3" :label="$t('common.description')">
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

import { localizationMixin } from './../../mixins/localizationMixin';

export default {
  mixins: [
    localizationMixin
  ],

  props: {
        planOperationVisible: {
            type: Boolean,
            required: false
        },

        planOperationAction: {
            type: Object,
            required: false,
            default() {
                return {};
            }
        },

        planOperationIndex: {
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
            selectedPatient: null,
            required: [v => !!v || this.$i18n.t('common.required')],
            multipleRequired: [
                v => !!v && v.length > 0 || this.$i18n.t('common.required')
            ],
            eventNumber: vm.planOperationAction['eventNumber']
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
              vm.planOperationAction['eventNumber'] ?
              vm.planOperationAction['eventNumber'] : "") + " " + (vm.planOperationAction['fullName'] ?
              vm.planOperationAction['fullName'] : "");

          },

          set(val) {
            const vm = this;

            vm.planOperationAction.operationName = val;
          }
        },

        formTitle() {
          const vm = this;

          return vm.$i18n.t('operation.addOperationInformation') ;
        },

        showModal: {
            get() {
                const vm = this;

                return vm.planOperationVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the work types edit component
                if (!value) {
                    vm.$emit('cancel');
                }
            }
        },

        operationTypes() {
          const vm = this;

          return vm.$store.state.operationModule.filteredOperationTypes;
        },

        patients() {
          const vm = this;

          return vm.$store.state.patientModule.allPatients;
        },

        selectOperationType: {
          get() {
            const vm = this;

            return vm.planOperationAction.operationTypeId;
          },

          set(val) {
            const vm = this;

            vm.planOperationAction.operationTypeId = val;
          }
        },

        selectPersonnel: {
          get() {
            const vm = this;

            return vm.planOperationAction.personnelIds;
          },

          set(val) {
            const vm = this;

            vm.planOperationAction.personnelIds = val;
          }
        },

        selectPatient: {
          get() {
            const vm = this;

            return vm.planOperationAction.patientId;
          },

          set(val) {
            const vm = this;

            vm.planOperationAction.patientId = val;
          }
        },

        selectOperatingRoom: {
          get() {
            const vm = this;

            return vm.planOperationAction.blockedOperatingRoomIds;
          },

          set(val) {
            const vm = this;

            vm.planOperationAction.blockedOperatingRoomIds = val;
          }
        },

        selectOperationTime: {
          get() {
            const vm = this;

            return vm.planOperationAction.operationTime;
          },

          set(val) {
            const vm = this;

            vm.planOperationAction.operationTime = val;
          }
        },

        branches() {
          const vm = this;

          return vm.$store.state.operationModule.allBranches;
        },

        date: {
          get() {
            const vm = this;

            if (vm.planOperationAction.date) {
              vm.$store.commit('saveGlobalDate', vm.planOperationAction.date);
            }

            vm.dateFormatted = this.formatDate(vm.$store.state.operationModule.globalDate);
            return vm.$store.state.operationModule.globalDate;
          },

          set(newValue) {
            const vm = this;

            if (newValue) {
              vm.planOperationAction.date = newValue;
              vm.$store.commit('saveGlobalDate', vm.planOperationAction.date);
            }
          }
        }
    },

    methods: {
      handleChange(val) {
          const vm = this;

          vm.selectedPatient = val;
          vm.operationName = "";
      },

      customFilter(item, queryText, itemText) {
          const vm = this;

          const text = vm.replaceForAutoComplete(item.name);
          const searchText = vm.replaceForAutoComplete(queryText);

          return text.indexOf(searchText) > -1;
      },

      customFilterForFullName(item, queryText, itemText) {
        const vm = this;

        const text = vm.replaceForAutoComplete(item.fullName);
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

          vm.planOperationAction.date = null;
          vm.$store.commit('saveGlobalDate', null);
          vm.dateFormatted = null;
      },

      save() {
          const vm = this;

          if (!vm.$refs.form.validate()) {
              return;
          }

          vm.snackbarVisible = false;

              //We are accessing insertOperation in vuex store
          vm.$store
              .dispatch('insertOperation', {
                  name: vm.operationName,
                  date: vm.planOperationAction.date,
                  operationTime: vm.planOperationAction.operationTime,
                  description: vm.planOperationAction.description,
                  operationTypeId: vm.planOperationAction.operationTypeId,
                  personnelIds: vm.planOperationAction.personnelIds,
                  operatingRoomIds: vm.planOperationAction.blockedOperatingRoomIds,
                  patientId: vm.planOperationAction.id,
                  eventNumber: vm.planOperationAction.eventNumber
              })
              .then(response => {
                  vm.processResult(response);
              });
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

        setTimeout(() => {
            vm.snackbarVisible = false;
        }, 2300);

        vm.showModal = false;
        vm.clear();
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

          if (vm.planOperationAction.operationTypeId) {
              vm.$store
                  .dispatch('getPersonnelsByOperationTypeId', {
                      operationTypeId: vm.planOperationAction.operationTypeId
                  })
                  .then(() => {
                      vm.filterPersonnels =
                          vm.$store.state.operationModule.filteredPersonnels;
                  });
          }

          if (vm.planOperationAction.operationTypeId) {
              vm.$store
                  .dispatch('getOperatingRoomsByOperationTypeId', {
                      operationTypeId: vm.planOperationAction.operationTypeId
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


