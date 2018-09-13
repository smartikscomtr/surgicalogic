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
    </v-card>

    <v-container
                fluid
                grid-list-md
                grey
                lighten-4>

      <v-layout row wrap>
        <v-spacer></v-spacer>

        <v-flex v-for="(doctorCard, i) in doctorCards"
                :key="i"
                xs12
                sm6
                md4>
          <v-card>
            <img :src="`https://picsum.photos/200/300?image=${getImage()}`"
                   height="300px">

              <span class="headline black--text pl-3 pt-3"
                    v-text="doctorCard.fullName">
              </span>
            </img>

            <!-- <v-card-actions class="white justify-center">
            <v-input> {{ doctorCard.fullName }} </v-input>
            </v-card-actions> -->

          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
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
      },
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
