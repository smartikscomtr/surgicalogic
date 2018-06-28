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
                              :label="$t('branchs.branch')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-text-field v-model="editAction['description']"
                              :label="$t('common.description')">
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

      return vm.editIndex === -1 ? vm.$i18n.t("branchs.addBranchsInformation") : vm.$i18n.t("branchs.editBranchsInformation");
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
        vm.$store.dispatch('updateBranch', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          description: vm.editAction.description
        });
      } else {
        vm.$store.dispatch('insertBranch', {
          name: vm.editAction.name,
          description: vm.editAction.description
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
        //Silme İşlemi
      }
    });
  }
}

</script>
