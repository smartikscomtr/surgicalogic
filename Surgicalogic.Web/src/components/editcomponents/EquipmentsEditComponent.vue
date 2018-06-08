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
                <v-text-field v-model="actions['name']" label="Adı"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['type']" label="Tipi"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['portable']" label="Taşınabilirlik"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['description']" label="Açıklama"></v-text-field>
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

    title: {
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

      return vm.title === -1 ? "Yeni Ekipman Bilgisi Ekle" : "Ekipman Bilgileri Düzenleme";
    },

    showModal: {
      get() {
        const vm = this;

        return vm.visible;
      },
      set (value) {
        const vm = this;

        if (!value) {
          vm.$emit('visible')
        }
      }
    }
  },

  methods: {
    cancel() {
      const vm = this;

      vm.visible = false;
    },

    save() {
      const vm = this;

      if (vm.title > -1) {
        Object.assign(vm.items[vm.title], vm.actions);
      }

      //Güncelleme işlemi

      // vm.$store.dispatch('updateEquipments', {
      //   id: vm.actions.id,
      //   name: vm.actions.name,
      //   type: vm.actions.type,
      //   portable: vm.actions.portable,
      //   description: vm.actions.description
      // });

      vm.cancel();
    }
  },

  created() {
    const vm = this;

    vm.$watch('deleteValue', (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm("Silmek istediğinizden emin misiniz ?")

        vm.visible = false;
        //Silme İşlemi
      }
    });
  }
}

</script>
