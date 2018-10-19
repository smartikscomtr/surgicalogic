<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="operations"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    @edit="edit"
                    @exportToExcel="exportOperationToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <operation-detail-component :detail-action="detailAction"
                                 :detail-visible="detailDialog"
                                 @cancel="cancel">
    </operation-detail-component>

    <operation-edit-component :edit-action="editAction"
                               :edit-visible="editDialog"
                               :edit-index="editedIndex"
                               @cancel="cancel">
    </operation-edit-component>

    <delete-component :delete-value="deleteValue"
                      :delete-visible="deleteDialog"
                      :deleteMethode="deleteMethodName"
                      @cancel="cancel">
    </delete-component>
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
      deletePath: 'deleteOperation'
    };
  },

  computed: {
     title() {
      const vm = this;

      return vm.$i18n.t('operation.operations');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('operation.operationName'),
          sortable: true,
          align: 'left'
        },
        {
          value:'operationTypeName',
          sortable:'OperationType.Name',
          text: vm.$i18n.t('operation.operationType'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'date',
          text: vm.$i18n.t('operation.operationDate'),
          sortable: true,
          isDate: true,
          align: 'left'
        },
        {
          value: 'operationTime',
          text: vm.$i18n.t('operation.operationTime'),
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

    operations() {
      const vm = this;

      return vm.$store.state.operationModule.operation;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.operationModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.operationModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllOperationTypes in vuex store
     if(vm.editLoadOnce){
         vm.$store.dispatch('getOperationTypesByBranchId');
        // vm.$store.dispatch('getPersonnelsByOperationTypeId');
        vm.editLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getOperations";
    },

    deleteMethodName(){
      return "deleteOperation";
    },

    exportOperationToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOperation');
    },

    getOperations(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
