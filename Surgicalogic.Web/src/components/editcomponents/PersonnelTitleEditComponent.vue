<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
              {{ formTitle }}
            </span>
          </div>
        </v-card-title>

        <v-card-text>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['name']"
                              :label="$t('personneltitle.personnelTitle')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['description']"
                              :label="$t('common.description')">
                </v-text-field>
              </v-flex>
              <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right>
                <v-btn class="btnSave orange"
                       @click.native="save">
                  Kaydet
                </v-btn>
              </v-flex>
            </v-layout>
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
    },
    deleteValue: {
      type: Object,
      required: false
    }
  },

  data() {
    return {};
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1 ? vm.$i18n.t('personneltitle.addPersonnelTitlesInformation') : vm.$i18n.t('personneltitle.editPersonnelTitlesInformation');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editVisible;
      },

      set (value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the personnel title edit component
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

      //Edit personnel title
      if (vm.editIndex > -1) {
        //We are accessing updatePersonnelTitle in vuex store
        vm.$store.dispatch('updatePersonnelTitle', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          description: vm.editAction.description
        });
      }
      //Add personnel title
      else {
        //We are accessing insertPersonnelTitle in vuex store
        vm.$store.dispatch('insertPersonnelTitle', {
          name: vm.editAction.name,
          description: vm.editAction.description
        });
      }

      vm.showModal = false;
    }
  }
}

</script>
