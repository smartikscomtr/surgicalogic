<template>
  <div>
    <grid-component :headers="headers"
                    :items="rooms"
                    :title="title"
                    :show-edit="true"
                    :show-delete="true"
                    @action="action"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <rooms-edit-component :actions="actions"
                          :visible="dialog"
                          :edit="editedIndex"
                          :delete-value="deleteValue"
                          @cancel="cancel">
    </rooms-edit-component>
  </div>
</template>

<script>

import _each from 'lodash/each';

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('rooms.rooms'),
      search: '',
      dialog: false,
      actions : {},
      deleteValue: {},
      editedIndex: -1,
      equipmentTitle: []
    }
  },

  computed: {
    headers() {
      const vm = this;

      return [
        {
          value: "room",
          text: vm.$i18n.t("rooms.room"),
          sortable: true,
          align: "left"
        },
        {
          value: "location",
          text: vm.$i18n.t("rooms.location"),
          sortable: true,
          align: "left"
        },
        {
          value: "size",
          text: vm.$i18n.t("rooms.size"),
          sortable: true,
          align: "left"
        },
        {
          value: "equipmentTitle",
          text: vm.$i18n.t("equipments.equipments"),
          sortable: true,
          align: "left"
        },
        {
          isAction: true,
          sortable: false,
          align: "right"
        }
      ];
    },

    rooms() {
      return _each(this.$store.state.roomModule.rooms, (item) => item.equipmentTitle = item.equipments[0].name );
    }
  },

  methods: {
    action(payload) {
      const vm = this;

      vm.dialog = true;
      vm.editedIndex = vm.rooms.indexOf(payload);
      vm.actions = payload;
    },

    addNewItem(){
      const vm = this;

      vm.dialog = true;

      //Yeni Ekleme
    },

    cancel() {
      const vm = this;

      vm.dialog = false;
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
