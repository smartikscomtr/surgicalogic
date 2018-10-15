<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="overtimeOperations"
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
                    @detail="detail"
                    @exportToExcel="exportOvertimeReportToExcel"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
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
      title: vm.$i18n.t('report.overtimeReportTitle'),
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
      deletePath: ''
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'operationName',
          sortBy: 'Operation.Name',
          text: vm.$i18n.t('report.operationName'),
          sortable: true,
          align: 'left'
         },
        {
          value:'operatingRoomName',
          sortBy:'OperatingRoom.Name',
          text: vm.$i18n.t('report.roomName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'operationStartDate',
          sortBy:'OperationDate',
          text: vm.$i18n.t('report.operationDate'),
          isDateTime: true,
          sortable: true,
          align: 'left'
        },
        {
          value: 'realizedStartDate',
          text: vm.$i18n.t('report.realizedStartDate'),
          isDateTime: true,
          sortable: false,
          align: 'left'
        },
        {
          value: 'realizedEndDate',
          text: vm.$i18n.t('report.realizedEndDate'),
          isDateTime: true,
          sortable: false,
          align: 'left'
        },
        {
          value: 'realizedEndDate',
          text: vm.$i18n.t('report.overtime'),
          isDateTime: true,
          sortable: false,
          align: 'left'
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
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
    getMethodName(){
      return "getOvertimeOperations";
    },

    deleteMethodName(){
      return "";
    },

    exportOvertimeReportToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOvertimeOperations');
    }
  }
};

</script>
