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
            <v-flex xs12 sm12 md12>
              <v-text-field v-model="editAction['name']"  :rules="required" :label="$t('worktypes.workType')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm12 md12>
              <v-textarea v-model="editAction['description']" rows="3" :label="$t('common.description')">
              </v-textarea>
            </v-flex>
          </v-layout>
        </v-card-text>

        <v-card-actions class="justify-end flex">
              <v-btn class="orangeButton" @click.native="save">
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
            savedMessage: this.$i18n.t('worktypes.workTypeSaved'),
            valid: true,
            required: [
              v => !!v || this.$i18n.t('common.required')
            ],
            multipleRequired: [
              v => !!v && v.length > 0 || this.$i18n.t('common.required')
            ]
        };
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.editIndex === -1
                ? vm.$i18n.t('worktypes.addWorkTypesInformation')
                : vm.$i18n.t('worktypes.editWorkTypesInformation');
        },

        showModal: {
            get() {
                const vm = this;

                return vm.editVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the work types edit component
                if (!value) {
                    vm.$emit('cancel');
                }
            }
        }
    },

    methods: {
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

            //Edit work type
            if (vm.editIndex > -1) {
                //We are accessing updateWorkType in vuex store
                vm.$store
                    .dispatch('updateWorkType', {
                        id: vm.editAction.id,
                        name: vm.editAction.name,
                        description: vm.editAction.description
                    })
                    .then(() => {
                        vm.snackbarVisible = true;

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }
            //Add work type
            else {
                //We are accessing insertWorkType in vuex store
                vm.$store
                    .dispatch('insertWorkType', {
                        name: vm.editAction.name,
                        description: vm.editAction.description
                    })
                    .then(() => {
                        vm.snackbarVisible = true;

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }

            vm.showModal = false;
            vm.clear();
        }
    }
};
</script>
