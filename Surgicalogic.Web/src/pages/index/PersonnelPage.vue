<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnels"
                    :title="title"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-edit-component :actions="actions"
                              :visible="dialog"
                              :edit="editedIndex"
                              :delete-value="deleteValue">
    </personnel-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('personnel.personnels'),
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
          value: "personnelCode",
          text: vm.$i18n.t("personnel.personnelCode"),
          sortable: true,
          align: "left"
        },
        {
          value: "givenName",
          text: vm.$i18n.t("personnel.givenName"),
          sortable: true,
          align: "left"
        },
        {
          value: "familyName",
          text: vm.$i18n.t("personnel.familyName"),
          sortable: true,
          align: "left"
        },
        {
          value: "task",
          text: vm.$i18n.t("personnel.task"),
          sortable: true,
          align: "left"
        },
        {
          value: "branch",
          text: vm.$i18n.t("personnel.branch"),
          sortable: true,
          align: "left"
        },
        {
          value: "shift",
          text: vm.$i18n.t("personnel.shift"),
          sortable: true,
          align: "left"
        },
        {
          value: "workType",
          text: vm.$i18n.t("personnel.workType"),
          sortable: true,
          align: "left"
        }
      ];
    },

    personnels() {
      const vm = this;

      return vm.$store.state.personnelModule.personnel;
    }
  },

  methods: {
    action(payload) {
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.personnels.indexOf(payload);
      vm.actions = payload;
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

    vm.$store.dispatch("getPersonnel");
  }
};

</script>
