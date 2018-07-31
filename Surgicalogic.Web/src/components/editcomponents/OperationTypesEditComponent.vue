<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
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
              <v-flex xs12 sm6 md12>
                <v-text-field v-model="editAction['name']"
                              :label="$t('operationtypes.operationtype')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-text-field v-model="editAction['description']"
                              :label="$t('common.description')">
                </v-text-field>
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

              <v-flex xs12 sm6 md12>
                <v-autocomplete v-model="selectEquipment"
                                :items="equipments"
                                :label="$t('equipments.equipment')"
                                 multiple
                                 chips
                                 deletable-chips
                                :filter="customFilter"
                                item-text="name"
                                item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-autocomplete v-model="selectOperatingRoom"
                                :items="operatingRooms"
                                :label="$t('operatingrooms.operatingRoom')"
                                 multiple
                                 chips
                                 deletable-chips
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

      return vm.editIndex === -1 ? vm.$i18n.t('operationtypes.addOperationType') : vm.$i18n.t('operationtypes.editOperationType');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the operating types edit component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    branches() {
      const vm = this;

      return vm.$store.state.operationTypeModule.allBranches;
    },

    selectBranch: {
      get() {
        const vm = this;

        return vm.editAction.branchId;
      },

      set(val) {
        const vm = this;

        vm.editAction.branchId = val;
      }
    },

    equipments() {
      const vm = this;

      return vm.$store.state.operationTypeModule.allEquipments;
    },

    selectEquipment: {
      get() {
        const vm = this;

        return vm.editAction.equipments;
      },

      set(val) {
        const vm = this;

        vm.editAction.equipmentId = val;
      }
    },

    operatingRooms() {
      const vm = this;

      return vm.$store.state.operationTypeModule.allOperatingRooms;
    },

    selectOperatingRoom: {
      get() {
        const vm = this;

        return vm.editAction.operatingRoomIds;
      },

      set(val) {
        const vm = this;

        vm.editAction.operatingRoomId = val;
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

      //Edit operation type
      if (vm.editIndex > -1) {
        //We are accessing updateOperationType in vuex store
        vm.$store.dispatch('updateOperationType', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          type: vm.editAction.type,
          portable: vm.editAction.portable,
          description: vm.editAction.description,
          branchId: vm.editAction.branchId,
          equipments: vm.editAction.equipmentId,
          operatingRoomIds: vm.editAction.operatingRoomId
        });
      }

      //Add operation type
      else {
        //We are accessing insertOperationType in vuex store
        vm.$store.dispatch('insertOperationType', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          type: vm.editAction.type,
          portable: vm.editAction.portable,
          description: vm.editAction.description,
          branchId: vm.editAction.branchId,
          equipments: vm.editAction.equipmentId,
          operatingRoomIds: vm.editAction.operatingRoomId
        });
      }

      vm.showModal = false;
    }
  }
};

</script>
