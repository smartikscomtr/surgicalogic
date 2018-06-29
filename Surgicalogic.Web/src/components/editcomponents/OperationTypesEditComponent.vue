<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              max-width="500px">
      <v-card>
        <v-card-title>
          <div class="headline-wrap">
            <v-btn class="backBtn"
                  flat
                  @click.native="cancel">
              <v-icon>arrow_back</v-icon>
            </v-btn>

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
                <v-text-field v-model="actions['name']"
                              :label="$t('operationtypes.operationtype')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select v-model="selectBranchs"
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
    visible: {
      type: Boolean,
      required: false
    },

    actions: {
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

    edit: {
      type: Number,
      required: false
    }
  },

  data() {
    return {
    };
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.edit === -1
        ? vm.$i18n.t("operationtypes.addOperationType")
        : vm.$i18n.t("operationtypes.editOperationType");
    },

    showModal: {
      get() {
        const vm = this;

        return vm.visible;
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

    selectBranchs() {
      const vm = this;

      const items = vm.actions['branch'];

      _each(items, (item) => {
          item.name = vm.$store.state.branchsModule.branchs.find(d => d.id === item.id);
      });

      return items;
    }
  },

  methods: {
    save() {
      const vm = this;

      if (vm.edit > -1) {
        Object.assign(vm.items[vm.edit], vm.actions);
      }

      //Güncelleme işlemi

      // vm.$store.dispatch('updateEquipments', {
      //   id: vm.actions.id,
      //   name: vm.actions.name,
      //   type: vm.actions.type,
      //   portable: vm.actions.portable,
      //   description: vm.actions.description
      // });

      // vm.cancel();
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch("getBranchs");

    vm.$watch("deleteValue", (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm(vm.$i18n.t("common.areYouSureWantToDelete"));
        vm.visible = false;
        //Silme İşlemi
      }
    });
  }
};
</script>
