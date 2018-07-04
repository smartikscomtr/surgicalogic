<template>
  <div>
    <grid-component :headers="headers"
                    :items="operationTypes"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <operation-types-edit-component :editAction="editAction"
                                    :editVisible="editDialog"
                                    :editIndex="editedIndex"
                                    :delete-value="deleteValue"
                                    @cancel="cancel">
    </operation-types-edit-component>
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
      detailAction: {},
      editAction: {},
      deleteValue: {},
      editedIndex: -1
    };
  },

  computed: {
    headers() {
      const vm = this;

      return [
        {
          value: "name",
          text: vm.$i18n.t("operationtypes.operationtype"),
          sortable: true,
          align: "left"
        },
        {
          value: "branchId",
          text: vm.$i18n.t("branchs.branch"),
          sortable: true,
          align: "left"
        },
        {
          isAction: true,
          sortable: false,
          align: "right"
        }
      ];
    },

    operationTypes() {
      const vm = this;

      return vm.$store.state.operationTypeModule.operationTypes;
      // return _each(this.$store.state.operationTypeModule.operationTypes, (item) => item.branchTitle = item.branch[0].name );
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
    },

    addNewItem(){
      const vm = this;

      vm.editDialog = true;

      //Yeni Ekleme
    },

    deleteItem(payload) {
      const vm = this;

        vm.$store.dispatch('deleteOperationType', {
          id: payload.id
        });

      vm.deleteValue = payload;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch("getOperationTypes");
  }
};

</script>
