<template>
    <div>
      <grid-component :headers="headers"
                      :items="equipments"

                      :show-edit="true"
                      :show-delete="true"
                      @edited="edited">
      </grid-component>
      <edit-page-component :headers="headers"
                             :editedItem="editedItem"
                             :visible="showEditPageComponent">
        </edit-page-component>
    </div>
</template>

<script>

import EditPageComponent from '../../components/EditPageComponent';

export default {
    data() {
      return {
        showEditPageComponent: false,
        editedItem : {}
      };
    },

    computed: {
      // editedItem() {
      //   return
      //     {
      //       name = '',
      //       type = '',
      //       portable = '',
      //       description = ''
      //     };
      // },

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
        this.showEditPageComponent = true;
        this.editedItem = payload;
      }
    },

    created() {
      const vm = this;
      vm.$store.dispatch('fetchEquipments');
    }
}

</script>
