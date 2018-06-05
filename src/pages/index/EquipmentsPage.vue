<template>
    <div>
      <grid-component :columns="columns"
                      :items="equipments"
                      :show-edit="true"
                      :show-delete="true"
                      @action="action">
      </grid-component>

      <equipments-edit-component :columns="columns"
                                 :actions="actions"
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
        actions : {}
      };
    },

    computed: {
      columns() {
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
      action(payload){
        const vm = this;

        vm.dialog = true;
        vm.actions = payload;
      }
    },

    created() {
      const vm = this;

      vm.$store.dispatch('fetchEquipments');
    }
}

</script>
