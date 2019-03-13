<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="branches"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @edit="edit"
                    @exportToExcel="exportBranchesToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <branches-edit-component :edit-action="editAction"
                            :edit-visible="editDialog"
                            :edit-index="editedIndex"
                            @cancel="cancel">
    </branches-edit-component>

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
      search: 'Ara',
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      totalRowCount:0,
      editedIndex: -1,
      deletePath: 'deleteBranch'
    };
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('branches.branches');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('branches.branch'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'description',
          text: vm.$i18n.t('common.description'),
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

    branches() {
      const vm = this;

      return vm.$store.state.branchesModule.branches;
    },


    getTotalCount() {
     const vm = this;

      return  vm.$store.state.branchesModule.totalCount;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.branchesModule.loading;
    }
  },

  methods: {
    getMethodName(){
     return "getBranches";
    },

    deleteMethodName(){
      return "deleteBranch";
    },

    exportBranchesToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportBranches', { langId: vm.$cookie.get("currentLanguage")});
    },

    getBranches(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
