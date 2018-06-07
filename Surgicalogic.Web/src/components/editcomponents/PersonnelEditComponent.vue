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
                <v-text-field v-model="actions['personnelCode']" label="Personel Kodu"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['givenName']" label="Adı"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['familyName']" label="Soyadı"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['tasks']" label="Görevi"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['branch']" label="Branşı"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['shift']" label="Vardiya"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="actions['workType']" label="Çalışma Tipi"></v-text-field>
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
    },

    deleteValue: {
      type: Object,
      required: false
    }
  },

  data() {
    return {
      editedIndex: -1,
      defaultItem: {
        personnelCode: 0,
        givenName: '',
        familyName: '',
        tasks: '',
        branch: '',
        shift: '',
        workType: ''
      }
    };
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editedIndex === -1 ? "Yeni Personel Bilgisi Ekle" : "Personel Bilgisi Düzenle";
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

      if (vm.editedIndex > -1) {
        Object.assign(vm.items[vm.editedIndex], vm.actions);
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

    vm.$watch('deleteValue', (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm('Silmek istediğinizden emin misiniz ?');

        //Silme İşlemi
      }
    });
  }
}

</script>
