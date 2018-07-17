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
              <v-icon>
                arrow_back
              </v-icon>
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
                <v-text-field v-model="editAction['name']"
                              :label="$t('operationtypes.operationtype')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select v-model="selectBranch"
                          :items="branches"
                          :label="$t('branches.branch')"
                          item-text="name"
                          items-value="id">
                </v-select>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select v-model="selectEquipment"
                          :items="equipments"
                          :label="$t('equipments.equipment')"
                          item-text="name"
                          items-value="id">
                </v-select>
              </v-flex>
              <v-flex xs12 sm6 md6>
                <v-select v-model="selectOperatingRoom"
                          :items="operatingRooms"
                          :label="$t('operatingrooms.operatingRoom')"
                          item-text="name"
                          items-value="id">
                </v-select>
              </v-flex>
              <v-flex xs12 sm6 md6>
                <v-select v-model="selectPersonnel"
                          :items="personnels"
                          :label="$t('personnel.personnel')"
                          item-text="name"
                          items-value="id">
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

        vm.editAction.branchName = vm.$store.state.operationTypeModule.allBranches.find(
          item => {
            if (item.id == val) {
              return item;
            }
          }
        ).name;

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

        return vm.editAction.equipmentId;
      },

      set(val) {
        const vm = this;

        vm.editAction.equipmentName = vm.$store.state.operationTypeModule.allEquipments.find(
          item => {
            if (item.id == val) {
              return item;
            }
          }
        ).name;

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

        return vm.editAction.operatingRoomId;
      },

      set(val) {
        const vm = this;

        vm.editAction.operatingRoomName = vm.$store.state.operationTypeModule.allOperatingRooms.find(
          item => {
            if (item.id == val) {
              return item;
            }
          }
        ).name;

        vm.editAction.operatingRoomId = val;
      }
    },

    personnels() {
      const vm = this;

      return vm.$store.state.operationTypeModule.allPersonnels;
    },

    selectPersonnel: {
      get() {
        const vm = this;

        return vm.editAction.personnelId;
      },

      set(val) {
        const vm = this;

        vm.editAction.personnelName = vm.$store.state.operationTypeModule.allPersonnels.find(
          item => {
            if (item.id == val) {
              return item;
            }
          }
        ).name;

        vm.editAction.personnelId = val;
      }
    }
  },

  methods: {
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
          branchId: vm.selectBranch,
          equipmentId: vm.selectEquipment,
          operatingRoomId: vm.selectOperatingRoom,
          personnelId: vm.selectPersonnel
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
          branchId: vm.selectBranch,
          equipmentId: vm.selectEquipment,
          operatingRoomId: vm.selectOperatingRoom,
          personnelId: vm.selectPersonnel
        });
      }

      vm.showModal = false;
    }
  }
};

</script>
