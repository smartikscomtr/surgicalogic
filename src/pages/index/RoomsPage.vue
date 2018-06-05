<template>
  <div>

    <grid-component :headers="headers"
                    :items="rooms"
                    :show-edit="true"
                    :show-delete="true">
    </grid-component>
  </div>
</template>

<script>

export default {
  data() {
    return {
      desserts: [],
      editedIndex: -1,
      editedItem: {
        name: '',
        calories: 0,
        fat: 0,
        carbs: 0,
        protein: 0
      },
      defaultItem: {
        name: '',
        calories: 0,
        fat: 0,
        carbs: 0,
        protein: 0
      }
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

  created() {
    this.$store.dispatch("fetchRooms");
  }
};

</script>
