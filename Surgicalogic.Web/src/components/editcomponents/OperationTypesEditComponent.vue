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

        <v-card-text >
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['name']"
                              :label="$t('operationtypes.operationtype')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select
                          :items="branchs"
                          :label="$t('branchs.branch')"
                          item-text="name"
                          items-value="id">
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

      return vm.editIndex === -1
        ? vm.$i18n.t("operationtypes.addOperationType")
        : vm.$i18n.t("operationtypes.editOperationType");
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

    branchs() {
      const vm = this;
      return vm.$store.state.branchsModule.branchs;
    },

    // selectBranchs() {
    //   const vm = this;

    //   const items = vm.actions['branch'];

    //   _each(items, (item) => {
    //       item.name = vm.$store.state.branchsModule.branchs.find(d => d.id === item.id);
    //   });

    //   return items;
    // }
  },

  methods: {
    cancel() {
      const vm = this;

      vm.showModal = false;
    },

    save() {
      const vm = this;

      if (vm.editIndex > -1) {
        vm.$store.dispatch("updateOperationType", {
          id: vm.editAction.id,
          name: vm.editAction.name,
          type: vm.editAction.type,
          portable: vm.editAction.portable,
          description: vm.editAction.description
        });
      } else {
        vm.$store.dispatch("insertOperationType", {
          id: vm.editAction.id,
          name: vm.editAction.name,
          type: vm.editAction.type,
          portable: vm.editAction.portable,
          description: vm.editAction.description
        });
      }

      vm.showModal = false;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch("getBranchs");

    vm.$watch("deleteValue", (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm(vm.$i18n.t("common.areYouSureWantToDelete"));
        vm.editVisible = false;
        //Silme İşlemi
      }
    });
  }
};
</script>
