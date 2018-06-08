<template>
  <div>
    <grid-component :headers="headers"
                    :items="equipments"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <equipments-edit-component :actions="actions"
                               :visible="dialog"
                               :title="editedIndex"
                               :delete-value="deleteValue">
    </equipments-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    return {
      search: '',
      dialog: false,
      actions : {},
      deleteValue: {},
      editedIndex: -1
    };
  },

  computed: {
    headers() {
      return [
        {
          value: "name",
          text: "Adı",
          sortable: 'true',
          align: "left"
        },
        {
          value: "type",
          text: "Tipi",
          sortable: 'true',
          align: "left"
        },
        {
          value: "portable",
          text: "Taşınabilirlilik",
          sortable: 'true',
          align: "left"
        },
        {
          value: "description",
          text: "Açıklama",
          sortable: 'true',
          align: "left"
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
