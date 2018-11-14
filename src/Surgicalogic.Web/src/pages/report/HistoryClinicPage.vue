<template>
  <div class="container fluid grid-list-md">
    <div class="grid-card v-card">
      <v-form ref="form" v-model="valid" lazy-validation>
        <div class="v-card__text layout row wrap">
          <v-flex xs12 sm12 md12>
            <div class="btn-wrap">
              <v-btn class="btnSave orange"
                    lang="tr"
                    @click.native="filteredReport">
                {{ $t('report.filter') }}
              </v-btn>
            </div>

            <div class="btn-wrap">
              <v-btn class="btnSave orange"
                    lang="tr"
                    @click.native="clearReport">
                {{ $t('report.clear') }}
              </v-btn>
            </div>
          </v-flex>

          <v-flex xs12 sm12 md12>
            <v-autocomplete v-model="selectBranch"
                            :items="branches"
                            :label="$t('report.branch')"
                            clearable
                            multiple
                            chips
                            deletable-chips
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
                            multiple
                            chips
                            deletable-chips
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
                            multiple
                            chips
                            deletable-chips
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
                    :return-value.sync="appointmentStartDate"
                    transition="scale-transition"
                    offset-y
                    full-width
                    min-width="290px">
              <v-text-field readonly slot="activator"
                            v-model="startDateFormatted"
                            clearable
                            :label="$t('report.appointmentStartDate')">
              </v-text-field>

              <v-date-picker v-model="appointmentStartDate"
                            no-title
                            @input="$refs.menu1.save(appointmentStartDate)"
                            :max="getMaxDate()">
              </v-date-picker>
            </v-menu>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-menu ref="menu2"
                    :close-on-content-click="false"
                    v-model="menu2"
                    :nudge-right="40"
                    lazy
                    :return-value.sync="appointmentEndDate"
                    transition="scale-transition"
                    offset-y full-width min-width="290px">
              <v-text-field readonly
                            clearable slot="activator"
                            v-model="endDateFormatted"
                            :label="$t('report.appointmentEndDate')">
              </v-text-field>

              <v-date-picker v-model="appointmentEndDate"
                            no-title
                            @input="$refs.menu2.save(appointmentEndDate)"
                            :max="getMaxDate()">
              </v-date-picker>
            </v-menu>
          </v-flex>
        </div>
      </v-form>
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
      menu2: false,
      startDateFormatted: null,
      endDateFormatted: null,
      customParameters: {},
      branchId: null,
      doctorId: null,
      patientId: null,
      appointmentStartDate: null,
      appointmentEndDate: null,
      filteredDoctors: [],
      doctorCards: [],
      valid: true
    };
  },

  watch: {
    appointmentStartDate(val) {
      const vm = this;

      vm.startDateFormatted = vm.formatDate(vm.appointmentStartDate);
    },

    appointmentEndDate(val) {
      const vm = this;

      vm.endDateFormatted = vm.formatDate(vm.appointmentEndDate);
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

      if (vm.branchId) {
        vm.customParameters.branchId = vm.branchId.join();
      }

      if (vm.doctorId) {
        vm.customParameters.doctorId = vm.doctorId.join();
      }

      if (vm.patientId) {
        vm.customParameters.patientId = vm.patientId.join();
      }

      vm.customParameters.appointmentStartDate = vm.appointmentStartDate;
      vm.customParameters.appointmentEndDate = vm.appointmentEndDate;

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
        appointmentStartDate: vm.appointmentStartDate,
        appointmentEndDate: vm.appointmentEndDate
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
    },

    clearReport() {
      const vm = this;

      vm.clear();
    },

    clear() {
      const vm = this;

      vm.$refs.form.reset();
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
