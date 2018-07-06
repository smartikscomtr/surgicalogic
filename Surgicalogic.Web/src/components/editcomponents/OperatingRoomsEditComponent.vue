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
                <v-text-field v-model="editAction['name']"
                              :label="$t('operatingrooms.operatingRoom')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['location']"
                              :label="$t('operatingrooms.location')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['width']"
                              :label="$t('operatingrooms.width')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['height']"
                              :label="$t('operatingrooms.height')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['lenght']"
                              :label="$t('operatingrooms.lenght')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['description']"
                              :label="$t('common.description')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select v-model="selectEquipments"
                          :items="equipments"
                          :label="$t('equipments.equipments')"
                          item-text="name"
                          item-value="id"
                          multiple
                          chips>
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

        if (!value) {
          vm.$emit("cancel");
        }
      }
    },

    equipments() {
      const vm = this;
      return vm.$store.state.equipmentModule.equipments;
    },

    selectEquipments() {
      const vm = this;

      const items = vm.editAction['equipmentId'];

      _each(items, (item) => {
          item.name = vm.$store.state.equipmentModule.equipments.find(d => d.id === item.id);
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
        vm.$store.dispatch("updateOperatingRoom", {
          id: vm.actions.id,
          name: vm.editAction.name,
          description: vm.editAction.description,
          width: vm.editAction.width,
          height: vm.editAction.height,
          lenght: vm.editAction.lenght
        });
      } else {
        vm.$store.dispatch("insertOperatingRoom", {
          name: vm.editAction.name,
          description: vm.editAction.description,
          width: vm.editAction.width,
          height: vm.editAction.height,
          lenght: vm.editAction.lenght
        });
      }

      vm.showModal = false;
    }
  },

  created() {
    const vm = this;

    vm.$watch('deleteValue', (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm(vm.$i18n.t('common.areYouSureWantToDelete'));

        vm.editVisible = false;
      }
    });
  }
}

</script>
