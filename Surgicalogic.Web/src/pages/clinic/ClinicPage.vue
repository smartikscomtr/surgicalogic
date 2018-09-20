<template>
  <div>
    <v-card class="container fluid grid-list-md">
      <v-layout wrap edit-layout>
        <v-flex xs12 sm3 md3>
          <v-autocomplete v-model="selectBranch"
                          :items="branches"
                          :label="$t('branches.branch')"
                          box
                          :filter="customFilter"
                          @change="filterBranch()"
                          item-text="name"
                          item-value="id">
          </v-autocomplete>
        </v-flex>

        <v-flex xs12 sm3 md3>
          <v-autocomplete v-model="selectDoctor"
                          :items="doctors"
                          :label="$t('personnel.doctor')"
                          box
                          :filter="customFilter"
                           @change="filterDoctor()"
                          item-text="personnelTitleName"
                          item-value="id">
          </v-autocomplete>
        </v-flex>
      </v-layout>

    <v-container grid-list-md
                 grey
                 lighten-4>
      <v-layout row wrap>
        <v-flex v-for="(doctorCard, i) in doctorCards"
                :key="i"
                xs2 sm2 m2>
          <v-card>
            <img :src="doctorCard.pictureUrl"
                  height="300px"
                  width="250px">
              <span class="doctorName-wrap"
                    v-text="doctorCard.personnelTitleName">
              </span>

              <v-spacer></v-spacer>

              <span class="branchName-wrap"
                    v-text="doctorCard.branchNames">
              </span>
            </img>

            <v-spacer></v-spacer>

            <v-btn @click="$router.push('/appointmentcalendarpage')"> Randevu Al
            </v-btn>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
    </v-card>
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
    getImage () {
      const min = 550
      const max = 560

      return Math.floor(Math.random() * (max - min + 1)) + min;
    },

    customFilter (item, queryText, itemText) {
      const vm = this;

      const text = vm.replaceForAutoComplete(item.name);
      const searchText = vm.replaceForAutoComplete(queryText);

      return text.indexOf(searchText) > -1;
    },

    replaceForAutoComplete(text) {
      return text.replace(/İ/g, 'i').replace(/I/g, 'ı').toLowerCase();
    },

    filterBranch() {
      const vm = this;

      vm.$store.dispatch('getDoctorsByBranchIdAsync',
      {
        branchId: vm.branchId
      }).then(() => {
          setTimeout(() => {
            vm.filteredDoctors = vm.$store.state.personnelModule.filteredDoctor;
            vm.doctorCards = vm.filteredDoctors;
        }, 1000)
      });
    },

    filterDoctor() {
      const vm = this;
      var doctor = [];
      vm.doctorCards = [];
      for (let index = 0; index < vm.filteredDoctors.length; index++) {
        const element = vm.filteredDoctors[index];

        if (element.id == vm.doctorId){
            vm.doctorCards.push(vm.filteredDoctors[index]);
        }
      }
    }
  },

  created () {
    const vm = this;

    vm.$store.dispatch('getDoctorsByBranchIdAsync', {
      branchId: 0
    }).then(() => {
        setTimeout(() => {
          vm.filteredDoctors = vm.$store.state.personnelModule.filteredDoctor;
          vm.doctorCards = vm.filteredDoctors;
      }, 1000)
    });

    vm.$store.dispatch('getAllBranches');
  }
};

</script>

<style>
.doctorName-wrap {
  color: #cd5e15;
  font-family: serif;
  font-size: 18px;
  font-weight: 600;
  line-height: 20px;
  padding-left: 10px !important;
  padding-right: 10px !important;
}
.branchName-wrap {
  color: #cd5e15;
  font-family: serif;
  font-size: 10px;
  text-transform: uppercase;
  font-weight: 600;
  background-color: #fff;
  padding: 1.5px 3px;
  display: inline-block;
  min-width: 100px;
  width: auto;
  max-width: 180px;
  border-radius: 4px;
  -webkit-box-decoration-break: clone;
}
</style>
