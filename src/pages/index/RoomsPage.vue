<template>
  <div>
    <grid-component :headers="headers"
                    :items="rooms"
                    :show-edit="true"
                    :show-delete="true"
                    @edited="edited">
    </grid-component>

    <rooms-edit-component :headers="headers"
                          :editedItem="editedItem"
                          :visible="dialog">
    </rooms-edit-component>
  </div>
</template>

<script>

import RoomsEditComponent from '../../components/editcomponents/RoomsEditComponent';

export default {
  data() {
    return {
        dialog: false,
        editedItem : {}
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
      return this.$store.state.rooms;
    }
  },

  methods: {
    edited(payload){
      const vm = this;

      vm.dialog = true;
      vm.editedItem = payload;
    }
  },

  created() {
    this.$store.dispatch("fetchRooms");
  }
};

</script>
