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
              Kaydet
            </v-btn>
          </div>
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

    editIndex: {
      type: Number,
      required: false
    }
  },

  data() {
    return {}
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
      
      return vm.$store.state.equipmentModule.allEquipmentTypes;
    },  
    selectEquipmentType: {
     get : function(){
        return this.editAction.equipmentTypeId
     },
     set: function(val){
        this.editAction.equipmentTypeName = this.$store.state.equipmentModule.allEquipmentTypes.find(function(item) { if(item.id == val) return item }).name       
        this.editAction.equipmentTypeId = val
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

      if (vm.editIndex > -1) {
          vm.$store.dispatch('updateEquipment',{
            id : vm.editAction.id,
            name: vm.editAction.name,
            description: vm.editAction.description,
            isPortable: vm.editAction.isPortable,
            equipmentTypeId: vm.selectEquipmentType                         
          });

      } else {
        console.log(vm)
        vm.$store.dispatch('insertEquipment', {
          name: vm.editAction.name,
          description: vm.editAction.description,
          isPortable: vm.editAction.isPortable,
          equipmentTypeId: vm.selectEquipmentType

        });
      }

      vm.showModal = false;
    }
  }
}

</script>
