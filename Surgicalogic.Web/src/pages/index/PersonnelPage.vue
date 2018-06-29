<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnels"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-edit-component :editAction="editAction"
                              :editVisible="editDialog"
                              :editIndex="editedIndex"
                              :delete-value="deleteValue"
                              @cancel="cancel">
    </personnel-edit-component>
  </div>
</template>

<script>

import _each from 'lodash/each';

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('personnel.personnels'),
      search: '',
      detailDialog: false,
      editDialog: false,
      detailAction: {},
      editAction: {},
      deleteValue: {},
      editedIndex: -1,
      workTypeTitle: []
    };
  },

  computed: {
    headers() {
      const vm = this;

      return [
        {
          value: "personnelCode",
          text: vm.$i18n.t("personnel.personnelCode"),
          sortable: true,
          align: "left"
        },
        {
          value: "givenName",
          text: vm.$i18n.t("personnel.givenName"),
          sortable: true,
          align: "left"
        },
        {
          value: "familyName",
          text: vm.$i18n.t("personnel.familyName"),
          sortable: true,
          align: "left"
        },
        {
          value: "task",
          text: vm.$i18n.t("personnel.task"),
          sortable: true,
          align: "left"
        },
        {
          value: "branch",
          text: vm.$i18n.t("personnel.branch"),
          sortable: true,
          align: "left"
        },
        {
          value: "shift",
          text: vm.$i18n.t("personnel.shift"),
          sortable: true,
          align: "left"
        },
        {
          value: "workTypeTitle",
          text: vm.$i18n.t("personnel.workType"),
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

    personnels() {
      return _each(this.$store.state.personnelModule.personnel, (item) => item.workTypeTitle = item.workTypes[0].name );
    }
  },

  methods: {
    detail(payload) {
      const vm = this;

      vm.detailDialog = true;
      vm.detailAction = payload;
    },

    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.personnel.indexOf(payload);
      vm.editAction = payload;
    },

    cancel() {
      const vm = this;

      vm.detailDialog = false;
      vm.editDialog = false;
    },

    addNewItem(){
      const vm = this;

      vm.editDialog = true;

      //Yeni Ekleme
    },

    deleteItem(payload) {
      const vm = this;

        vm.$store.dispatch('deletePersonnel', {
          id: payload.id
        });
      //vm.deleteValue = payload;
    }
  },
  created() {
    const vm = this;

    vm.$store.dispatch("getPersonnel");
  }
};

</script>
