<template>
  <div class="container fluid grid-list-md clinic-page">
    <v-layout wrap edit-layout>
      <v-flex lg4 md3 sm6 xs12>
        <v-autocomplete v-model="selectBranch" :items="branches" :label="$t('branches.branch')" box :filter="customFilter" @change="filterBranch()" item-text="name" item-value="id">
        </v-autocomplete>
      </v-flex>

      <v-flex lg4 md3 sm6 xs12>
        <v-autocomplete v-model="selectDoctor" :items="doctors" :label="$t('personnel.doctor')" box :filter="customFilter" @change="filterDoctor()" item-text="personnelTitleName" item-value="id">
        </v-autocomplete>
      </v-flex>
    </v-layout>

    <v-layout row wrap>
      <v-flex v-for="(doctorCard, i) in doctorCards" :key="i" lg2 md3 sm6 xs12>
        <v-card>
          <img :src="doctorCard.pictureUrl" height="150px" />
          <span class="doctorName-wrap" v-text="doctorCard.personnelTitleName">
          </span>
          <span class="branchName-wrap" v-text="doctorCard.branchNames">
          </span>
          <v-btn @click="$router.push('/appointmentcalendarpage')"> Randevu Al
          </v-btn>
        </v-card>
      </v-flex>
    </v-layout>
  </div>
</template>

<script>
export default {
    data() {
        const vm = this;

        return {
            branchId: null,
            doctorId: null,
            filteredDoctors: [],
            doctorCards: []
        };
    },

    computed: {
        branches() {
            const vm = this;

            return vm.$store.state.branchesModule.allBranches;
        },

        selectBranch: {
            get() {
                const vm = this;

                return vm.branchId;
            },

            set(val) {
                const vm = this;

                vm.branchId = val;
            }
        },

        doctors: {
            get() {
                const vm = this;

                return vm.filteredDoctors;
            },

            set(val) {
                const vm = this;

                vm.filteredDoctors = val;
            }
        },

        selectDoctor: {
            get() {
                const vm = this;

                return vm.doctorId;
            },

            set(val) {
                const vm = this;

                vm.doctorId = val;
            }
        }
    },

    methods: {
        getImage() {
            const min = 550;
            const max = 560;

            return Math.floor(Math.random() * (max - min + 1)) + min;
        },

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

        filterBranch() {
            const vm = this;

            vm.$store
                .dispatch('getDoctorsByBranchIdAsync', {
                    branchId: vm.branchId
                })
                .then(() => {
                    vm.filteredDoctors =
                        vm.$store.state.personnelModule.filteredDoctor;
                    vm.doctorCards = vm.filteredDoctors;
                });
        },

        filterDoctor() {
            const vm = this;
            var doctor = [];
            vm.doctorCards = [];
            for (let index = 0; index < vm.filteredDoctors.length; index++) {
                const element = vm.filteredDoctors[index];

                if (element.id == vm.doctorId) {
                    vm.doctorCards.push(vm.filteredDoctors[index]);
                }
            }
        }
    },

    created() {
        const vm = this;

        vm.$store
            .dispatch('getDoctorsByBranchIdAsync', {
                branchId: 0
            })
            .then(() => {
                vm.filteredDoctors =
                    vm.$store.state.personnelModule.filteredDoctor;
                vm.doctorCards = vm.filteredDoctors;
            });

        vm.$store.dispatch('getAllBranches');
    }
};
</script>

<style>
.clinic-page > div {
      padding: 0 44px;
}
.clinic-page .wrap > div .v-card {
    height: 100%;
    text-align: center;
}
.clinic-page .wrap > div .v-card img {
    margin: 0 auto;
    display: block;
}
.clinic-page .wrap > div .v-card span {
    display: block;
    margin-bottom: 10px;
}
.clinic-page .wrap > div .v-card img + span {
    margin-top: 20px;
}
.clinic-page .wrap > div .v-btn {
    margin: 10px;
    background-color: #ff7107 !important;
}
.clinic-page .wrap > div .v-btn {
    color: #fff;
}
.clinic-page .v-input__slot {
    background-color: #fff !important;
    color: #ff7107 !important;
}
.clinic-page .v-input__slot:before {
    border-color: rgba(0, 0, 0, 0.42) !important;
}
.clinic-page .v-menu__activator label.primary--text,
.clinic-page .v-menu__activator .v-input__icon i.primary--text {
    color: rgba(0, 0, 0, 0.54) !important;
}
.clinic-page .v-menu__activator--active .v-select__slot label.primary--text,
.clinic-page .v-menu__activator--active .v-input__icon i {
    color: #ff7107 !important;
}
.v-list .primary--text {
    color: inherit !important;
}
</style>
