<template>
  <div>
    <grid-component :headers="headers"
                    :items="branches"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :methodName="getMethodName"
                    :totalCount="getTotalCount"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <branches-edit-component :edit-action="editAction"
                            :edit-visible="editDialog"
                            :edit-index="editedIndex"
                            @cancel="cancel">
    </branches-edit-component>

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
      title: vm.$i18n.t('branches.branches'),
      search: '',
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      totalRowCount:0,
      editedIndex: -1,
      deletePath: 'deleteBranch'
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('branches.branch'),
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

    branches() {
      const vm = this;

      return vm.$store.state.branchesModule.branches;
    },


    getTotalCount() {
     const vm = this;

      return vm.$store.state.branchesModule.totalCount;
    }
  },

  methods: {
    getMethodName(){
     return "getBranches";
    },

    deleteMethodName(){
      return "deleteBranch";
    }
  }
};

</script>
