<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
      <v-card>
        <v-card-title>
          <div class="headline-wrap">
            <a class="backBtn"
                  flat
                   @click="cancel">
              <v-icon>arrow_back</v-icon>
            </a>

            <span class="text">
              {{ formTitle }}
            </span>

            <v-btn class="btnSave"
                  flat
                  @click.native="save">
              Save
            </v-btn>
          </div>
        </v-card-title>

        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['personnelCode']"
                              :label="$t('personnel.personnelCode')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['givenName']" :label="$t('personnel.givenName')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['familyName']" :label="$t('personnel.familyName')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="actions['task']" :label="$t('personnel.task')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="actions['branch']" :label="$t('personnel.branch')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="actions['shift']" :label="$t('personnel.shift')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md12>
                <v-select v-model="selectWorkTypes"
                          :items="workTypes"
                          :label="$t('worktypes.workTypes')"
                          item-text="name"
                          item-value="id">
                </v-select>
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
    return {};
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

        if (!value) {
          vm.$emit("cancel");
        }
      }
    },

    workTypes() {
      const vm = this;

      return vm.$store.state.workTypesModule.workTypes;
    },

    selectWorkTypes() {
      const vm = this;

      const items = vm.actions['workTypes'];

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

      if (vm.editIndex > -1) {
        vm.$store.dispatch("updateBranch", {
          id: vm.editAction.id,
          personnelCode: vm.editAction.personnelCode,
          givenName: vm.editAction.givenName,
          familyName: vm.editAction.familyName,
          tasks: vm.editAction.tasks,
          branch: vm.editAction.branch,
          shift: vm.editAction.shift,
          workType: vm.editAction.workType
        });
      } else {
        vm.$store.dispatch("insertBranch", {
          id: vm.editAction.id,
          personnelCode: vm.editAction.personnelCode,
          givenName: vm.editAction.givenName,
          familyName: vm.editAction.familyName,
          tasks: vm.editAction.tasks,
          branch: vm.editAction.branch,
          shift: vm.editAction.shift,
          workType: vm.editAction.workType
        });
      }

      vm.showModal = false;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch('getWorkTypes');

    vm.$watch('deleteValue', (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm(vm.$i18n.t('common.areYouSureWantToDelete'));
        vm.editVisible = false;

        //Silme İşlemi
      }
    });
  }
}

</script>
