<template>
  <div>
    <grid-component :headers="headers"
                    :items="futurePlans"
                    :title="title"
                    :show-detail="false"
                    :show-edit="false"
                    :show-delete="false"
                    :show-search="false"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <!-- <operation-detail-component :detail-action="detailAction"
                                 :detail-visible="detailDialog"
                                 @cancel="cancel">
    </operation-detail-component> -->

    <!-- <operation-edit-component :edit-action="editAction"
                               :edit-visible="editDialog"
                               :edit-index="editedIndex"
                               @cancel="cancel">
    </operation-edit-component> -->

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
      title: vm.$i18n.t('plan.futurePlan'),
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
      deletePath: ''
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'operatingRoomName',
          sortBy: 'OperatingRoom.Name',
          text: vm.$i18n.t('plan.roomName'),
          sortable: false,
          align: 'left'
         },
        {
          value:'operationName',
          sortBy:'Operation.Name',
          text: vm.$i18n.t('plan.operationName'),
          sortable: false,
          align: 'left'
        },
        {
          value: 'operationDate',
          text: vm.$i18n.t('plan.operationDate'),
          sortable: false,
          isDateTime: true,
          align: 'left'
        },
        // {
        //   value: 'operationTime',
        //   text: vm.$i18n.t('operation.operationTime'),
        //   sortable: true,
        //   align: 'left'
        // },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    futurePlans() {
      const vm = this;

      return vm.$store.state.planModule.plan;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.planModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.planModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllOperationTypes in vuex store
     if(vm.editLoadOnce){
        vm.$store.dispatch('getAllOperationTypes');
        vm.editLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getOperationPlans";
    },

    deleteMethodName(){
      return "";
    }
  }
};

</script>
