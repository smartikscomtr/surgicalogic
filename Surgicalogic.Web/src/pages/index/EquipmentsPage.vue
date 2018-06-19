<template>
  <div>
    <grid-component :headers="headers"
                    :items="equipments"
                    :title="title"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <equipments-edit-component :actions="actions"
                               :visible="dialog"
                               :edit="editedIndex"
                               :delete-value="deleteValue">
    </equipments-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('equipments.equipments'),
      search: '',
      dialog: false,
      actions : {},
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
          text: vm.$i18n.t("equipments.name"),
          sortable: true,
          align: "left"
        },
        {
          value: "type",
          text: vm.$i18n.t("equipments.type"),
          sortable: true,
          align: "left"
        },
        {
          value: "portable",
          text: vm.$i18n.t("equipments.portable"),
          sortable: true,
          align: "left"
        },
        {
          value: "description",
          text: vm.$i18n.t("equipments.description"),
          sortable: true,
          align: "left"
        },
        {
          text: vm.$i18n.t("common.actions"),
          sortable: false,
          isAction: true
        }
      ];
    },

    equipments() {
      const vm = this;

      return vm.$store.state.equipmentModule.equipments;
    }
  },

  methods: {
    action(payload){
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.equipments.indexOf(payload);
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

    vm.$store.dispatch('getEquipments');
  }
};

</script>
