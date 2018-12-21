<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="operationTypes"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="true"
                    :loading="getLoading"
                    :methodName="getMethodName"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    @edit="edit"
                    @exportToExcel="exportOperationTypeToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <operation-types-detail-component :detail-action="detailAction"
                                      :detail-visible="detailDialog"
                                      @cancel="cancel">
    </operation-types-detail-component>

    <operation-types-edit-component :edit-action="editAction"
                                    :edit-visible="editDialog"
                                    :edit-index="editedIndex"
                                    @cancel="cancel">
    </operation-types-edit-component>

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
      deletePath: 'deleteOperationType'
    };
  },

  computed: {
     title() {
      const vm = this;

      return vm.$i18n.t('operationtypes.operationtypes');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('operationtypes.operationType'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'branchName',
          sortBy: 'Branch.Name',
          text: vm.$i18n.t('branches.branch'),
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

    operationTypes() {
      const vm = this;

      return vm.$store.state.operationTypeModule.operationTypes;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.operationTypeModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.operationTypeModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllEquipments, getAllBranches and getAllOperatingRooms in vuex store
     if(vm.editLoadOnce){
        vm.$store.dispatch('getAllBranchesForOperationType');
        vm.$store.dispatch('getAllEquipmentsForOperationType');
        vm.$store.dispatch('getAllOperatingRoomsForOperationType');
        vm.branchesLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getOperationTypes";
    },

    deleteMethodName(){
      return "deleteOperationType";
    },

    exportOperationTypeToExcel() {
      const vm = this;

      vm.$store.dispatch('exportOperationTypeToExcel');
    },

    getOperationTypes(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
