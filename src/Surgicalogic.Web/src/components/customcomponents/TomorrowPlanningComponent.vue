<template>
  <div>
    <grid-component :headers="headers"
                    :items="tomorrowList"
                    :title="title"
                    :show-detail="false"
                    :show-edit="false"
                    :show-delete="false"
                    :show-search="false"
                    :show-insert="false"
                    :hide-actions="false"
                    :hide-export="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    :custom-parameters="customParameters"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>
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
      deletePath: '',
      customParameters: {}
    };
  },

  computed: {
     title() {
      const vm = this;

      return vm.$i18n.t('plan.operations');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'operationName',
          sortBy: 'Operation.Name',
          text: vm.$i18n.t('plan.operationName'),
          sortable: false,
          align: 'left'
         },
        {
          value:'operatingRoomName',
          sortBy:'OperatingRoom.Name',
          text: vm.$i18n.t('plan.roomName'),
          sortable: false,
          align: 'left'
        },
        {
          value: 'operationStartDate',
          text: vm.$i18n.t('plan.operationStartDate'),
          isDateTime: true,
          sortable: false,
          align: 'left'
        },
        {
          value: 'operationEndDate',
          text: vm.$i18n.t('plan.operationEndDate'),
          isDateTime: true,
          sortable: false,
          align: 'left'
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    tomorrowList() {
      const vm = this;

      return vm.$store.state.planArrangementsModule.tomorrowList;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.planArrangementsModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.planArrangementsModule.tomorrowListTotalCount;
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
      return "getOperationListByDate";
    },

    deleteMethodName(){
      return "";
    },

    getOperations(datetime) {
      const vm = this;

      vm.customParameters.operationDate = datetime;
      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    },

    formatDate() {
      const vm = this;

      const toTwoDigits = num => (num < 10 ? '0' + num : num);
      let tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);

      let year = tomorrow.getFullYear();
      let month = toTwoDigits(tomorrow.getMonth() + 1);
      let day = toTwoDigits(tomorrow.getDate());

      return `${year}-${month}-${day}`;
    }
  },

  mounted() {
    const vm = this;

    vm.customParameters.operationDate = vm.formatDate()
  },

  created () {
    const vm = this;

    vm.customParameters.operationDate = vm.formatDate()
  }
};

</script>
