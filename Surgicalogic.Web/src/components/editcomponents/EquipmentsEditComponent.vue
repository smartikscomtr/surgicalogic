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
              <v-flex xs12 sm6 md12>
                <v-text-field v-model="editAction['name']"
                              :label="$t('equipments.name')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-select v-model="selectEquipmentType"
                          :items="equipmentTypes"
                          :label="$t('equipmenttypes.equipmentTypes')"
                          item-text="name"
                          item-value="id">
                </v-select>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-text-field v-model="editAction['description']"
                              :label="$t('equipments.description')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-checkbox v-model="editAction['isPortable']"
                              :label="$t('equipments.portable')"
                              color="primary">
                </v-checkbox>
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
      selectEquipmentType : {}
    };
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1 ? vm.$i18n.t("equipments.addEquipmentsInformation") : vm.$i18n.t("equipments.editEquipmentsInformation");
    },

    showModal: {
      get() {
        const vm = this;
        
        return vm.editVisible;
      },
      set (value) {
        const vm = this;

        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    equipmentTypes() {
      const vm = this;

      return vm.$store.state.equipmentTypesModule.equipmentTypes;
    }

    // selectEquipmentType() {
    //   const vm = this;

    //   const items = vm.actions['equipmentTypes'];

    //   _each(items, (item) => {
    //       item.name = vm.$store.state.equipmentTypesModule.equipmentTypes.find(d => d.id === item.id);
    //   });

    //   return items;
    // }
  },  
  methods: {
    save() {
      const vm = this;

      if (vm.editIndex > -1) {
        
          vm.$store.dispatch('updateEquipment',{
            id : vm.editAction.id,
            name: vm.editAction.name,
            description: vm.editAction.description,
            isPortable: vm.editAction.isPortable,
            equipmentTypeId: vm.selectEquipmentType            
          });

      }else{
          vm.$store.dispatch('insertEquipment', {
            name: vm.editAction.name,
            description: vm.editAction.description,
            isPortable: vm.editAction.isPortable,
            equipmentTypeId: vm.selectEquipmentType
          });
      }

    }
  },

  created() {
    const vm = this;

    

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
