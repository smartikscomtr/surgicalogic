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

            <v-icon @click="cancel"
                    class="close-wrap">
              close
            </v-icon>
          </div>
        </v-card-title>

        <v-card-text>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="editAction['name']"
                              :label="$t('operation.operationName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-autocomplete v-model="selectOperationType"
                                :items="operationTypes"
                                :label="$t('operation.operationType')"
                                :filter="customFilter"
                                item-text="name"
                                item-value="id">
                </v-autocomplete>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="selectOperationTime"
                              :label="$t('operation.operationTime')"
                              :value="editAction['operationTime']"
                              type="time">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-menu
                  ref="menu"
                  lazy
                  :close-on-content-click="false"
                  v-model="menu"
                  transition="scale-transition"
                  offset-y
                  full-width
                  :nudge-right="40"
                  min-width="290px"
                  :return-value.sync="date"
                >
                  <v-text-field
                    slot="activator"
                    label="Picker in menu"
                    v-model="editAction['date']"
                    prepend-icon="event"
                    readonly
                  ></v-text-field>
                  <v-date-picker v-model="date" no-title scrollable>
                    <v-spacer></v-spacer>
                    <v-btn flat color="primary" @click="menu = false">Cancel</v-btn>
                    <v-btn flat color="primary" @click="$refs.menu.save(date)">OK</v-btn>
                  </v-date-picker>
                </v-menu>
              </v-flex>

              <v-flex xs12 sm6 md12>
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

    <snackbar-component :snackbar-visible="snackbarVisible"
                        :savedMessage="savedMessage">
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
      savedMessage: this.$i18n.t('operation.OperationSaved'),
      date: this.editAction.date,
      menu: false,
      modal: false,
      datestring: this.editAction.date
    };
  },

  watch: {
    showModal (val) {
      val || this.cancel()
    },

    date:function(val, oldVal) {
      debugger;
			this.datestring = this.gettanggal(val);
	}
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1 ? vm.$i18n.t('operation.addOperationInformation') : vm.$i18n.t('operation.editOperationInformation');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the operations edit component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    operationTypes() {
      const vm = this;

      return vm.$store.state.operationModule.allOperationTypes;
    },

    selectOperationType: {
      get() {
        const vm = this;

        return vm.editAction.operationTypeId;
      },

      set(val) {
        const vm = this;

        vm.editAction.operationTypeName = vm.$store.state.operationModule.allOperationTypes.find(
          item => {
            if (item.id == val) {
              return item;
            }
          }
        ).name;

        vm.editAction.operationTypeId = val;
      }
    },

    selectOperationTime: {
      get() {
        const vm = this;

        return vm.editAction.operationTime;
      },

      set(val) {
        const vm = this;

        vm.editAction.operationTime = val;
      }
    }
  },

  methods: {
     customFilter (item, queryText, itemText) {
      const vm = this;

      const text = vm.replaceForAutoComplete(item.name);
      const searchText = vm.replaceForAutoComplete(queryText);

      return text.indexOf(searchText) > -1;
    },

    replaceForAutoComplete(text)
    {
      return text.replace(/İ/g, 'i').replace(/I/g, 'ı').toLowerCase();
    },

    cancel() {
      const vm = this;

      vm.showModal = false;
    },

    save() {
      const vm = this;

      vm.snackbarVisible = false;

      //Edit operation
      if (vm.editIndex > -1) {
        //We are accessing updateOperation in vuex store
        vm.$store.dispatch('updateOperation', {
          id: vm.editAction.id,
          name: vm.editAction.name,
          date: vm.editAction.date,
          operationTime: vm.editAction.operationTime,
          description: vm.editAction.description,
          operationTypeId: vm.selectOperationType
        }).then(() => {
          setTimeout(() => {
            vm.snackbarVisible = true;
          }, 200)

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300)
        })
      }

      //Add operation
      else {
        //We are accessing insertOperation in vuex store
        vm.$store.dispatch('insertOperation', {
          name: vm.editAction.name,
          date: vm.editAction.date,
          operationTime: vm.editAction.operationTime,
          description: vm.editAction.description,
          operationTypeId: vm.selectOperationType
        }).then(() => {
          setTimeout(() => {
            vm.snackbarVisible = true;
          }, 200)

          setTimeout(() => {
            vm.snackbarVisible = false;
          }, 2300)
        })
      }

      vm.showModal = false;
    },

     gettanggal(str) {
      if (str != null) {
        return str.substring(8, 10)+'/'+str.substring(5, 7)+'/'+str.substring(0, 4);
      }
      return '';
    }
  },
};

</script>
