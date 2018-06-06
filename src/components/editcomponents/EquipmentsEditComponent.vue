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

    columns: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },

    actions: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    }
  },

  data() {
    return {
      defaultItem: {
        name: '',
        type: '',
        portable: '',
        description: ''
      }
    };
  },

  computed: {
    showModal: {
      get() {
        const vm = this;

        return vm.visible;
      },
      set (value) {
        const vm = this;

        if (!value) {
          vm.$emit('close')
        }
      }
    },

    formTitle() {
      const vm = this;

      return vm.editedIndex === -1 ? "Yeni Ekipman Bilgisi Ekle" : "Ekipman Bilgileri Düzenleme";
    }
  },

  methods: {
    cancel() {
      this.dialog = false;

      // setTimeout(() => {
      //   this.actions = Object.assign({}, this.defaultItem);
      //   this.editedIndex = -1;
      // }, 300);
    },

    save() {
      const vm = this;

      if (this.editedIndex > -1) {
        // Object.assign(this.items[this.editedIndex], this.actions);
      }

      vm.$store.dispatch('updateEquipments', {
        id: vm.actions.id,
        name: vm.actions.name,
        type: vm.actions.type,
        portable: vm.actions.portable,
        description: vm.actions.description
      });
      // editedName: this.actions.name,
      // editedType: this.actions.type,
      // editedPortable: this.actions.portable,
      // editedDescription: this.actions.description
        // this.items.push(this.actions);
      vm.cancel();
    }
  }
}

</script>
