<template>
  <div>
    <v-dialog v-model="showModal" slot="activator" persistent>
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
              {{ formTitle }}
            </span>

            <v-icon @click="cancel" class="close-wrap">
              close
            </v-icon>
          </div>
        </v-card-title>

        <v-form ref="form" v-model="valid" lazy-validation>
        <v-card-text>
          <v-layout wrap edit-layout>
            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['name']"
                            :rules="required"
                            :label="$t('equipments.name')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['code']"
                            :rules="required"
                            :label="$t('equipments.equipmentCode')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-autocomplete v-model="selectEquipmentType"
                              :items="equipmentTypes"
                              :label="$t('equipmenttypes.equipmentType')"
                              :filter="customFilter"
                              clearable
                              item-text="name"
                              item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6 input-group-checkbox>
              <v-checkbox v-model="editAction['isPortable']"
                          :label="$t('equipments.portable')"
                          color="primary">
              </v-checkbox>
            </v-flex>

            <v-flex v-if="!editAction['isPortable']" xs12 sm12 md12>
              <v-autocomplete v-model="selectOperatingRoom"
                              :items="operatingRooms"
                              :label="$t('operatingrooms.operatingRoom')"
                              :filter="customFilter"
                              multiple
                              chips
                              clearable
                              deletable-chips
                              item-text="name"
                              item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm12 md12>
              <v-textarea v-model="editAction['description']" rows="3" :label="$t('equipments.description')">
              </v-textarea>
            </v-flex>
          </v-layout>
        </v-card-text>

          <v-card-actions class="justify-end flex">
              <v-btn class="orange" @click.native="save">
              {{ $t('menu.save') }}
              </v-btn>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>

    <snackbar-component :snackbar-visible="snackbarVisible" :savedMessage="savedMessage">
    </snackbar-component>
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
        return {
            snackbarVisible: null,
            savedMessage: this.$i18n.t('equipments.equipmentSaved'),
            valid: true,
            required: [
              v => !!v || this.$i18n.t('common.required')
            ],
            multipleRequired: [
              v => !!v && v.length > 0 || this.$i18n.t('common.required')
            ]
        };
    },

    watch: {
        showModal(val) {
            val || this.cancel();
        }
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.editIndex === -1
                ? vm.$i18n.t('equipments.addEquipmentsInformation')
                : vm.$i18n.t('equipments.editEquipmentsInformation');
        },

        showModal: {
            get() {
                const vm = this;

                return vm.editVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the equipments edit component
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
            get() {
                const vm = this;

                return vm.editAction.equipmentTypeId;
            },

            set(val) {
                const vm = this;

                vm.editAction.equipmentTypeId = val;
            }
        },

        operatingRooms() {
            const vm = this;

            return vm.$store.state.operatingRoomModule.allOperatingRooms;
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
        customFilter(item, queryText, itemText) {
            const vm = this;

            const text = vm.replaceForAutoComplete(item.name);
            const searchText = vm.replaceForAutoComplete(queryText);

            return text.indexOf(searchText) > -1;
        },

        replaceForAutoComplete(text) {
            return text
                .replace(/İ/g, 'i')
                .replace(/I/g, 'ı')
                .toLowerCase();
        },

      cancel() {
        const vm = this;

        vm.clear();
        vm.showModal = false;
      },

      clear () {
          this.$refs.form.reset()
      },

      save() {
        const vm = this;

        if (!vm.$refs.form.validate()) {
          return;
        }

            vm.snackbarVisible = false;

            //Edit equipment
            if (vm.editIndex > -1) {

                //We are accessing updateEquipment in vuex store
                vm.$store
                    .dispatch('updateEquipment', {
                        id: vm.editAction.id,
                        name: vm.editAction.name,
                        code: vm.editAction.code,
                        description: vm.editAction.description,
                        isPortable: vm.editAction.isPortable,
                        equipmentTypeId: vm.selectEquipmentType,
                        operatingRoomIds: vm.editAction.operatingRoomId
                    })
                    .then(() => {
                        vm.snackbarVisible = true;
                        vm.$parent.getEquipments();

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }

            //Add equipment
            else {
                //We are accessing insertEquipment in vuex store
                vm.$store
                    .dispatch('insertEquipment', {
                        name: vm.editAction.name,
                        code: vm.editAction.code,
                        description: vm.editAction.description,
                        isPortable: vm.editAction.isPortable,
                        equipmentTypeId: vm.selectEquipmentType,
                        operatingRoomIds: vm.editAction.operatingRoomId
                    })
                    .then(() => {
                        vm.snackbarVisible = true;
                        vm.$parent.getEquipments();

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }

            vm.showModal = false;
            vm.clear();
        }
    }
};
</script>
