<template>
  <div>
    <grid-component :headers="headers"
                    :items="branchs"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <branchs-detail-component :detailAction="detailAction"
                              :detailVisible="detailDialog"
                              @cancel="cancel">
    </branchs-detail-component>

    <branchs-edit-component :editAction="editAction"
                            :editVisible="editDialog"
                            :editIndex="editedIndex"
                            :delete-value="deleteValue"
                            @cancel="cancel">
    </branchs-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('branchs.branchs'),
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
          text: vm.$i18n.t("branchs.branch"),
          sortable: true,
          align: "left"
        },
        {
          value: "description",
          text: vm.$i18n.t("common.description"),
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

    branchs() {
      const vm = this;

      return vm.$store.state.branchsModule.branchs;
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
      vm.editedIndex = vm.branchs.indexOf(payload);
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

        vm.$store.dispatch('deleteBranch', {
          id: payload.id
        });

      vm.deleteValue = payload;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch('getBranchs');
  }
};

</script>
