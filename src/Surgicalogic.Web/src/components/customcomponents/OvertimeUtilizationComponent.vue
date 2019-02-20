<template>
    <div>
      <grid-component :headers="headers"
                      :items="overtimeUtilizations"
                      :title="title"
                      :show-detail="false"
                      :show-edit="false"
                      :show-delete="false"
                      :show-search="false"
                      :show-insert="false"
                      :hide-actions="false"
                      :hide-export="true"
                      :methodName="getMethodName"
                      :loading="getLoading"
                      :totalCount="getTotalCount"
                      @detail="detail"
                      :custom-parameters="customParameters"
                      @exportToExcel="exportOvertimeUtilizationReportToExcel"
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
      menu: false,
      dateFormatted: null,
      date: null,
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
      customParameters: {}
    };
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('planarrangements.overtimeUtilization');
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

    exportOvertimeUtilizationReportToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOvertimeUtilization');
    },

    formatDate() {
      const vm = this;

      const toTwoDigits = num => (num < 10 ? '0' + num : num);
      let today = new Date();

      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let tomorrow = toTwoDigits(today.getDate() + 1);

      return `${year}-${month}-${tomorrow}`;
    },

    addFormatDate() {
      const vm = this;

      const toTwoDigits = num => (num < 10 ? '0' + num : num);
      let today = new Date();

      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let twoNextDay = toTwoDigits(today.getDate() + 2);

      return `${year}-${month}-${twoNextDay}`;
    }
  },

  mounted() {
    const vm = this;

    vm.customParameters.operationStartDate = vm.formatDate()
    vm.customParameters.operationEndDate = vm.addFormatDate()
  },

  created () {
    const vm = this;

    vm.customParameters.operationStartDate = vm.formatDate()
    vm.customParameters.operationEndDate = vm.addFormatDate()
  }
}

</script>
