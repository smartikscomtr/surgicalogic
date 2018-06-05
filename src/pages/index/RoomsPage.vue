<template>
  <div>
    <v-card-title>
      <v-text-field v-model="search"
                    append-icon="search"
                    label="Search"
                    single-line
                    hide-details>
      </v-text-field>

      <v-btn icon
             slot="activator"
             class="mb-2">
        <v-icon color="teal">
                add
        </v-icon>
      </v-btn>
    </v-card-title>

    <grid-component :columns="columns"
                    :items="rooms"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action">
    </grid-component>

    <rooms-edit-component :columns="columns"
                          :actions="actions"
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
      actions : {}
    }
  },

  computed: {
    columns() {
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

      return vm.$store.state.rooms;
    }
  },

  methods: {
    action(payload){
      const vm = this;

      vm.dialog = true;
      vm.actions = payload;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch("fetchRooms");
  }
};

</script>
