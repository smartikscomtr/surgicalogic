<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="users"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :show-insert="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @edit="edit"
                    @exportToExcel="exportUserToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    @resetpassword="resetPassword"
                    ref="gridComponent">
    </grid-component>

    <users-edit-component :edit-action="editAction"
                          :edit-visible="editDialog"
                          :edit-index="editedIndex"
                          @cancel="cancel">
    </users-edit-component>

    <delete-component :delete-value="deleteValue"
                      :delete-visible="deleteDialog"
                      :deleteMethode="deleteMethodName"
                      @cancel="cancel">
    </delete-component>
  </div>
</template>

<script>

import { gridMixin } from './../../mixins/gridMixin';

export default {
  mixins: [
    gridMixin
  ],

  data() {
    const vm = this;

    return {
      search: '',
      detailDialog: false,
      editDialog: false,
      deleteDialog: false,
      detailAction: {},
      editAction: {},
      deleteValue: {},
      editedIndex: -1,
      totalRowCount:0,
      editLoadOnce: true,
      deletePath: 'deleteUser'
    };
  },

  computed: {
     title() {
      const vm = this;

      return vm.$i18n.t('users.users');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'userName',
          text: vm.$i18n.t('users.userName'),
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
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.usersModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.usersModule.totalCount;
    }
  },

  methods: {
    getMethodName(){
      return "getUsers";
    },

    deleteMethodName(){
      return "deleteUser";
    },

    exportUserToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportUser', { langId: vm.$cookie.get("currentLanguage")});
    },

    getUsers(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
}

</script>
