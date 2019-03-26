<template>
  <div>
    <div class="container fluid grid-list-md">
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
                      @deleteitem="deleteItem"
                      ref="gridComponent">
      </grid-component>

      <settings-edit-component :edit-action="editAction"
                                      :edit-visible="editDialog"
                                      :edit-index="editedIndex"
                                      @cancel="cancel">
      </settings-edit-component>
    </div>
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
      deletePath: ''
    };
  },

  computed: {
     title() {
      const vm = this;

      return vm.$i18n.t('settings.title');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'name',
          sortBy: 'Name',
          text: vm.$i18n.t('settings.name'),
          sortable: true,
          align: 'left'
         },
        {
          value:'value',
          sortBy:'Value',
          text: vm.$i18n.t('settings.value'),
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

    settings() {
      const vm = this;

      return vm.$store.state.settingsModule.settings;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.settingsModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.settingsModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllOperationTypes in vuex store
     if(vm.editLoadOnce){
        vm.$store.dispatch('getSettingDataTypes');
        vm.$store.dispatch('getSettingValues');
        vm.editLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getSettings";
    },

    deleteMethodName(){
      return "";
    },

    getSettings(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }

    // exportPlanningHistoryToExcel() {
    //   const vm = this;

    //   vm.$store.dispatch('excelExportPlanningHistory');
    // }
  }
};

</script>
