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
      editedIndex: -1
      // workTypeTitle: []
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
          value: "firstName",
          text: vm.$i18n.t("personnel.givenName"),
          sortable: true,
          align: "left"
        },
        {
          value: "lastName",
          text: vm.$i18n.t("personnel.familyName"),
          sortable: true,
          align: "left"
        },
        {
          value: "personnelTitleId",
          text: vm.$i18n.t("personnel.personnelTitle"),
          sortable: true,
          align: "left"
        },
        {
          value: "branchId",
          text: vm.$i18n.t("personnel.branch"),
          sortable: true,
          align: "left"
        },
        {
          value: "workTypeId",
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
      const vm = this;

      return vm.$store.state.personnelModule.personnel;
      // return _each(this.$store.state.personnelModule.personnel, (item) => item.workTypeTitle = item.workTypes[0].name );
    }
  },

  watch: {
   editDialog() {
    const vm = this;

     if(vm.workTypeLoadOnce){
        vm.$store.dispatch('getWorkTypes');
        vm.workTypeLoadOnce = false;
     }

     if(vm.personnelTitleLoadOnce){
        vm.$store.dispatch('getPersonnelTitles');
        vm.personnelTitleLoadOnce = false;
     }

     if(vm.branchLoadOnce){
        vm.$store.dispatch('getBranchs');
        vm.branchLoadOnce = false;
     }
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

    vm.$store.dispatch("getPersonnels");
  }
};

</script>
