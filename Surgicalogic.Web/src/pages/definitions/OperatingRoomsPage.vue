<template>
  <div>
    <grid-component :headers="headers"
                    :items="operatingRooms"
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

    <operating-rooms-detail-component :detail-action="detailAction"
                                       :detail-visible="detailDialog"
                                       @cancel="cancel">
    </operating-rooms-detail-component>

    <operating-rooms-edit-component :edit-action="editAction"
                                    :edit-visible="editDialog"
                                    :edit-index="editedIndex"
                                    @cancel="cancel">
    </operating-rooms-edit-component>

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
      title: vm.$i18n.t('operatingrooms.operatingRooms'),
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
      equipmentLoadOnce: true,
      equipmentName: null
    }
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('operatingrooms.operatingRoom'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'location',
          text: vm.$i18n.t('operatingrooms.location'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'width',
          text: vm.$i18n.t('operatingrooms.width'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'height',
          text: vm.$i18n.t('operatingrooms.height'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'length',
          text: vm.$i18n.t('operatingrooms.length'),
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

    operatingRooms() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.operatingRooms;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.totalCount;
    }
  },

  // watch: {
  //  editDialog(){
  //    const vm = this;

  //   //We are accessing getAllEquipments in vuex store
  //    if(vm.equipmentLoadOnce){
  //       vm.$store.dispatch('getAllEquipments');
  //       vm.equipmentTypeLoadOnce = false;
  //    }
  //   }
  // },

  methods: {
    detail(payload) {
      const vm = this;

      vm.detailDialog = true;
      vm.detailAction = payload;
    },

    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.operatingRooms.indexOf(payload);
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
      return "getOperatingRooms";
    }
  }
};

</script>
