<template>
  <div>
    <grid-component :headers="headers"
                    :items="equipmentTypes"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <equipment-types-edit-component :edit-action="editAction"
                                    :edit-visible="editDialog"
                                    :edit-index="editedIndex"
                                    @cancel="cancel">
    </equipment-types-edit-component>

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
      title: vm.$i18n.t('equipmenttypes.equipmentTypes'),
      search: '',
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      editedIndex: -1
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('equipmenttypes.equipmentTypes'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'description',
          text: vm.$i18n.t('common.description'),
          sortable: true,
          align: "left"
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    equipmentTypes() {
      const vm = this;

      return vm.$store.state.equipmentTypesModule.equipmentTypes;
    }
  },

  methods: {
    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.equipmentTypes.indexOf(payload);
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
    }
  },

  created() {
    const vm = this;

    //We are accessing getEquipmentTypes in vuex store
    vm.$store.dispatch('getEquipmentTypes');
  }
};

</script>
