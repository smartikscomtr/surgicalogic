<template>
  <div>
    <grid-component :headers="headers"
                    :items="personnelTitles"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <personnel-title-edit-component :edit-action="editAction"
                                    :edit-visible="editDialog"
                                    :edit-index="editedIndex"
                                    @cancel="cancel">
    </personnel-title-edit-component>

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
      title: vm.$i18n.t('personneltitle.personnelTitles'),
      search: '',
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},

      editedIndex: -1,
      totalRowCount:0,
      deletePath: 'deletePersonnelTitle'
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('personneltitle.personnelTitles'),
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

    personnelTitles() {
      const vm = this;

      return vm.$store.state.personnelTitleModule.personnelTitle;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.personnelTitleModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.personnelTitleModule.totalCount;
    }
  },

  methods: {
    getMethodName(){
      return "getPersonnelTitles";
    },

    deleteMethodName(){
      return "deletePersonnelTitle";
    }
  }
};

</script>
