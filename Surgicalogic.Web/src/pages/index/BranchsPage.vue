<template>
  <div>
    <grid-component :headers="headers"
                    :items="branchs"
                    :title="title"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <branchs-edit-component :actions="actions"
                            :visible="dialog"
                            :edit="editedIndex"
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
    action(payload){
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.branchs.indexOf(payload);
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

    vm.$store.dispatch('getBranchs');
  }
};

</script>
