<template>
  <div>
    <grid-component :headers="headers"
                    :items="equipmentTypes"
                    :title="title"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <equipment-types-edit-component :actions="actions"
                            :visible="dialog"
                            :edit="editedIndex"
                            :delete-value="deleteValue"
                            @cancel="cancel">
    </equipment-types-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('equipmenttypes.equipmentTypes'),
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
          text: vm.$i18n.t("equipmenttypes.equipmentTypes"),
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

    equipmentTypes() {
      const vm = this;

      return vm.$store.state.equipmentTypesModule.equipmentTypes;
    }
  },

  methods: {
    action(payload){
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.equipmentTypes.indexOf(payload);
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
      vm.$store.dispatch('deleteEquipmentType', {
        id: payload.id
      });
      //vm.deleteValue = payload;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch('getEquipmentTypes');
  }
};

</script>
