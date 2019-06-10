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
                            :label="$t('report.operationStartDate')">
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
                    offset-y full-width min-width="290px">
              <v-text-field readonly
                            clearable slot="activator"
                            v-model="endDateFormatted"
                            :label="$t('report.operationEndDate')">
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
                    :items="overtimeUtilizations"
                    :title="title"
                    :show-detail="false"
                    :show-edit="false"
                    :show-delete="false"
                    :show-search="false"
                    :show-insert="false"
                    :hide-actions="false"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    :custom-parameters="customParameters"
                    @detail="detail"
                    @exportToExcel="exportOvertimeUtilizationReportToExcel"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <div class="grid-card v-card flex layout">
      <v-flex xs6 sm6 md6 class="chart">
        <span class="headline">
          {{ $t('report.overTime') }}
        </span>

        <column-chart :data="overtimeChartData"
                   donut
                   :messages="{empty: $t('report.notFoundRoomInfo')}">
        </column-chart>
      </v-flex>

      <v-flex xs6 sm6 md6 class="chart">
        <span class="headline">
          {{ $t('report.utilization') }}
        </span>

        <pie-chart :data="utilizationChartData"
                   donut
                   prefix="%"
                   legend="bottom"
                   :messages="{empty: $t('report.notFoundRoomInfo')}">
        </pie-chart>
      </v-flex>
    </div>
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
      customParameters: {},
      startDateFormatted: null,
      endDateFormatted: null,
      menu1: false,
      menu2: false,
      dateFormatted: null,
      startDate: null,
      endDate: null,
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

      return vm.$i18n.t('report.overtimeUtilizationReport');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'operatingRoom',
          text: vm.$i18n.t('report.operatingRoom'),
          sortBy:'OperatingRoom',
          sortable: true,
          align: 'left'
        },
        {
          value:'overtime',
          sortBy: 'Overtime',
          text: vm.$i18n.t('report.overTime'),
          sortBy:'Overtime',
          sortable: true,
          align: 'left'
        },
        {
          value:'utilization',
          text: vm.$i18n.t('report.utilization'),
          sortBy:'Utilization',
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

    overtimeUtilizations() {
      const vm = this;

      return vm.$store.state.reportsModule.overtimeUtilization;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.reportsModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.reportsModule.totalCount;
    },

    overtimeChartData() {
      const vm = this;

      return vm.$store.state.reportsModule.overtimeUtilization.map(x => [x.operatingRoom + " " + vm.$i18n.t('report.room'), x.overtime]);
    },

    utilizationChartData() {
      const vm = this;

      return vm.$store.state.reportsModule.overtimeUtilization.map(x => [x.operatingRoom+ " " + vm.$i18n.t('report.room'), x.utilization.replace('%', '')]);
    }
  },

  methods: {
    getMethodName(){
      return "getOvertimeUtilization";
    },

    deleteMethodName(){
      return "";
    },

    replaceForAutoComplete(text) {
      return text.replace(/İ/g, 'i')
                 .replace(/I/g, 'ı')
                 .toLowerCase();
    },

    filteredReport() {
      const vm = this;

      if (vm.startDateFormatted) {
        vm.customParameters.operationStartDate = vm.startDate;
      } else {
        vm.customParameters.operationStartDate = null;
      }

      if (vm.endDateFormatted) {
        vm.customParameters.operationEndDate = vm.endDate;
      } else {
        vm.customParameters.operationEndDate = null;
      }

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    },

    exportOvertimeUtilizationReportToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOvertimeUtilization', { langId: vm.$cookie.get("currentLanguage"), operationStartDate: vm.startDate, operationEndDate: vm.endDate});
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

    getDate() {
        const toTwoDigits = num => (num < 10 ? '0' + num : num);
        let today = new Date();
        let year = today.getFullYear();
        let month = toTwoDigits(today.getMonth() + 1);
        let day = toTwoDigits(today.getDate());

        return `${year}-${month}-${day}`;
    },

    getMinDate() {
        const toTwoDigits = num => (num < 10 ? '0' + num : num);
        let selectDay = new Date();

        selectDay.setDate(selectDay.getDate() - 7);

        let year = selectDay.getFullYear();
        let month = toTwoDigits(selectDay.getMonth() + 1);
        let day = toTwoDigits(selectDay.getDate());

        return `${year}-${month}-${day}`;
    },

    clearReport() {
      const vm = this;

      vm.clear();
    },

    clear() {
      const vm = this;

      vm.$refs.form.reset();
    },
  },

  mounted() {
    const vm = this;

    vm.customParameters.operationStartDate = vm.startDate;
    vm.customParameters.operationEndDate = vm.endDate;
  },

  created () {
    const vm = this;

    vm.startDate = vm.getMinDate();
    vm.endDate = vm.getDate();

    // vm.$store.dispatch('getOvertimeUtilization', {
    //   operationStartDate: vm.startDate,
    //   operationEndDate: vm.endDate
    // });
  }
};

</script>
