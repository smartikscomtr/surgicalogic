<template>
  <div>
    <grid-component :headers="headers"
                    :items="workTypes"
                    :title="title"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <work-types-edit-component :actions="actions"
                                    :visible="dialog"
                                    :edit="editedIndex"
                                    :delete-value="deleteValue"
                                    @cancel="cancel">
    </work-types-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('worktypes.workTypes'),
      search: '',
      dialog: false,
      actions: {},
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
          text: vm.$i18n.t("worktypes.workTypes"),
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

    workTypes() {
      const vm = this;

      return vm.$store.state.workTypesModule.workTypes;
    }
  },

  methods: {
    action(payload){
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.workTypes.indexOf(payload);
      vm.actions = payload;
    },

    cancel() {
      const vm = this;

      vm.dialog = false;
    },

    addNewItem(){
      const vm = this;

      vm.dialog = true;

      //Yeni Ekleme
    },

    deleteItem(payload) {
      const vm = this;

      vm.deleteValue = payload;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch('getWorkTypes');
  }
};

</script>
