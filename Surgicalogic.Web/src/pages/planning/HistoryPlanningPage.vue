<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="historyPlannings"
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
                    @exportToExcel="exportPlanningHistoryToExcel"
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
      title: vm.$i18n.t('plan.planningHistory'),
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
          text: vm.$i18n.t('plan.operationName'),
          sortable: true,
          align: 'left'
         },
        {
          value:'operatingRoomName',
          sortBy:'OperatingRoom.Name',
          text: vm.$i18n.t('plan.roomName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'operationStartDate',
          sortBy:'OperationDate',
          text: vm.$i18n.t('plan.operationDate'),
          isDateTime: true,
          sortable: true,
          align: 'left'
        },
        {
          value: 'realizedStartDate',
          text: vm.$i18n.t('plan.realizedStartDate'),
          isDateTime: true,
          sortable: false,
          align: 'left'
        },
        {
          value: 'realizedEndDate',
          text: vm.$i18n.t('plan.realizedEndDate'),
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

    historyPlannings() {
      const vm = this;

      return vm.$store.state.historyPlanningModule.plan;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.historyPlanningModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.historyPlanningModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllOperationTypes in vuex store
     if(vm.editLoadOnce){
        vm.$store.dispatch('getAllOperationTypes');
        vm.editLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getOperationPlanHistory";
    },

    deleteMethodName(){
      return "";
    },

    exportPlanningHistoryToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportPlanningHistory');
    }
  }
};

</script>
