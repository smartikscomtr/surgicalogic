<template>
  <div class="container fluid grid-list-md clinic-page" id="section-to-print">
    <v-layout wrap edit-layout>
      <v-flex lg4 md3 sm6 xs12>
        <v-autocomplete v-model="selectBranch" :items="branches" :label="$t('branches.branch')" box
                        clearable :filter="customFilter" @change="filterBranch()" item-text="name" item-value="id">
        </v-autocomplete>
      </v-flex>

      <v-flex lg4 md3 sm6 xs12>
        <v-autocomplete v-model="selectDoctor" :items="doctors" :label="$t('personnel.doctor')" box
                        clearable :filter="customFilter" @change="filterDoctor()" item-text="personnelTitleName" item-value="id">
        </v-autocomplete>
      </v-flex>
    </v-layout>

    <v-layout row wrap class="column-cards">
      <v-flex v-for="(doctorCard, i) in doctorCards" :key="i" lg3 md3 sm6 xs12>
        <v-card class="clinic-branch">
          <img :src="doctorCard.pictureUrl" height="150px" />

          <span class="doctorName-wrap" v-text="doctorCard.personnelTitleName">
          </span>

          <span class="branchName-wrap" v-text="doctorCard.branchNames">
          </span>

          <v-btn class="orange"
          @click="routePageGetAppointment(doctorCard.id)"> {{ $t('appointmentcalendar.setAppointment') }}
          </v-btn>

          <v-btn class="orange"
           @click="routePageAppointmentList(doctorCard.id)"> {{ $t('appointmentcalendar.appointmentList') }}
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
                  branchId: vm.branchId ? vm.branchId : 0
              })
              .then(() => {
                  vm.filteredDoctors =
                      vm.$store.state.personnelModule.filteredDoctor;
                  vm.doctorCards = vm.filteredDoctors;

              if (vm.doctorId)
              {
                vm.filterDoctor();
              }

              });
        },

        filterDoctor() {
            const vm = this;

            var doctor = [];
            vm.doctorCards = [];
            for (let index = 0; index < vm.filteredDoctors.length; index++) {
                const element = vm.filteredDoctors[index];

                if (!vm.doctorId || element.id == vm.doctorId) {
                    vm.doctorCards.push(vm.filteredDoctors[index]);
                }
            }
        },

        routePageGetAppointment(clickDoctorId) {
            const vm = this;

            return vm.$router.push(
                '/appointmentcalendarpage?doctorId=' + clickDoctorId
            );
        },

        routePageAppointmentList(clickDoctorId) {
            const vm = this;

            return vm.$router.push(
                '/appointmentlistpage?doctorId=' + clickDoctorId
            );
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
@media print {
    body * {
        visibility: hidden;
    }
    #section-to-print,
    #section-to-print * {
        visibility: visible;
    }
    .v-content {
        position: relative;
    }
    #section-to-print {
        position: absolute;
        left: 0;
        top: 0;
    }
    .navigation.v-navigation-drawer {
        display: none !important;
    }
    main.v-content {
        padding: 0 !important;
    }
}
.clinic-page .column-cards > div {
    margin-bottom: 10px;
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
    font-weight: 500;
    height: 40px;
    overflow: hidden;
}
span.branchName-wrap {
    height: 85px;
    overflow: hidden;
}
.clinic-branch {
    padding: 0 10px;
}
</style>
