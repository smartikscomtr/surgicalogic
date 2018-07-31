<template>
  <div>
    <v-dialog v-model="showModal" slot="activator">
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
          <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['personnelCode']"
                              :label="$t('personnel.personnelCode')">
                </v-text-field>
              </v-flex>

              <span>&nbsp;</span>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['firstName']"
                              :label="$t('personnel.firstName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['lastName']"
                              :label="$t('personnel.lastName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectPersonnelTitle"
                                :items="personnelTitles"
                                :label="$t('personneltitle.personnelTitle')"
                                :filter="customFilter"
                                item-text="name"
                                item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectWorkType"
                                :items="workTypes"
                                :label="$t('worktypes.workType')"
                                :filter="customFilter"
                                item-text="name"
                                item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-autocomplete v-model="selectBranch"
                                :items="branches"
                                :label="$t('branches.branch')"
                                :filter="customFilter"
                                item-text="name"
                                item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right>
                <v-btn class="btnSave orange"
                       @click.native="save">
                  Kaydet
                </v-btn>
              </v-flex>
            </v-layout>
        </v-card-text>
      </v-card>
    </v-dialog>
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
    return {};
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1 ? vm.$t('personnel.addPersonnelInformation') : vm.$t('personnel.editPersonnelInformation');
    },

    showModal: {
      get() {
          const vm = this;

          return vm.editVisible;
      },

      set(value) {
          const vm = this;

          //When the cancel button is clicked, the event is sent to the personnel edit component
          if (!value) {
              vm.$emit('cancel');
          }
        }
    },

    personnelTitles() {
      const vm = this;
      return vm.$store.state.personnelModule.allPersonnelTitles;
    },

    selectPersonnelTitle: {
      get() {
        const vm = this;

        return vm.editAction.personnelTitleId;
      },

      set(val) {
        const vm = this;

        vm.editAction.personnelTitleId = val;
      }
    },

    branches() {
      const vm = this;

      return vm.$store.state.personnelModule.allBranches;
    },

    selectBranch: {
      get() {
        const vm = this;

        return vm.editAction.branchIds;
      },

      set(val) {
        const vm = this;

        vm.editAction.branchId = val;
      }
    },

    workTypes() {
      const vm = this;

      return vm.$store.state.personnelModule.allWorkTypes;
    },

    selectWorkType: {
      get() {
        const vm = this;

        return vm.editAction.workTypeId;
      },

      set(val) {
        const vm = this;

        vm.editAction.workTypeId = val;
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

      //Edit personnel
      if (vm.editIndex > -1) {
        //We are accessing updatePersonnel in vuex store
        vm.$store.dispatch('updatePersonnel', {
          id: vm.editAction.id,
          personnelCode: vm.editAction.personnelCode,
          firstName: vm.editAction.firstName,
          lastName: vm.editAction.lastName,
          personnelTitleId: vm.editAction.personnelTitleId,
          branches: vm.editAction.branchId,
          workTypeId: vm.editAction.workTypeId
        });
      }
      //Add personnel
      else {
        //We are accessing insertPersonnel in vuex store
        vm.$store.dispatch('insertPersonnel', {
          personnelCode: vm.editAction.personnelCode,
          firstName: vm.editAction.firstName,
          lastName: vm.editAction.lastName,
          personnelTitleId: vm.editAction.personnelTitleId,
          branches:  vm.editAction.branchId,
          workTypeId:  vm.editAction.workTypeId
        });
      }

      vm.showModal = false;
    }
  }
};

</script>
