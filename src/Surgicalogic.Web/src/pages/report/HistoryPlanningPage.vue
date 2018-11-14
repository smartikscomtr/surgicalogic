<template>
  <div class="container fluid grid-list-md">
    <div class="grid-card v-card">
      <v-form ref="form" v-model="valid" lazy-validation>
        <div class="v-card__text layout row wrap">
          <v-flex xs12 sm12 md12>
            <div class="btn-wrap">
              <v-btn class="btnSave orange"
                    lang="tr"
                    @click.native="filteredReport">
                {{ $t('report.filter') }}
              </v-btn>
            </div>

            <div class="btn-wrap">
              <v-btn class="btnSave orange"
                    lang="tr"
                    @click.native="clearReport">
                {{ $t('report.clear') }}
              </v-btn>
            </div>
          </v-flex>

          <v-flex xs12 sm12 md12>
            <v-autocomplete v-model="selectOperation"
                            :items="operations"
                            :label="$t('operation.operationName')"
                            clearable
                            multiple
                            chips
                            deletable-chips
                            box
                            :filter="customFilter"
                            item-text="name"
                            item-value="id">
            </v-autocomplete>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-autocomplete v-model="selectOperatingRoom"
                            :items="operatingRooms"
                            :label="$t('operatingrooms.operatingRoom')"
                            box
                            clearable
                            :filter="customFilter"
                            item-text="name"
                            item-value="id">
            </v-autocomplete>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-text-field v-model="identityNumber"
                          :label="$t('patient.identityNumber')">
            </v-text-field>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-menu ref="menu1"
                    :close-on-content-click="false"
                    v-model="menu1"
                    :nudge-right="40"
                    lazy
                    :return-value.sync="startDate"
                    transition="scale-transition"
                    offset-y full-width min-width="290px">
              <v-text-field readonly slot="activator"
                            v-model="startDateFormatted"
                            clearable
                            :label="$t('report.operationStartDate')">
              </v-text-field>

              <v-date-picker v-model="startDate" no-title @input="$refs.menu1.save(startDate)" :max="getMaxDate()">
              </v-date-picker>
            </v-menu>
          </v-flex>

          <v-flex xs12 sm6 md6>
            <v-menu ref="menu2"
                    :close-on-content-click="false"
                    v-model="menu2"
                    :nudge-right="40"
                    lazy
                    :return-value.sync="endDate"
                    transition="scale-transition"
                    offset-y full-width min-width="290px">
              <v-text-field readonly
                            clearable slot="activator"
                            v-model="endDateFormatted"
                            :label="$t('report.operationEndDate')">
              </v-text-field>

              <v-date-picker v-model="endDate"
                            no-title
                            @input="$refs.menu2.save(endDate)"
                            :max="getMaxDate()">
              </v-date-picker>
            </v-menu>
          </v-flex>
        </div>
      </v-form>
    </div>

    <grid-component :headers="headers"
                    :items="historyPlannings"
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
                    @exportToExcel="exportHistoryPlanningReportToExcel"
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
      operatingRoomId: null,
      operationId: null,
      menu1: false,
      menu2: false,
      startDate: null,
      endDate: null,
      dateFormatted: null,
      startDateFormatted: null,
      endDateFormatted: null,
      customParameters: {},
      identityNumber: null,
      valid: true
    };
  },

  watch: {
    startDate(val) {
      const vm = this;

      vm.startDateFormatted = vm.formatDate(vm.startDate);
    },

    endDate(val) {
      const vm = this;

      vm.endDateFormatted = vm.formatDate(vm.endDate);
    }
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('plan.planningHistory');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'operationName',
          sortBy: 'Operation.Name',
          text: vm.$i18n.t('plan.operationName'),
          sortable: true,
          align: 'left'
        },
        {
          value:'patientIdentityNumber',
          text: vm.$i18n.t('operation.patientIdentityNumber'),
          sortable: true,
          align: 'left'
        },
        {
          value:'operatingRoomName',
          sortBy:'OperatingRoom.Name',
          text: vm.$i18n.t('plan.operationRoomName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'operationDate',
          sortBy:'OperationDate',
          text: vm.$i18n.t('plan.operationDate'),
          isDateTime: true,
          sortable: true,
          align: 'left'
        },
        {
          value: 'realizedStartDate',
          text: vm.$i18n.t('plan.realizedStartDate'),
          isDateTime: true,
          sortable: false,
          align: 'left'
        },
        {
          value: 'realizedEndDate',
          text: vm.$i18n.t('plan.realizedEndDate'),
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

    historyPlannings() {
      const vm = this;

      return vm.$store.state.reportsModule.plan;
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
    },

    selectOperation: {
      get() {
        const vm = this;

        vm.operationId;
      },

      set(val) {
        const vm = this;

        vm.operationId = val;
      }
    },

    operations() {
      const vm = this;

      return vm.$store.state.operationModule.allOperations;
    }
  },

  methods: {
    getMethodName(){
      return "getOperationPlanHistory";
    },

    deleteMethodName(){
      return "";
    },

    exportPlanningHistoryToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportPlanningHistory');
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

      vm.customParameters.operationId = vm.operationId;
      vm.customParameters.operatingRoomId = vm.operatingRoomId;
      vm.customParameters.operationStartDate = vm.startDate;
      vm.customParameters.operationEndDate = vm.endDate;
      vm.customParameters.identityNumber = vm.identityNumber;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    },

    getMaxDate() {
      const toTwoDigits = num => (num < 10 ? '0' + num : num);
      let today = new Date();

      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let day = toTwoDigits(today.getDate());

      return `${year}-${month}-${day}`;
    },

    formatDate(date) {
      if (!date || date.indexOf('.') > -1) return null;

      const [year, month, day] = date.split('-');

      return `${day}.${month}.${year}`;
    },

    exportHistoryPlanningReportToExcel() {
        const vm = this;

        vm.$store.dispatch('excelExportHistoryPlanning', {
          operationId: vm.operationId,
          operatingRoomId: vm.operatingRoomId,
          operationStartDate: vm.startDate,
          operationEndDate: vm.startDate
        });
    },

    clearReport() {
      const vm = this;

      vm.clear();
    },

    clear() {
      const vm = this;

      vm.$refs.form.reset();
    }
  },

  created () {
    const vm = this;

    vm.$store.dispatch('getAllOperatingRooms');
    vm.$store.dispatch('getAllOperations');
    vm.$store.dispatch('getAllPatients');
  }
};

</script>
