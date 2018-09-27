<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="personnels"
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
                    @exportToExcel="exportPersonnelsToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-edit-component :edit-action="editAction"
                              :edit-visible="editDialog"
                              :edit-index="editedIndex"
                              @cancel="cancel">
    </personnel-edit-component>

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
      title: vm.$i18n.t('personnel.personnels'),
      search: '',
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      totalRows:0,
      editedIndex: -1,
      personnelTitleLoadOnce: true,
      workTypeLoadOnce: true,
      branchLoadOnce: true,
      deletePath: 'deletePersonnel'
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'personnelCode',
          text: vm.$i18n.t('personnel.personnelCode'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'firstName',
          text: vm.$i18n.t('personnel.firstName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'lastName',
          text: vm.$i18n.t('personnel.lastName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'personnelTitleName',
          sortBy: 'PersonnelTitle.Name',
          text: vm.$i18n.t('personnel.personnelTitle'),
          sortable: true,
          align: "left"
        },
        {
          value: 'branchNames',
          sortBy:'BranchNames',
          text: vm.$i18n.t('branches.branch'),
          sortable: false,
          align: 'left'
        },
        {
          value: 'workTypeName',
          sortBy: 'WorkType.Name',
          text: vm.$i18n.t('worktypes.workType'),
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

    personnels() {
      const vm = this;

      return vm.$store.state.personnelModule.personnel;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.personnelModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.personnelModule.totalCount;
    }
  },

  watch: {
   editDialog() {
    const vm = this;

     //We are accessing getWorkTypes in vuex store
     if(vm.workTypeLoadOnce){
        vm.$store.dispatch('getAllWorkTypesForPersonnel');
        vm.workTypeLoadOnce = false;
     }

     //We are accessing getPersonnelTitles in vuex store
     if(vm.personnelTitleLoadOnce){
        vm.$store.dispatch('getAllPersonnelTitlesForPersonnel');
        vm.personnelTitleLoadOnce = false;
     }

     //We are accessing getBranches in vuex store
     if(vm.branchLoadOnce){
        vm.$store.dispatch('getAllBranchesForPersonnel');
        vm.branchLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getPersonnels";
    },

    deleteMethodName(){
      return "deletePersonnel";
    },

    exportPersonnelsToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportPersonnel');
    }
  }
};

</script>
