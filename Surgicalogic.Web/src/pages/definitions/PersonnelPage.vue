<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnels"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-detail-component :detail-action="detailAction"
                               :detail-visible="detailDialog"
                               @cancel="cancel">
    </personnel-detail-component>

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
      detailDialog: false,
      editDialog: false,
      deleteDialog: false,
      detailAction: {},
      editAction: {},
      deleteValue: {},
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
          value: 'personnelTitleName',
          text: vm.$i18n.t('personneltitle.personnelTitle'),
          sortable: true,
          align: "left"
        },
        {
          value: 'branchName',
          text: vm.$i18n.t('branchs.branch'),
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
    }
  },

  watch: {
   editDialog() {
    const vm = this;

     //We are accessing getWorkTypes in vuex store
     if(vm.workTypeLoadOnce){
        vm.$store.dispatch('getWorkTypes');
        vm.workTypeLoadOnce = false;
     }

     //We are accessing getPersonnelTitles in vuex store
     if(vm.personnelTitleLoadOnce){
        vm.$store.dispatch('getPersonnelTitles');
        vm.personnelTitleLoadOnce = false;
     }

     //We are accessing getBranchs in vuex store
     if(vm.branchLoadOnce){
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
      // vm.editedIndex = vm.personnel.indexOf(payload);
      vm.editAction = payload;
    },

    cancel() {
      const vm = this;

      vm.detailDialog = false;
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
    }
  },

  created() {
    const vm = this;

    //We are accessing getPersonnels in vuex store
    vm.$store.dispatch('getPersonnels');
  }
};

</script>
