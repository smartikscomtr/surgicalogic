<template>
  <div>
    <grid-component :headers="headers"
                    :items="equipments"
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

    <equipments-detail-component :detail-action="detailAction"
                                 :detail-visible="detailDialog"
                                 @cancel="cancel">
    </equipments-detail-component>

    <equipments-edit-component :edit-action="editAction"
                               :edit-visible="editDialog"
                               :edit-index="editedIndex"
                               @cancel="cancel">
    </equipments-edit-component>

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
      title: vm.$i18n.t('equipments.equipments'),
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
      equipmentTypeLoadOnce: true
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('equipments.name'),
          sortable: true,
          align: 'left'
        },
        {
          value:'equipmentTypeName',
          text: vm.$i18n.t('equipmenttypes.equipmentType'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'isPortable',
          text: vm.$i18n.t('equipments.portable'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'description',
          text: vm.$i18n.t('equipments.description'),
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

    equipments() {
      const vm = this;

      return vm.$store.state.equipmentModule.equipments;
    },
    getTotalCount() {
      const vm = this;

      return vm.$store.state.equipmentModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllEquipmentTypes in vuex store
     if(vm.equipmentTypeLoadOnce){
        vm.$store.dispatch('getAllEquipmentTypes');
        vm.equipmentTypeLoadOnce = false;
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
      vm.editedIndex = vm.equipments.indexOf(payload);
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

	  // if () {
      //   vm.$store.dispatch('deleteEquipment', {
      //     id: vm.deleteValue.id
      //   });
      // }
    },

    getMethodName(){
      return "getEquipments"
    }
  }

  // created() {
  //    const vm = this;

  //    //We are accessing getEquipments in vuex store
  //    vm.$store.dispatch('getEquipments');
  // }
};

</script>
