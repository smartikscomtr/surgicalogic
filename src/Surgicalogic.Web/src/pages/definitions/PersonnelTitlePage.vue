<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="personnelTitles"
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
                    @exportToExcel="exportPersonnelTitleToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <personnel-title-edit-component :edit-action="editAction"
                                    :edit-visible="editDialog"
                                    :edit-index="editedIndex"
                                    @cancel="cancel">
    </personnel-title-edit-component>

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
      deletePath: 'deletePersonnelTitle'
    };
  },

  computed: {
     title() {
      const vm = this;

      return vm.$i18n.t('personneltitle.personnelTitles');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('personneltitle.personnelTitles'),
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

    personnelTitles() {
      const vm = this;

      return vm.$store.state.personnelTitleModule.personnelTitle;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.personnelTitleModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.personnelTitleModule.totalCount;
    }
  },

  methods: {
    getMethodName(){
      return "getPersonnelTitles";
    },

    deleteMethodName(){
      return "deletePersonnelTitle";
    },

    exportPersonnelTitleToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportPersonnelTitle', { langId: vm.$cookie.get("currentLanguage")});
    },

    getPersonnelTitles(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
