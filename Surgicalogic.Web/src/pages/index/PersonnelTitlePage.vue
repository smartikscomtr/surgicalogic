<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnelTitles"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-title-edit-component :editAction="editAction"
                                    :editVisible="editDialog"
                                    :editIndex="editedIndex"
                                    :delete-value="deleteValue"
                                    @cancel="cancel">
    </personnel-title-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('personneltitle.personnelTitles'),
      search: '',
      editDialog: false,
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
          text: vm.$i18n.t("personneltitle.personnelTitles"),
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

    personnelTitles() {
      const vm = this;

      return vm.$store.state.personnelTitleModule.personnelTitle;
    }
  },

  methods: {
    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.personnelTitles.indexOf(payload);
      vm.editAction = payload;
    },

    cancel() {
      const vm = this;

      vm.editDialog = false;
    },

    addNewItem(){
      const vm = this;

      vm.editDialog = true;

      //Yeni Ekleme
    },

    deleteItem(payload) {
      const vm = this;

        vm.$store.dispatch('deletePersonnelTitle', {
          id: payload.id
        });

      vm.deleteValue = payload;
    }
  },
  created() {
    const vm = this;

    vm.$store.dispatch('getPersonnelTitles');
  }
};

</script>
