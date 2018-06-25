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
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['name']"
                              :label="$t('equipments.name')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md4>
                <v-select v-model="actions['type']"
                          :label="$t('equipments.type')">
                </v-select>
              </v-flex>

              <v-flex xs12 sm6 md4>
                <v-checkbox v-model="actions['portable']"
                              :label="$t('equipments.portable')"
                              color="primary">
                </v-checkbox>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-text-field v-model="actions['description']"
                              :label="$t('equipments.description')">
                </v-text-field>
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
    return {};
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.edit === -1 ? vm.$i18n.t("equipments.addEquipmentsInformation") : vm.$i18n.t("equipments.editEquipmentsInformation");
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
    }
  },

  methods: {

    save() {
      const vm = this;
      
      // if (vm.edit > -1) {
      //   Object.assign(vm.items[vm.edit], vm.actions);
      // }

      //Güncelleme işlemi

      vm.$store.dispatch('updateEquipment',{
        id : vm.actions.id,
        name: vm.actions.name,
        type: vm.actions.type,
        portable: vm.actions.portable,
        description: vm.actions.description
      });
       //vm.cancel();
    }
  },

  created() {
    const vm = this;

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
