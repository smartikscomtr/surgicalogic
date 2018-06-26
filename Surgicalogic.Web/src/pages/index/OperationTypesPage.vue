<template>
  <div>
    <grid-component :headers="headers"
                    :items="operationTypes"
                    :title="title"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <operation-types-edit-component :actions="actions"
                                    :visible="dialog"
                                    :edit="editedIndex"
                                    :delete-value="deleteValue"
                                    @cancel="cancel">
    </operation-types-edit-component>
  </div>
</template>

<script>

import _each from 'lodash/each';

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('operationtypes.operationtypes'),
      search: '',
      dialog: false,
      actions: {},
      deleteValue: {},
      editedIndex: -1,
      branchTitle: []
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
          value: "branchTitle",
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
      return _each(this.$store.state.operationTypeModule.operationTypes, (item) => item.branchTitle = item.branch[0].name );
    }
  },

  methods: {
    action(payload) {
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.operationTypes.indexOf(payload);
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

    vm.$store.dispatch("getOperationTypes");
  }
};

</script>
