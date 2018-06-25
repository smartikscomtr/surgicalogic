<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">
            {{ formTitle }}
          </span>
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
                <v-select v-model="branchs"
                          :items="branchs"
                          :label="$t('branchs.branch')">
                </v-select>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="blue darken-1"
                 flat
                 @click.native="cancel">
            Cancel
          </v-btn>
          <v-btn color="blue darken-1"
                 flat
                 @click.native="save">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

import _each from 'lodash/each';
import _map from 'lodash/map';

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

    // branchTitle() {
    //   const vm = this;

    //   // _each(this.$store.state.operationTypeModule.operationTypes, (item) => item.branchTitle = item.branch[0].name );
    //   console.log(vm.actions['branch']);
    //   return vm.actions['branch'].name;
    // },


    branchs() {
      const vm = this;

      console.log('branchs', vm.$store.state.branchsModule.branch[0].name);
      return vm.$store.state.branchsModule.branch;
    },

    // selectBranchs() {
    //   const vm = this;

    //   const items = vm.$store.state.branchModule.branch;

    //   // console.log("branch", items);

    //   _each(items, (item) => {
    //       item.name = vm.$store.state.branchModule.branch.find(d => d.id === item.id);
    //       // console.log(item.name)
    //   });

    //   return items;
    // }

    // operationTypes() {
    //   const vm = this;

    //   const items = _map(vm.$store.state.operationTypeModule.operationTypes);

    //   _each(items, (item) => {
    //     if (item.branch) {
    //       item.branch.name = vm.$store.state.branchModule.branch.name;
    //       console.log(item.branch.name);
    //     }
    //   });

    //   return items;
    // }
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

    vm.$store.dispatch("getBranch");

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
