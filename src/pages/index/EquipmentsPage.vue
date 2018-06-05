<template>
    <div>
      <grid-component :headers="headers"
                      :items="equipments"
                      :show-edit="true"
                      :show-delete="true"
                      @edited="edited">
      </grid-component>

      <equipments-edit-component :headers="headers"
                                 :editedItem="editedItem"
                                 :visible="dialog">
      </equipments-edit-component>
    </div>
</template>

<script>

import EquipmentsEditComponent from '../../components/editcomponents/EquipmentsEditComponent';

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

        return vm.$store.state.equipments;
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
      const vm = this;

      vm.$store.dispatch('fetchEquipments');
    }
}

</script>
