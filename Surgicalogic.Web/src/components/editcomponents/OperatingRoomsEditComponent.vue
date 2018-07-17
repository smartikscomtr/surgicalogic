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
                <v-text-field v-model="editAction['length']"
                              :label="$t('operatingrooms.length')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['description']"
                              :label="$t('common.description')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-select multiple v-model="selectEquipment"
                          :items="equipments"
                          :label="$t('equipments.equipments')"
                          item-text="name"
                          item-value="id">
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
    selectEquipment: {
      get() {
        const vm = this;

        return vm.editAction.equipmentTypeId;
      },

      set(val) {
        const vm = this;

        vm.editAction.equipmentName = vm.$store.state.operatingRoomModule.nonPortableEquipments.find(
          item => {
            if (item.id == val) {
              return item;
            }
          }
        ).name;

        vm.editAction.equipmentId = val;
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
          length: vm.editAction.length
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
          length: vm.editAction.length
        });
      }

      vm.showModal = false;
    }
  }
}

</script>
