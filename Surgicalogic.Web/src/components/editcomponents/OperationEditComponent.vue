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
            <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['name']"
                              :label="$t('operation.operationName')">
                </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-autocomplete v-model="selectOperationType"
                              :items="operationTypes"
                              :label="$t('operation.operationType')"
                              :filter="customFilter"
                              @change="operationTypeChanged()"
                              item-text="name"
                              item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-autocomplete v-model="selectPersonnel"
                              :items="filterPersonnels"
                              :label="$t('personnel.personnel')"
                              :filter="customFilter"
                              multiple
                              chips
                              deletable-chips
                              item-text="personnelTitleName"
                              item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-menu ref="menu"
                      :close-on-content-click="false"
                      v-model="menu"
                      :nudge-right="40"
                      :return-value.sync="date"
                      lazy
                      transition="scale-transition"
                      offset-y
                      full-width
                      min-width="290px">
                <v-text-field readonly
                              slot="activator"
                              v-model="dateFormatted"
                              :label="$t('operation.operationDate')">
                </v-text-field>

                <v-date-picker v-model="date"
                                no-title
                                @input="$refs.menu.save(date)"
                                :min="getMinDate()">
                </v-date-picker>
                </v-menu>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="selectOperationTime"
                              :label="$t('operation.operationTime')"
                              :value="editAction['operationTime']"
                              type="time">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectOperatingRoom"
                                :items="filteredOperatingRooms"
                                :label="$t('operatingrooms.blockedOperatingRooms')"
                                :filter="customFilter"
                                multiple
                                chips
                                deletable-chips
                                item-text="name"
                                item-value="id">
                </v-autocomplete>
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
    const vm = this;

    return {
      filterOperationTypes: [],
      filterPersonnels: [],
      filteredOperatingRooms:[],
      snackbarVisible: null,
      savedMessage: vm.$i18n.t('operation.operationSaved'),
      menu: false,
      dateFormatted: null
    };
  },

  watch: {
    showModal (val) {
      val || this.cancel()
    },

    date (val) {
      this.dateFormatted = this.formatDate(this.date)
    }
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1 ? vm.$i18n.t('operation.addOperationInformation') : vm.$i18n.t('operation.editOperationInformation');
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
      get(){
        const vm = this;

          if (vm.editAction.date) {
            vm.$store.commit('saveGlobalDate',  vm.editAction.date);
          }

          return vm.$store.state.operationModule.globalDate;
      },

      set(newValue){
        const vm = this;

        if (newValue) {
          vm.editAction.date = newValue;
          vm.$store.commit('saveGlobalDate', vm.editAction.date);
        }
      }
     }
  },

  methods: {
    customFilter (item, queryText, itemText) {
      const vm = this;

      const text = vm.replaceForAutoComplete(item.name);
      const searchText = vm.replaceForAutoComplete(queryText);

      return text.indexOf(searchText) > -1;
    },

    replaceForAutoComplete(text)
    {
      return text.replace(/İ/g, 'i').replace(/I/g, 'ı').toLowerCase();
    },

    cancel() {
      const vm = this;

      vm.showModal = false;
    },

    save() {
      const vm = this;

      vm.snackbarVisible = false;

      //Edit operation
      if (vm.editIndex > -1) {
        //We are accessing updateOperation in vuex store
        vm.$store.dispatch('updateOperation', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          date: vm.editAction.date,
          operationTime: vm.editAction.operationTime,
          description: vm.editAction.description,
          operationTypeId: vm.editAction.operationTypeId,
          personnelIds: vm.editAction.personnelIds,
          operatingRoomIds: vm.editAction.blockedOperatingRoomIds
        }).then(() => {
          vm.snackbarVisible = true;

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300)
        })
      }

      //Add operation
      else {
        //We are accessing insertOperation in vuex store
        vm.$store.dispatch('insertOperation', {
          name: vm.editAction.name,
          date: vm.editAction.date,
          operationTime: vm.editAction.operationTime,
          description: vm.editAction.description,
          operationTypeId: vm.editAction.operationTypeId,
          personnelIds: vm.editAction.personnelIds,
          operatingRoomIds: vm.editAction.blockedOperatingRoomIds
        }).then(() => {
          vm.snackbarVisible = true;
          vm.$store.dispatch('getOperations');

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300)
        })
      }

      vm.showModal = false;
    },

    getMinDate () {
      const toTwoDigits = num => num < 10 ? '0' + num : num;
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
        vm.$store.dispatch('getPersonnelsByOperationTypeId', {
          operationTypeId: vm.editAction.operationTypeId
        }).then(() => {
          vm.filterPersonnels = vm.$store.state.operationModule.filteredPersonnels;
        });
      }

      if (vm.editAction.operationTypeId) {
        vm.$store.dispatch('getOperatingRoomsByOperationTypeId', {
          operationTypeId: vm.editAction.operationTypeId
        }).then(() => {
          vm.filteredOperatingRooms = vm.$store.state.operationModule.filteredOperatingRooms;
        });
      }
    },

    formatDate (date) {
      if (!date || date.indexOf('.')> -1)
        return null;

      const [year, month, day] = date.split('-');

      return `${day}.${month}.${year}`;
    }
  }
}

</script>
