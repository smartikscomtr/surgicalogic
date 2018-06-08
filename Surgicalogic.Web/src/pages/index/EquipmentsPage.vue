<template>
  <div>
    <grid-component :headers="headers"
                    :items="equipments"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @deleteitem="deleteItem">
    </grid-component>

    <equipments-edit-component :actions="actions"
                               :visible="dialog"
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
      deleteValue: {}
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
      vm.actions = payload;
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
