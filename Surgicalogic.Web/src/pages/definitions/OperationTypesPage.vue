<template>
  <div>
    <grid-component :headers="headers"
                    :items="operationTypes"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

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
      editDialog: false,
      deleteDialog: false,
      detailAction: {},
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
          text: vm.$i18n.t('operationtypes.operationtype'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'branchId',
          text: vm.$i18n.t('branchs.branch'),
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
    }
  },

  methods: {
    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.operationTypes.indexOf(payload);
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

    //We are accessing getBranchs and getOperationTypes in vuex store
    vm.$store.dispatch('getBranchs');
    vm.$store.dispatch('getOperationTypes');
  }
};

</script>
