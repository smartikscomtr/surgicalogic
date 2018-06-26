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
                               :delete-value="deleteValue"
                               @cancel="cancel">
    </equipments-edit-component>
  </div>
</template>

<script>

import _each from 'lodash/each';

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('equipments.equipments'),
      search: '',
      dialog: false,
      actions: {},
      deleteValue: {},
      editedIndex: -1,
      equipmentTypeTitle: []
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
          value: "equipmentTypeTitle",
          text: vm.$i18n.t("equipmenttypes.equipmentType"),
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
          isAction: true,
          sortable: false,
          align: "right"
        }
      ];
    },

    equipments() {
      return _each(this.$store.state.equipmentModule.equipments, (item) => item.equipmentTypeTitle = item.equipmentTypes[0].name );
    }
  },

  methods: {
    action(payload){
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.equipments.indexOf(payload);
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

    vm.$store.dispatch('getEquipments');
  }
};

</script>
