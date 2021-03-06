<template>
  <div class="container fluid grid-list-md">
    <div class="grid-card v-card">
      <v-form ref="form" v-model="valid" lazy-validation>
        <div class="v-card__text layout row wrap">
          <v-flex xs12 sm12 md12>
            <div class="btn-wrap">
              <v-btn class="btnSave orangeButton"
                    lang="tr"
                    @click.native="filteredReport">
                {{ $t('report.filter') }}
              </v-btn>
            </div>

            <div class="btn-wrap">
              <v-btn class="btnSave orangeButton"
                    lang="tr"
                    @click.native="clearReport">
                {{ $t('report.clear') }}
              </v-btn>
            </div>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-autocomplete v-model="selectBranch"
                            :items="branches"
                            :label="$t('branches.branch')"
                            box
                            clearable
                            multiple
                            chips
                            deletable-chips
                            :filter="customFilter"
                            item-text="name"
                            item-value="id">
            </v-autocomplete>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-autocomplete v-model="selectDoctor"
                            :items="doctors"
                            :label="$t('personnel.doctor')"
                            box
                            clearable
                            multiple
                            chips
                            deletable-chips
                            :filter="customFilterForDoctor"
                            item-text="personnelTitleName"
                            item-value="id">
            </v-autocomplete>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-menu ref="menu1"
                    :close-on-content-click="false"
                    v-model="menu1"
                    :nudge-right="40"
                    lazy
                    :return-value.sync="startDate"
                    transition="scale-transition"
                    offset-y full-width min-width="290px">
              <v-text-field readonly
                            clearable
                            slot="activator"
                            v-model="startDateFormatted"
                            :label="$t('report.realizedStartDate')">
              </v-text-field>

              <v-date-picker v-model="startDate"
                             no-title
                             :locale="getLocale()"
                             @input="$refs.menu1.save(startDate)"
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
                    :return-value.sync="endDate"
                    transition="scale-transition"
                    offset-y
                    full-width
                    min-width="290px">
              <v-text-field readonly
                            clearable
                            slot="activator"
                            v-model="endDateFormatted"
                            :label="$t('report.realizedEndDate')">
              </v-text-field>

              <v-date-picker v-model="endDate"
                             no-title
                             :locale="getLocale()"
                             @input="$refs.menu2.save(endDate)"
                             :max="getMaxDate()">
              </v-date-picker>
            </v-menu>
          </v-flex>
        </div>
      </v-form>
    </div>

    <grid-component :headers="headers"
                    :items="overtimeOperations"
                    :title="title"
                    :show-detail="false"
                    :show-edit="false"
                    :show-delete="false"
                    :show-search="true"
                    :show-insert="false"
                    :hide-actions="false"
                    :custom-parameters="customParameters"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    @exportToExcel="exportOvertimeReportToExcel"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>
  </div>
</template>

<script>

import { gridMixin } from './../../mixins/gridMixin';

import { localizationMixin } from './../../mixins/localizationMixin';

export default {
  mixins: [
    gridMixin,
    localizationMixin
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
      totalRowCount: 0,
      editLoadOnce: true,
      deletePath: '',
      branchId: null,
      doctorId: null,
      filteredDoctors: [],
      startDateFormatted: null,
      endDateFormatted: null,
      doctorCards: [],
      menu1: false,
      menu2: false,
      dateFormatted: null,
      startDate: null,
      endDate: null,
      customParameters: {},
      valid: true
    };
  },

  watch: {
    startDate(val) {
      const vm = this;

      vm.startDateFormatted = vm.formatDate(vm.startDate);
    },

    endDate(val) {
      const vm = this;

      vm.endDateFormatted = vm.formatDate(vm.endDate);
    }
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('report.overtimeReportTitle');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'branchName',
          text: vm.$i18n.t('report.branchName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'operationName',
          text: vm.$i18n.t('report.operationName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'doctorName',
          text: vm.$i18n.t('report.doctorName'),
          sortable: false,
          align: 'left'
        },
        {
          value: 'operationRoomName',
          text: vm.$i18n.t('report.roomName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'operationStartDate',
          text: vm.$i18n.t('report.plannedStartDate'),
          isDateTime: true,
          align: 'left'
        },
        {
          value: 'operationEndDate',
          text: vm.$i18n.t('report.plannedEndDate'),
          isDateTime: true,
          align: 'left'
        },
        {
          value: 'realizedStartDate',
          text: vm.$i18n.t('report.realizedStartDate'),
          isDateTime: true,
          align: 'left'
        },
        {
          value: 'realizedEndDate',
          text: vm.$i18n.t('report.realizedEndDate'),
          isDateTime: true,
          align: 'left'
        },
        {
          value: 'operationTimeDifference',
          isOvertime: 'isOvertime',
          text: vm.$i18n.t('report.overtime'),
          sortable: true,
          overtimeValue: true,
          align: 'left'
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

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
    },

    overtimeOperations() {
      const vm = this;

      return vm.$store.state.reportsModule.reportItems;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.reportsModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.reportsModule.totalCount;
    }
  },

  methods: {
    getMethodName() {
      return 'getOvertimeOperations';
    },

    deleteMethodName() {
      return '';
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
      if (!date || date.indexOf('.') > -1)
        return null;

      const [year, month, day] = date.split('-');

      return `${day}.${month}.${year}`;
    },

    exportOvertimeReportToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOvertimeOperations', { langId: vm.$cookie.get("currentLanguage")});
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

    replaceForAutoComplete(text) {
      return text.replace(/İ/g, 'i')
                 .replace(/I/g, 'ı')
                 .toLowerCase();
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

    filteredReport() {
      const vm = this;

      if (vm.branchId) {
        vm.customParameters.branchId = vm.branchId.join();
      } else {
        vm.customParameters.branchId = null;
      }

      if (vm.doctorId) {
        vm.customParameters.doctorId = vm.doctorId.join();
      } else {
        vm.customParameters.doctorId = null;
      }

      if (vm.startDateFormatted) {
        vm.customParameters.realizedStartDate = vm.startDate;
      } else {
        vm.customParameters.realizedStartDate = null;
      }

      if (vm.endDateFormatted) {
        vm.customParameters.realizedEndDate = vm.endDate;
      } else {
        vm.customParameters.realizedEndDate = null;
      }

      var child = this.$refs.gridComponent;
      child.executeGridOperations(true);
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

  created() {
    const vm = this;

    vm.$store.dispatch('getDoctorsByBranchIdAsync', { branchId: 0 })
        .then(() => {
          vm.filteredDoctors = vm.$store.state.personnelModule.filteredDoctor;
          vm.doctorCards = vm.filteredDoctors;
        });

    vm.$store.dispatch('getAllBranches');
  }
};
</script>
