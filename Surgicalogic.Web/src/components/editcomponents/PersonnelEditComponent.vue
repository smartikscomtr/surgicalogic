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
              <v-text-field v-model="editAction['firstName']"
                            :rules="required"
                            :label="$t('personnel.firstName')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['lastName']"
                            :rules="required"
                            :label="$t('personnel.lastName')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-autocomplete v-model="selectPersonnelTitle" :items="PersonnelTitles" :label="$t('personnel.personnelTitle')" :filter="customFilter" item-text="name" item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-autocomplete v-model="selectPersonnelCategory" :items="PersonnelCategories" :label="$t('personnelcategory.personnelCategory')" :filter="customFilter" item-text="name" item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm12 md12>
              <v-autocomplete v-model="selectBranch" :items="branches" :label="$t('branches.branch')" multiple chips deletable-chips :filter="customFilter" item-text="name" item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['personnelCode']"
                            :rules="required"
                            :label="$t('personnel.personnelCode')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
                <v-text-field :label="$t('personnel.personnelPhoto')" @click='pickFile' v-model='imageName' prepend-icon='attach_file'></v-text-field>
                <input
                  type="file"
                  style="display: none"
                  id="file"
                  ref="file"
                  accept="image/*"
                  @change="onFilePicked"
                >
				    </v-flex>

            <v-flex xs12 sm6 md6>
              <v-autocomplete v-model="selectWorkType" :rules="required" :items="workTypes" :label="$t('worktypes.workType')" :filter="customFilter" item-text="name" item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <img :src="imageUrl" height="150" v-if="imageUrl"/>
              <img :src="editAction['pictureUrl']" height="150px" v-if="!imageUrl" />
            </v-flex>
          </v-layout>
        </v-card-text>

        <v-card-text>
          <div class="margin-bottom-none btn-wrap">
            <v-btn class="btnSave orange" @click.native="save">
              Kaydet
            </v-btn>
          </div>
        </v-card-text>
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
            savedMessage: this.$i18n.t('personnel.personnelSaved'),
            file: '',
            imageName: '',
            imageUrl: '',
            imageFile: '',
            valid: true,
            required: [
              v => !!v || this.$i18n.t('common.required')
            ],
            multipleRequired: [
              v => v.length > 0 || this.$i18n.t('common.required')
            ]
        };
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.editIndex === -1
                ? vm.$t('personnel.addPersonnelInformation')
                : vm.$t('personnel.editPersonnelInformation');
        },

        showModal: {
            get() {
                const vm = this;

                return vm.editVisible;
            },

            set(value) {
                const vm = this;

                //When the cancel button is clicked, the event is sent to the personnel edit component
                if (!value) {
                    vm.$emit('cancel');
                }
            }
        },

        PersonnelCategories() {
            const vm = this;
            return vm.$store.state.personnelModule.allPersonnelCategories;
        },

        PersonnelTitles() {
            const vm = this;
            return vm.$store.state.personnelModule.personnelTitles;
        },

        selectPersonnelCategory: {
            get() {
                const vm = this;

                return vm.editAction.personnelCategoryId;
            },

            set(val) {
                const vm = this;

                vm.editAction.personnelCategoryId = val;
            }
        },

        selectPersonnelTitle: {
            get() {
                const vm = this;

                return vm.editAction.personnelTitleId;
            },

            set(val) {
                const vm = this;

                vm.editAction.personnelTitleId = val;
            }
        },

        branches() {
            const vm = this;

            return vm.$store.state.personnelModule.allBranches;
        },

        selectBranch: {
            get() {
                const vm = this;

                return vm.editAction.branchIds;
            },

            set(val) {
                const vm = this;

                vm.editAction.branchIds = val;
            }
        },

        workTypes() {
            const vm = this;

            return vm.$store.state.personnelModule.allWorkTypes;
        },

        selectWorkType: {
            get() {
                const vm = this;

                return vm.editAction.workTypeId;
            },

            set(val) {
                const vm = this;

                vm.editAction.workTypeId = val;
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

        handleFileUpload(){

        },

          cancel() {
            const vm = this;

            vm.clear();
            vm.showModal = false;
          },

          clear () {
              this.$refs.form.reset()
              this.imageUrl = '';
              const input = this.$refs.file;
              input.type = 'text';
              input.type = 'file';
          },

          save() {
            const vm = this;

            if (!vm.$refs.form.validate()) {
              return;
            }

            vm.snackbarVisible = false;
            //Edit personnel
            if (vm.editIndex > -1) {
                //We are accessing updatePersonnel in vuex store
                let formData = new FormData();
                formData.append('file', this.file);
                formData.append('id', vm.editAction.id);
                formData.append('personnelCode',vm.editAction.personnelCode);
                formData.append('firstName',  vm.editAction.firstName);
                formData.append('pictureUrl',  vm.editAction.pictureUrl);
                formData.append('lastName',  vm.editAction.lastName);
                formData.append('personnelCategoryId',  vm.editAction.personnelCategoryId);
                formData.append('personnelTitleId',  vm.editAction.personnelTitleId);
                formData.append('branches',  vm.editAction.branchIds);
                formData.append('workTypeId',  vm.editAction.workTypeId);

                vm.$store
                    .dispatch('updatePersonnel', formData)
                    .then(() => {
                        vm.snackbarVisible = true;
                        vm.$store.dispatch('getPersonnels');

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }
            //Add personnel
            else {
                //We are accessing insertPersonnel in vuex store
                let formData = new FormData();
                formData.append('file', this.file);
                formData.append('personnelCode',vm.editAction.personnelCode);
                formData.append('firstName',  vm.editAction.firstName);
                formData.append('lastName',  vm.editAction.lastName);
                formData.append('personnelCategoryId',  vm.editAction.personnelCategoryId);
                formData.append('personnelTitleId',  vm.editAction.personnelTitleId);
                formData.append('branches',  vm.editAction.branchIds);
                formData.append('workTypeId',  vm.editAction.workTypeId);

                vm.$store
                    .dispatch('insertPersonnel', formData)
                    .then(() => {
                        vm.snackbarVisible = true;
                        vm.$store.dispatch('getPersonnels');

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }

            vm.showModal = false;
            vm.clear();
        },

         pickFile () {
            this.$refs.file.click ()
        },

        onFilePicked (e) {
          const files = e.target.files
          if(files[0] !== undefined) {
            this.file = this.$refs.file.files[0];
            this.imageName = files[0].name
            if(this.imageName.lastIndexOf('.') <= 0) {
              return
            }
            const fr = new FileReader ()
            fr.readAsDataURL(files[0])
            fr.addEventListener('load', () => {
              this.imageUrl = fr.result
              this.imageFile = files[0] // this is an image file that can be sent to server...
            })
          } else {
            this.imageName = ''
            this.imageFile = ''
            this.imageUrl = ''
          }
        }
    }
};
</script>
