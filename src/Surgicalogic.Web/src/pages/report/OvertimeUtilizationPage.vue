<template>
  <div class="container fluid grid-list-md">
    <div class="grid-card v-card">
      <div class="v-card__text layout row wrap">
        <v-flex xs12 sm12 md12>
          <div class="btn-wrap">
            <v-btn class="btnSave orange"
                   lang="tr"
                   @click.native="filteredReport">
              {{ $t('report.filter') }}
            </v-btn>
          </div>
        </v-flex>

        <v-flex xs12 sm6 md6>
          <v-autocomplete v-model="selectOperatingRoom"
                          :items="operatingRooms"
                          :label="$t('report.operatingRoom')"
                          clearable
                          box
                          :filter="customFilter"
                          item-text="name"
                          item-value="id">
          </v-autocomplete>
        </v-flex>
      </div>
    </div>

    <grid-component :headers="headers"
                    :items="overtimeUtilizations"
                    :title="title"
                    :show-detail="false"
                    :show-edit="false"
                    :show-delete="false"
                    :show-search="true"
                    :show-insert="false"
                    :hide-actions="false"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    :custom-parameters="customParameters"
                    @detail="detail"
                    @exportToExcel="exportOvertimeUtilizationReportToExcel"
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
      customParameters: {},
      operatingRoomId: null
    };
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('report.overtimeUtilizationReport');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'operatingRoom',
          text: vm.$i18n.t('report.operatingRoom'),
          sortBy:'OperatingRoom',
          sortable: true,
          align: 'left'
        },
        {
          value:'overtime',
          sortBy: 'Overtime',
          text: vm.$i18n.t('report.overTime'),
          sortBy:'Overtime',
          sortable: true,
          align: 'left'
        },
        {
          value:'utilization',
          text: vm.$i18n.t('report.utilization'),
          sortBy:'Utilization',
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

    overtimeUtilizations() {
      const vm = this;

      return vm.$store.state.reportsModule.overtimeUtilization;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.reportsModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.reportsModule.totalCount;
    },

    selectOperatingRoom: {
      get() {
        const vm = this;

        vm.operatingRoomId;
      },

      set(val) {
        const vm = this;

        vm.operatingRoomId = val;
      }
    },

    operatingRooms() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.allOperatingRooms;
    }
  },

  methods: {
    getMethodName(){
      return "getOvertimeUtilization";
    },

    deleteMethodName(){
      return "";
    },

    customFilter(item, queryText, itemText) {
      const vm = this;

      const text = vm.replaceForAutoComplete(item.name);
      const searchText = vm.replaceForAutoComplete(queryText);

      return text.indexOf(searchText) > -1;
    },

    replaceForAutoComplete(text) {
      return text.replace(/İ/g, 'i')
                 .replace(/I/g, 'ı')
                 .toLowerCase();
    },

    filteredReport() {
      const vm = this;

      vm.customParameters.operatingRoomId = vm.operatingRoomId;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    },

    exportOvertimeUtilizationReportToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOvertimeUtilization', {
        operatingRoomId: vm.operatingRoomId
      });
    }
  },

  created () {
    const vm = this;

    vm.$store.dispatch('getAllOperatingRooms');
  }
};

</script>
