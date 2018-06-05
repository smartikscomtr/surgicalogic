<template>
    <div>
      <grid-component :headers="headers"
                      :items="personnels"
                      :show-edit="true"
                      :show-delete="true"
                      @edited="edited">
      </grid-component>

      <personnel-edit-component :headers="headers"
                                :editedItem="editedItem"
                                :visible="dialog">
      </personnel-edit-component>
    </div>
</template>

<script>

import PersonnelEditComponent from '../../components/editcomponents/PersonnelEditComponent';

export default {
  data() {
    return {
        dialog: false,
        editedItem : {}
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

      return vm.$store.state.personnel;
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
    this.$store.dispatch("fetchPersonnel");
  }
};
</script>
