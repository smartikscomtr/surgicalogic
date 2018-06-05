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
                      :items="personnels"
                      :show-edit="true"
                      :show-delete="true"
                      @action="action">
      </grid-component>

      <personnel-edit-component :columns="columns"
                                :actions="actions"
                                :visible="dialog">
      </personnel-edit-component>
    </div>
</template>

<script>

import PersonnelEditComponent from "../../components/editcomponents/PersonnelEditComponent";

export default {
  data() {
    return {
      dialog: false,
      actions: {}
    };
  },

  computed: {
    columns() {
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

      return vm.$store.state.personnel;
    }
  },

  methods: {
    action(payload) {
      const vm = this;

      vm.dialog = true;
      vm.actions = payload;
    }
  },

  created() {
    const vm = this;

    vm.$store.dispatch("fetchPersonnel");
  }
};

</script>
