<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="workTypes"
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
                    @exportToExcel="exportWorkTypeToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <work-types-edit-component :edit-action="editAction"
                               :edit-visible="editDialog"
                               :edit-index="editedIndex"
                               @cancel="cancel">
    </work-types-edit-component>

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
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      editedIndex: -1,
      totalRowCount:0,
      deletePath: 'deleteWorkType'
    };
  },

  computed: {
     title() {
      const vm = this;

      return vm.$i18n.t('worktypes.workTypes');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('worktypes.workTypes'),
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

    workTypes() {
      const vm = this;

      return vm.$store.state.workTypesModule.workTypes;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.workTypesModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.workTypesModule.totalCount;
    }
  },

  methods: {
    getMethodName(){
      return "getWorkTypes";
    },

    deleteMethodName(){
      return "deleteWorkType";
    },

    exportWorkTypeToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportWorkType');
    },

    getWorkTypes(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
