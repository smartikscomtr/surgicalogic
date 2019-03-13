<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="personnelCategories"
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
                    @exportToExcel="exportPersonnelCategoryToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <personnel-category-edit-component :edit-action="editAction"
                                    :edit-visible="editDialog"
                                    :edit-index="editedIndex"
                                    @cancel="cancel">
    </personnel-category-edit-component>

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
      deletePath: 'deletePersonnelCategory'
    };
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('personnelcategory.personnelCategories');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('personnelcategory.personnelCategories'),
          sortable: true,
          align: 'left'
        },
        {
          isCheck: true,
          value: 'suitableForMultipleOperation',
          text: vm.$i18n.t('personnelcategory.suitableForMultipleOperation'),
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

    personnelCategories() {
      const vm = this;

      return vm.$store.state.personnelCategoryModule.personnelCategory;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.personnelCategoryModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.personnelCategoryModule.totalCount;
    }
  },

  methods: {
    getMethodName(){
      return "getPersonnelCategories";
    },

    deleteMethodName(){
      return "deletePersonnelCategory";
    },

    exportPersonnelCategoryToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportPersonnelCategory', { langId: vm.$cookie.get("currentLanguage")});
    },

    getPersonnelCategories(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
