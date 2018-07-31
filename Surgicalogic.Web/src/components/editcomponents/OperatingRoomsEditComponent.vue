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
            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['name']"
                            :label="$t('operatingrooms.operatingRoom')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['location']"
                            :label="$t('operatingrooms.location')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md4>
              <v-text-field v-model="editAction['width']"
                            :label="$t('operatingrooms.width')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md4>
              <v-text-field v-model="editAction['height']"
                            :label="$t('operatingrooms.height')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md4>
              <v-text-field v-model="editAction['length']"
                            :label="$t('operatingrooms.length')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md12>
              <v-text-field v-model="editAction['description']"
                            :label="$t('common.description')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md12>
              <v-autocomplete v-model="selectEquipment"
                              :items="equipments"
                              :label="$t('equipments.equipments')"
                              :filter="customFilter"
                              multiple
                              chips
                              deletable-chips
                              item-text="name"
                              item-value="id">
              </v-autocomplete>
            </v-flex>

              <v-flex xs12 sm6 md12>
              <v-autocomplete v-model="selectOperationType"
                        :items="operationTypes"
                        :label="$t('operationtypes.operationType')"
                        :filter="customFilter"
                        multiple
                        chips
                        deletable-chips
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

      return vm.editIndex === -1 ? vm.$t('operatingrooms.addOperatingRoomInformation') : vm.$t('operatingrooms.editOperatingRoomInformation');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the operating rooms edit component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    equipments() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.nonPortableEquipments;
    },

    operationTypes() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.allOperationTypes;
    },

    selectEquipment: {
      get() {
        const vm = this;

         let selectedEquipments = [];

        if (vm.editAction.operatingRoomEquipments)
        {
          vm.editAction.operatingRoomEquipments.forEach(item => {
            selectedEquipments.push(item.equipment.id);
          });
        }

        return selectedEquipments;
      },

      set(val) {
        const vm = this;

        vm.editAction.equipmentId = val;
      }
    },

    selectOperationType: {
      get() {
        const vm = this;

        let selectedOperationTypes = [];

        if (vm.editAction.operatingRoomOperationTypes)
        {
          vm.editAction.operatingRoomOperationTypes.forEach(item => {
            selectedOperationTypes.push(item.operationType.id);
          });
        }

        return selectedOperationTypes;
      },

      set(val) {
        const vm = this;

        vm.editAction.operationTypeId = val;
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

      //Edit operating room
      if (vm.editIndex > -1) {
        //We are accessing updateOperatingRoom in vuex store
        vm.$store.dispatch('updateOperatingRoom', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          description: vm.editAction.description,
          location: vm.editAction.location,
          width: vm.editAction.width,
          height: vm.editAction.height,
          length: vm.editAction.length,
          equipments: vm.editAction.equipmentId,
          operatingRoomEquipments: vm.editAction.equipmentId,
          operationTypes: vm.editAction.operationTypeId,
          operatingRoomOperationTypes: vm.selectOperationType
        });

      }
      //Add operating room
      else {
        //We are accessing insertOperatingRoom in vuex store
        vm.$store.dispatch('insertOperatingRoom', {
          name: vm.editAction.name,
          description: vm.editAction.description,
          location: vm.editAction.location,
          width: vm.editAction.width,
          height: vm.editAction.height,
          length: vm.editAction.length,
          equipments: vm.editAction.equipmentId,
          operatingRoomEquipments: vm.editAction.equipmentId,
          operationTypes: vm.editAction.operationTypeId,
          operatingRoomOperationTypes: vm.selectOperationType
        });
      }

      vm.showModal = false;
    }
  }
}

</script>
