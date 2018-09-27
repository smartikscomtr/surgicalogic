<template>
  <div>
    <grid-component :headers="headers"
                    :items="settings"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="false"
                    :show-search="true"
                    :show-insert="false"
                    :hide-actions="false"
                    :hide-export="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @detail="detail"
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
      title: vm.$i18n.t('settings.title'),
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
          value:'Name',
          sortBy: 'Name',
          text: vm.$i18n.t('settings.name'),
          sortable: true,
          align: 'left'
         },
        {
          value:'Value',
          sortBy:'Value',
          text: vm.$i18n.t('settings.value'),
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
