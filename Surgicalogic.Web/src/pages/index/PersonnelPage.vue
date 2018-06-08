<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnels"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-edit-component :actions="actions"
                              :visible="dialog"
                              :delete-value="deleteValue">
    </personnel-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    return {
      search: '',
      dialog: false,
      actions: {},
      deleteValue: {}
    };
  },

  computed: {
    headers() {
      return [
        {
          value: "personnelCode",
          text: "Personel Kodu",
          sortable: true,
          align: "left"
        },
        {
          value: "givenName",
          text: "Adı",
          sortable: true,
          align: "left"
        },
        {
          value: "familyName",
          text: "Soyadı",
          sortable: true,
          align: "left"
        },
        {
          value: "tasks",
          text: "Görevi",
          sortable: true,
          align: "left"
        },
        {
          value: "branch",
          text: "Branşı",
          sortable: true,
          align: "left"
        },
        {
          value: "shift",
          text: "Vardiya",
          sortable: true,
          align: "left"
        },
        {
          value: "workType",
          text: "Çalışma Tipi",
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
      vm.actions = payload;
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
