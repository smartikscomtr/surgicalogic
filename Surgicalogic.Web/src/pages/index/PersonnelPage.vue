<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnels"
                    :pagination.sync="pagination"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    :methodName="getMethodName"
                    :totalRowCount="getTotalCount"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-edit-component :edit-action="editAction"
                              :edit-visible="editDialog"
                              :edit-index="editedIndex"
                              :delete-value="deleteValue"
                              @cancel="cancel">
    </personnel-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('personnel.personnels'),
      search: '',
      detailDialog: false,
      editDialog: false,
      detailAction: {},
      editAction: {},
      deleteValue: {},
      pagination:{},
      totalRows:0,
      editedIndex: -1
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
          text: vm.$i18n.t('personnel.givenName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'lastName',
          text: vm.$i18n.t('personnel.familyName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'personnelTitleId',
          text: vm.$i18n.t('personnel.personnelTitle'),
          sortable: true,
          align: "left"
        },
        {
          value: 'branchId',
          text: vm.$i18n.t('personnel.branch'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'workTypeId',
          text: vm.$i18n.t('personnel.workType'),
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

     if(vm.workTypeLoadOnce){
        //We are accessing getWorkTypes in vuex store
        vm.$store.dispatch('getWorkTypes');
        vm.workTypeLoadOnce = false;
     }

     if(vm.personnelTitleLoadOnce){
        //We are accessing getPersonnelTitles in vuex store
        vm.$store.dispatch('getPersonnelTitles');
        vm.personnelTitleLoadOnce = false;
     }

     if(vm.branchLoadOnce){
        //We are accessing getBranchs in vuex store
        vm.$store.dispatch('getBranchs');
        vm.branchLoadOnce = false;
     }
    }
  },

  methods: {
    detail(payload) {
      const vm = this;

      vm.detailDialog = true;
      vm.detailAction = payload;
    },

    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.personnel.indexOf(payload);
      vm.editAction = payload;
    },

    cancel() {
      const vm = this;

      vm.detailDialog = false;
      vm.editDialog = false;
    },

    addNewItem(){
      const vm = this;

      vm.editDialog = true;
    },

    deleteItem(payload) {
      const vm = this;

      //We are accessing deletePersonnel in vuex store
      vm.$store.dispatch('deletePersonnel', {
        id: payload.id
      });

      vm.deleteValue = payload;
    },

    getMethodName(){
      return "getPersonnels"
    }
  },

  created() {
    // const vm = this;

    // //We are accessing getPersonnels in vuex store
    // vm.$store.dispatch('getPersonnels');
  }
};

</script>
