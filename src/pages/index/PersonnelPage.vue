<template>
    <div>
      <grid-component :headers="headers"
                      :items="personnels"
                      :show-edit="true"
                      :show-delete="true">
      </grid-component>
    </div>
</template>

<script>

export default {
  data() {
    return {};
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

  created() {
    this.$store.dispatch("fetchPersonnel");
  }
};
</script>
