<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <a class="backBtn"
               flat
               @click="cancel">
              <v-icon>arrow_back</v-icon>
            </a>

            <span class="text">
              {{ formTitle }}
            </span>
          </div>
        </v-card-title>

        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['personnelCode']"
                              :label="$t('personnel.personnelCode')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['firstName']"
                              :label="$t('personnel.givenName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['lastName']"
                              :label="$t('personnel.familyName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select v-model="selectPersonnelTitle"
                          :items="personnelTitles"
                          :label="$t('personnel.personnelTitle')"
                          item-text="name"
                          item-value="id">
                </v-select>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select v-model="selectBranch"
                          :items="branchs"
                          :label="$t('personnel.branch')"
                          item-text="name"
                          item-value="id">
                </v-select>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select v-model="selectWorkType"
                          :items="workTypes"
                          :label="$t('worktypes.workTypes')"
                          item-text="name"
                          item-value="id">
                </v-select>
              </v-flex>
               <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right>
               <v-btn class="btnSave orange"
                      @click.native="save">
                  Kaydet
               </v-btn>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

import _each from 'lodash/each';

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

    deleteValue: {
      type: Object,
      required: false
    },

    editIndex: {
      type: Number,
      required: false
    }
  },

  data() {
    return {
      selectPersonnelTitle: {},
      selectBranch: {},
      selectWorkType: {}
    };
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.edit === -1 ? vm.$t('personnel.addPersonnelInformation') : vm.$t('personnel.editPersonnelInformation');
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

      return vm.$store.state.personnelTitleModule.personnelTitle;
    },

    branchs() {
      const vm = this;

      return vm.$store.state.branchsModule.branchs;
    },

    workTypes() {
      const vm = this;

      return vm.$store.state.workTypesModule.workTypes;
    },

    selectPersonnelTitle() {
      const vm = this;

      const items = vm.editAction['personnelTitle'];

      _each(items, (item) => {
          item.name = vm.$store.state.personnelTitleModule.personnelTitle.find(d => d.id === item.id);
      });

      return items;
    },

    selectBranch() {
      const vm = this;

      const items = vm.editAction['branchs'];

      _each(items, (item) => {
          item.name = vm.$store.state.branchsModule.branchs.find(d => d.id === item.id);
      });

      return items;
    },

    selectWorkType() {
      const vm = this;

      const items = vm.editAction['workTypes'];

      _each(items, (item) => {
          item.name = vm.$store.state.workTypesModule.workTypes.find(d => d.id === item.id);
      });

      return items;
    }
  },

  methods: {
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
          personnelTitleId: vm.selectPersonnelTitle,
          branchId: vm.selectBranch,
          workTypeId: vm.selectWorkType
        });
      }
      //Add personnel
      else {
        //We are accessing insertPersonnel in vuex store
        vm.$store.dispatch('insertPersonnel', {
          personnelCode: vm.editAction.personnelCode,
          firstName: vm.editAction.firstName,
          lastName: vm.editAction.lastName,
          personnelTitleId: vm.selectPersonnelTitle,
          branchId: vm.selectBranch,
          workTypeId: vm.selectWorkType
        });
      }

      vm.showModal = false;
    }
  },

  created() {
    const vm = this;
    //We are accessing getBranchs, getPersonnelTitles and getWorkTypes in vuex store
    vm.$store.dispatch('getBranchs');
    vm.$store.dispatch('getPersonnelTitles');
    vm.$store.dispatch('getWorkTypes');

    //The deleteValue prop is followed and when the value is changed, confirm message is displayed to the user
    vm.$watch('deleteValue', (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm(vm.$i18n.t('common.areYouSureWantToDelete'));

        vm.editVisible = false;
      }
    });
  }
}

</script>
