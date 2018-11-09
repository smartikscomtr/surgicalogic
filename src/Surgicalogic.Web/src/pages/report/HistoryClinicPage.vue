<template>
  <div class="container fluid grid-list-md">
    <div class="grid-card v-card">
      <div class="v-card__text layout row wrap">
        <v-flex xs12 sm12 md12>
          <div class="btn-wrap">
            <v-btn class="btnSave orange"
                   lang="tr"
                   @click.native="filteredReport">
              {{ $t('report.filter') }}
            </v-btn>
          </div>
        </v-flex>

        <v-flex xs12 sm6 md6>
          <v-autocomplete v-model="selectBranch"
                          :items="branches"
                          :label="$t('report.branch')"
                          clearable
                          box
                          :filter="customFilter"
                          item-text="name"
                          item-value="id">
          </v-autocomplete>
        </v-flex>

        <v-flex xs12 sm6 md6>
          <v-autocomplete v-model="selectDoctor"
                          :items="doctors"
                          :label="$t('report.doctor')"
                          box
                          clearable
                          @change="filterDoctor()"
                          :filter="customFilterForDoctor"
                          item-text="personnelTitleName"
                          item-value="id">
          </v-autocomplete>
        </v-flex>

        <v-flex xs12 sm6 md6>
          <v-autocomplete v-model="selectPatient"
                          :items="patients"
                          :label="$t('report.patient')"
                          box
                          clearable
                          :filter="customFilterForPatient"
                          item-text="fullName"
                          item-value="id">
          </v-autocomplete>
        </v-flex>

        <v-flex xs12 sm6 md6>
          <v-menu ref="menu1"
                  :close-on-content-click="false"
                  v-model="menu1"
                  :nudge-right="40"
                  lazy
                  :return-value.sync="appointmentDate"
                  transition="scale-transition"
                  offset-y
                  full-width
                  min-width="290px">
            <v-text-field readonly slot="activator"
                          v-model="startDateFormatted"
                          clearable
                          :label="$t('plan.operationDate')">
            </v-text-field>

            <v-date-picker v-model="appointmentDate"
                           no-title
                           @input="$refs.menu1.save(appointmentDate)"
                           :max="getMaxDate()">
            </v-date-picker>
          </v-menu>
        </v-flex>
      </div>
    </div>

    <grid-component :headers="headers"
                    :items="historyClinics"
                    :title="title"
                    :show-detail="false"
                    :show-edit="false"
                    :show-delete="false"
                    :show-search="true"
                    :show-insert="false"
                    :hide-actions="false"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    :custom-parameters="customParameters"
                    @detail="detail"
                    @exportToExcel="exportHistoryClinicReportToExcel"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>
  </div>
</template>

<script>

import { gridMixin } from './../../mixins/gridMixin';

export default {
  mixins: [
    gridMixin
  ],

  data() {
    const vm = this;

    return {
      search: '',
      detailDialog: false,
      editDialog: false,
      deleteDialog: false,
      detailAction: {},
      editAction: {},
      deleteValue: {},
      editedIndex: -1,
      totalRowCount:0,
      editLoadOnce: true,
      deletePath: '',
      menu1: false,
      dateFormatted: null,
      startDateFormatted: null,
      customParameters: {},
      branchId: null,
      doctorId: null,
      patientId: null,
      appointmentDate: null,
      filteredDoctors: [],
      doctorCards: []
    };
  },

  watch: {
    appointmentDate(val) {
      const vm = this;

      vm.startDateFormatted = vm.formatDate(vm.appointmentDate);
    }
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('report.clinicHistoryReport');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'branchName',
          sortBy: 'Operation.Name',
          text: vm.$i18n.t('report.branch'),
          sortBy:'BranchName',
          sortable: true,
          align: 'left'
        },
        {
          value:'doctorName',
          text: vm.$i18n.t('report.doctor'),
          sortBy:'DoctorName',
          sortable: true,
          align: 'left'
        },
        {
          value:'patientName',
          text: vm.$i18n.t('report.patient'),
          sortBy:'PatientName',
          sortable: true,
          align: 'left'
        },
        {
          value: 'appointmentDate',
          sortBy:'AppointmentDate',
          text: vm.$i18n.t('report.appointmentDate'),
          isDateTime: true,
          sortable: true,
          align: 'left'
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    historyClinics() {
      const vm = this;

      return vm.$store.state.reportsModule.clinic;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.reportsModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.reportsModule.totalCount;
    },

    selectBranch: {
      get() {
        const vm = this;

        vm.branchId;
      },

      set(val) {
        const vm = this;

        vm.branchId = val;
      }
    },

    branches() {
      const vm = this;

      return vm.$store.state.branchesModule.allBranches;
    },

    selectDoctor: {
      get() {
        const vm = this;

        vm.doctorId;
      },

      set(val) {
        const vm = this;

        vm.doctorId = val;
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

    selectPatient: {
      get() {
        const vm = this;

        vm.patientId;
      },

      set(val) {
        const vm = this;

        vm.patientId = val;
      }
    },

    patients() {
      const vm = this;

      return vm.$store.state.patientModule.allPatients;
    }
  },

  methods: {
    getMethodName(){
      return "getClinicHistory";
    },

    deleteMethodName(){
      return "";
    },

    customFilter(item, queryText, itemText) {
      const vm = this;

      const text = vm.replaceForAutoComplete(item.name);
      const searchText = vm.replaceForAutoComplete(queryText);

      return text.indexOf(searchText) > -1;
    },

    customFilterForDoctor(item, queryText, itemText) {
      const vm = this;

      const text = vm.replaceForAutoComplete(item.personnelTitleName);
      const searchText = vm.replaceForAutoComplete(queryText);

      return text.indexOf(searchText) > -1;
    },

    customFilterForPatient(item, queryText, itemText) {
      const vm = this;

      const text = vm.replaceForAutoComplete(item.fullName);
      const searchText = vm.replaceForAutoComplete(queryText);

      return text.indexOf(searchText) > -1;
    },

    replaceForAutoComplete(text) {
      return text.replace(/İ/g, 'i')
                 .replace(/I/g, 'ı')
                 .toLowerCase();
    },

    filteredReport() {
      const vm = this;

      vm.customParameters.branchId = vm.branchId;
      vm.customParameters.doctorId = vm.doctorId;
      vm.customParameters.patientId = vm.patientId;
      vm.customParameters.appointmentDate = vm.appointmentDate;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    },

    getMaxDate() {
      const toTwoDigits = num => (num < 10 ? '0' + num : num);
      let today = new Date();

      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let day = toTwoDigits(today.getDate());

      return `${year}-${month}-${day}`;
    },

    formatDate(date) {
      if (!date || date.indexOf('.') > -1) return null;

      const [year, month, day] = date.split('-');

      return `${day}.${month}.${year}`;
    },

    exportHistoryClinicReportToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportHistoryClinic', {
        branchId: vm.branchId,
        doctorId: vm.doctorId,
        patientId: vm.patientId,
        appointmentDate: vm.appointmentDate
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

  created () {
    const vm = this;

    vm.$store.dispatch('getDoctorsByBranchIdAsync', {
      branchId: 0
    }).then(() => {
      vm.filteredDoctors = vm.$store.state.personnelModule.filteredDoctor;
      vm.doctorCards = vm.filteredDoctors;
    });

    vm.$store.dispatch('getAllBranches');
    vm.$store.dispatch('getAllPatients');
  }
};

</script>
