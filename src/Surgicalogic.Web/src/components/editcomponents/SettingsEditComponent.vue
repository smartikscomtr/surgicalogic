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
              <v-text-field v-model="editAction['name']" :rules="required"  :label="$t('settings.name')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6 v-show="0">
              <v-autocomplete v-model="selectSettingDataType" :items="settingDataTypes" :label="$t('settings.settingDataType')" :filter="customFilter"
                              clearable @change="settingDataTypeChanged()"  item-text="name" item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6 v-show="showInt">
              <v-text-field v-model="editAction['intValue']"  :rules="requiredInt" :label="$t('settings.value')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6 v-show="showString">
              <v-text-field v-model="editAction['stringValue']"  :rules="requiredString" :label="$t('settings.value')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6 v-show="showTime">
              <v-text-field v-model="editAction['timeValue']"  :rules="requiredTime" :label="$t('settings.value')" :value="editAction['timeValue']" type="time">
              </v-text-field>
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
            savedMessage: this.$i18n.t('settings.settingUpdated'),
            showInt: false,
            showString: false,
            showTime: false,
            valid: true,
            required: [
              v => !!v || this.$i18n.t('common.required')
            ],
            requiredInt: [
              v => (!!v || !this.showInt) || this.$i18n.t('common.required')
            ],
            requiredString: [
              v => (!!v || !this.showString) || this.$i18n.t('common.required')
            ],
            requiredTime: [
              v => (!!v  || !this.showTime) || this.$i18n.t('common.required')
            ]
        };
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.$i18n.t('settings.editSettings');
        },

        showModal: {
            get() {
                const vm = this;

                return vm.editVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the users edit component
                if (!value) {
                    vm.$emit('cancel');
                }
            }
        },

        settingDataTypes() {
            const vm = this;

            return vm.$store.state.settingsModule.settingDataTypes;
        },

        selectSettingDataType: {
            get() {
                const vm = this;

                return vm.editAction.settingDataTypeId;
            },

            set(val) {
                const vm = this;

                vm.editAction.settingDataTypeId = val;
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

            vm.$store
                .dispatch('updateSetting', {
                    id: vm.editAction.id,
                    name: vm.editAction.name,
                    settingDataTypeId: vm.editAction.settingDataTypeId,
                    intValue: vm.editAction.intValue,
                    stringValue: vm.editAction.stringValue,
                    timeValue: vm.editAction.timeValue
                })
                .then(() => {
                    vm.snackbarVisible = true;
                    vm.$parent.getSettings();
                    setTimeout(() => {
                        vm.snackbarVisible = false;
                    }, 2300);
                });

            vm.showModal = false;
            vm.clear();
        },

        settingDataTypeChanged() {
            const vm = this;

            switch (vm.editAction.settingDataTypeId) {
                case 1:
                    (vm.showInt = false),
                        (vm.showString = true),
                        (vm.showTime = false);
                    break;
                case 2:
                    (vm.showInt = true),
                        (vm.showString = false),
                        (vm.showTime = false);
                    break;
                case 3:
                    (vm.showInt = false),
                        (vm.showString = false),
                        (vm.showTime = true);
                    break;
                default:
                    break;
            }
        }
    }
};
</script>
