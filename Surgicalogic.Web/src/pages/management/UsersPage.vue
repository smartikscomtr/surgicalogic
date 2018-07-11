<template>
  <div>
    <grid-component :headers="headers"
                    :items="users"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <users-edit-component :edit-action="editAction"
                            :edit-visible="editDialog"
                            :edit-index="editedIndex"
                            @cancel="cancel">
    </users-edit-component>

    <delete-component :delete-value="deleteValue"
                      :delete-visible="deleteDialog"
                      @cancel="cancel">
    </delete-component>
  </div>
</template>

<script>

export default {
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('users.users'),
      search: '',
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      editedIndex: -1
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'givenName',
          text: vm.$i18n.t('users.givenName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'familyName',
          text: vm.$i18n.t('users.familyName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'email',
          text: vm.$i18n.t('users.email'),
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

    users() {
      const vm = this;

      return vm.$store.state.usersModule.users;
    }
  },

  methods: {
    edit(payload){
      const vm = this;

      vm.editDialog = true;
      vm.editedIndex = vm.users.indexOf(payload);
      vm.editAction = payload;
    },

    cancel() {
      const vm = this;

      vm.editDialog = false;
      vm.deleteDialog = false;
    },

    addNewItem(){
      const vm = this;

      vm.editDialog = true;
    },

    deleteItem(payload) {
      const vm = this;

      vm.deleteValue = payload;
      vm.deleteDialog = true;
    }
  },

  created() {
    const vm = this;

    //We are accessing getUsers in vuex store
    vm.$store.dispatch('getUsers');
  }
}

</script>
