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
                <v-text-field v-model="actions['room']" :label="$t('rooms.room')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md12>
                <v-text-field v-model="actions['location']" :label="$t('rooms.location')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md12>
                <v-text-field v-model="actions['size']" :label="$t('rooms.size')"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md12>

              <v-select v-model="selectEquipments"
                        :items="equipments"
                        :label="$t('equipments.equipments')"
                        item-text="name"
                        item-value="id"
                        multiple
                        chips>
              </v-select>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn color="blue darken-1" flat @click.native="cancel">Cancel</v-btn>
          <v-btn color="blue darken-1" flat @click.native="save">Save</v-btn>
        </v-card-actions>
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

      return vm.edit === -1 ? vm.$t('rooms.addRoomInformation') : vm.$t('rooms.editRoomInformation');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.visible;
      },
      set (value) {
        const vm = this;

        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    equipments() {
      const vm = this;

      return vm.$store.state.equipmentModule.equipments;
    },

    selectEquipments() {
      const vm = this;

      const items = vm.actions['equipments'];

      _each(items, (item) => {
          item.name = vm.$store.state.equipmentModule.equipments.find(d => d.id === item.id);
      });

      return items;
    }
  },

  methods: {

    cancel() {
      const vm = this;

      return vm.visible;
    },

    save() {
      const vm = this;

      if (vm.edit > -1) {
        Object.assign(vm.items[vm.edit], vm.actions);
      }

      //Güncelleme işlemi

      // vm.$store.dispatch('updatePersonnel', {
      //   id: vm.actions.id,
      //   personnelCode: vm.actions.personnelCode,
      //   givenName: vm.actions.givenName,
      //   familyName: vm.actions.familyName,
      //   tasks: vm.actions.tasks,
      //   branch: vm.actions.branch,
      //   shift: vm.actions.shift,
      //   workType: vm.actions.workType
      // });

      vm.cancel();
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch("getEquipments");

    vm.$watch('deleteValue', (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm(vm.$i18n.t('common.areYouSureWantToDelete'));

        vm.visible = false;
        //Silme İşlemi
      }
    });
  }
}

</script>
