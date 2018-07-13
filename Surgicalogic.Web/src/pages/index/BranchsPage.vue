<template>
  <div>
    <grid-component :headers="headers"
                    :items="branchs"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :methodName="getMethodName"
                    :totalCount="getTotalCount"
                    :pagination.sync="pagination"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <branchs-edit-component :edit-action="editAction"
                            :edit-visible="editDialog"
                            :edit-index="editedIndex"
                            :delete-value="deleteValue"
                            @cancel="cancel">
    </branchs-edit-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('branchs.branchs'),
      search: '',
      editDialog: false,
      editAction: {},
      deleteValue: {},
       pagination:{},
       totalRowCount:0,
      editedIndex: -1
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('branchs.branch'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'description',
          text: vm.$i18n.t('common.description'),
          sortable: true,
          align: 'left'
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    branchs() {
      const vm = this;

      return vm.$store.state.branchsModule.branchs;
    },


    getTotalCount() {
     const vm = this;

      return vm.$store.state.branchsModule.totalCount;
    }
  },

  methods: {
    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.branchs.indexOf(payload);
      vm.editAction = payload;
    },

    cancel() {
      const vm = this;

      vm.editDialog = false;
    },

    addNewItem(){
      const vm = this;

      vm.editDialog = true;
    },

    deleteItem(payload) {
      const vm = this;

      //We are accessing deleteBranch in vuex store
      vm.$store.dispatch('deleteBranch', {
        id: payload.id
      });

      vm.deleteValue = payload;
    },

    getMethodName(){
     return "getBranchs"
    },

  },

  // created() {
  //   const vm = this;

  //   //We are accessing getBranch in vuex store
  //   vm.$store.dispatch('getBranchs');
  // }
};

</script>
