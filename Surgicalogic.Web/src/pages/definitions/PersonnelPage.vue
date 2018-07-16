<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnels"
                    :pagination.sync="pagination"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :methodName="getMethodName"
                    :totalRowCount="getTotalCount"
                    @edit="edit"
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
                      @cancel="cancel">
    </delete-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('personnel.personnels'),
      search: '',
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      pagination:{},
      totalRows:0,
      editedIndex: -1,
      personnelTitleLoadOnce: true,
      workTypeLoadOnce: true,
      branchLoadOnce: true
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
          text: vm.$i18n.t('personneltitle.personnelTitle'),
          sortable: true,
          align: "left"
        },
        {
          value: 'branchName',
          text: vm.$i18n.t('branches.branch'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'workTypeName',
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
        vm.$store.dispatch('getAllWorkTypes');
        vm.workTypeLoadOnce = false;
     }

     //We are accessing getPersonnelTitles in vuex store
     if(vm.personnelTitleLoadOnce){
        vm.$store.dispatch('getAllPersonnelTitles');
        vm.personnelTitleLoadOnce = false;
     }

     //We are accessing getBranches in vuex store
     if(vm.branchLoadOnce){
        vm.$store.dispatch('getAllBranches');
        vm.branchLoadOnce = false;
     }
    }
  },

  methods: {
    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.personnels.indexOf(payload);
      vm.editAction = payload;
    },

    cancel() {
      const vm = this;

      vm.editDialog = false;
      vm.deleteDialog = false;
    },

    addNewItem(){
      const vm = this;

      vm.editDialog = true;
    },

    deleteItem(payload) {
      const vm = this;

      vm.deleteValue = payload;
      vm.deleteDialog = true;
    },

    getMethodName(){
      return "getPersonnels";
    }
  },

  created() {
    // const vm = this;

    // //We are accessing getPersonnels in vuex store
    // vm.$store.dispatch('getPersonnels');
  }
};

</script>
