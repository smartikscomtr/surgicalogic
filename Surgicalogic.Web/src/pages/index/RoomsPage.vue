<template>
  <div>
    <grid-component :headers="headers"
                    :items="rooms"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <rooms-edit-component :actions="actions"
                          :visible="dialog"
                          :delete-value="deleteValue">
    </rooms-edit-component>
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
    }
  },

  computed: {
    headers() {
      return [
        {
          value: "room",
          text: "Oda",
          sortable: "true",
          align: "left"
        },
        {
          value: "operationRoom",
          text: "Operasyon Odası",
          sortable: "true",
          align: "left"
        },
        {
          value: "size",
          text: "Ölçüler",
          sortable: "true",
          align: "left"
        }
      ];
    },

    rooms() {
      const vm = this;

      return vm.$store.state.roomModule.rooms;
    }
  },

  methods: {
    action(payload) {
      const vm = this;

      vm.dialog = true;
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

    vm.$store.dispatch("getRooms");
  }
};

</script>
