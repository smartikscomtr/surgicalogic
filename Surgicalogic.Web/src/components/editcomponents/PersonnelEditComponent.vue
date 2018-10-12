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

        <v-card-text>
          <v-layout wrap edit-layout>
            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['firstName']" :label="$t('personnel.firstName')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['lastName']" :label="$t('personnel.lastName')">
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

            <v-flex xs12 sm6 md6>
              <v-text-field v-model="editAction['personnelCode']" :label="$t('personnel.personnelCode')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm6 md6>
					 <img :src="imageUrl" height="50" v-if="imageUrl"/>
					<v-text-field label="Select Image" @click='pickFile' v-model='imageName' prepend-icon='attach_file'></v-text-field>
					<input
						type="file"
						style="display: none"
						ref="image"
						accept="image/*"
						@change="onFilePicked"
					>
				</v-flex>

            <v-flex xs12 sm12 md12>
              <v-autocomplete v-model="selectBranch" :items="branches" :label="$t('branches.branch')" multiple chips deletable-chips :filter="customFilter" item-text="name" item-value="id">
              </v-autocomplete>
            </v-flex>

            <v-flex xs12 sm6 md6>
              <v-autocomplete v-model="selectWorkType" :items="workTypes" :label="$t('worktypes.workType')" :filter="customFilter" item-text="name" item-value="id">
              </v-autocomplete>
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
            imageName: '',
            imageUrl: '',
            imageFile: ''
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

                vm.editAction.branchId = val;
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

        cancel() {
            const vm = this;

            vm.showModal = false;
        },

        save() {
            const vm = this;

            vm.snackbarVisible = false;
            //Edit personnel
            if (vm.editIndex > -1) {
                //We are accessing updatePersonnel in vuex store
                vm.$store
                    .dispatch('updatePersonnel', {
                        id: vm.editAction.id,
                        personnelCode: vm.editAction.personnelCode,
                        firstName: vm.editAction.firstName,
                        lastName: vm.editAction.lastName,
                        personnelCategoryId: vm.editAction.personnelCategoryId,
                        personnelTitleId: vm.editAction.personnelTitleId,
                        branches: vm.editAction.branchId,
                        workTypeId: vm.editAction.workTypeId,
                        personnelPhoto: vm.imageUrl
                    })
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
                vm.$store
                    .dispatch('insertPersonnel', {
                        personnelCode: vm.editAction.personnelCode,
                        firstName: vm.editAction.firstName,
                        lastName: vm.editAction.lastName,
                        personnelCategoryId: vm.editAction.personnelCategoryId,
                        personnelTitleId: vm.editAction.personnelTitleId,
                        branches: vm.editAction.branchId,
                        workTypeId: vm.editAction.workTypeId,
                        personnelPhoto: vm.imageUrl
                    })
                    .then(() => {
                        vm.snackbarVisible = true;
                        vm.$store.dispatch('getPersonnels');

                        setTimeout(() => {
                            vm.snackbarVisible = false;
                        }, 2300);
                    });
            }

            vm.showModal = false;
        },

         pickFile () {
            this.$refs.image.click ()
        },

        onFilePicked (e) {
          const files = e.target.files
          if(files[0] !== undefined) {
            debugger;
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
