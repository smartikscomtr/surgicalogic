<template>
  <div>
    <grid-component :headers="headers"
                    :items="operationTypes"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    :methodName="getMethodName"
                    :totalCount="getTotalCount"
                    :pagination.sync="pagination"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
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
                      @cancel="cancel">
    </delete-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('operationtypes.operationtypes'),
      search: '',
      detailDialog: false,
      editDialog: false,
      deleteDialog: false,
      detailAction: {},
      editAction: {},
      deleteValue: {},
      pagination: {},
      editedIndex: -1,
      totalRowCount:0,
      branchesLoadOnce: true,
      equipmentsLoadOnce: true,
      operatingRoomsLoadOnce: true,
      personnelLoadOnce: true
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('operationtypes.operationtype'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'branchName',
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

    getTotalCount() {
      const vm = this;

      return vm.$store.state.operationTypeModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllEquipments in vuex store
     if(vm.branchesLoadOnce){
        vm.$store.dispatch('getAllBranches');
        vm.branchesLoadOnce = false;
     }

    //We are accessing getAllEquipments in vuex store
     if(vm.equipmentsLoadOnce){
        vm.$store.dispatch('getAllEquipments');
        vm.equipmentsLoadOnce = false;
     }

    //We are accessing getAllEquipmentTypes in vuex store
     if(vm.operatingRoomsLoadOnce){
        vm.$store.dispatch('getAllOperatingRooms');
        vm.operatingRoomsLoadOnce = false;
     }

    //We are accessing getAllPersonnels in vuex store
     if(vm.personnelLoadOnce){
        vm.$store.dispatch('getAllPersonnels');
        vm.personnelLoadOnce = false;
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
      vm.editedIndex = vm.operationTypes.indexOf(payload);
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
    },

    getMethodName(){
      return "getOperationTypes";
    }
  }
};

</script>
