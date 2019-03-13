<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="patients"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="true"
                    :show-plan-an-operation="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @edit="edit"
                    @exportToExcel="exportPatientsToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    @planAnOperation="showModule"
                    ref="gridComponent">
    </grid-component>

    <patient-edit-component :edit-action="editAction"
                              :edit-visible="editDialog"
                              :edit-index="editedIndex"
                              @cancel="cancel">
    </patient-edit-component>

    <plan-an-operation-component :plan-operation-action="planOperationAction"
                              :plan-operation-visible="planOperationDialog"
                              :plan-operation-index="plannedOperationIndex"
                              @cancel="cancel">
    </plan-an-operation-component>

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
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      totalRows:0,
      editedIndex: -1,
      planOperationAction: {},
      planOperationDialog : false,
      plannedOperationIndex: -1,
      planLoadOnce: true,
      deletePath: 'deletePatient'
    };
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('patient.patients');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'firstName',
          text: vm.$i18n.t('patient.firstName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'lastName',
          text: vm.$i18n.t('patient.lastName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'identityNumber',
          text: vm.$i18n.t('patient.identityNumber'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'phone',
          text: vm.$i18n.t('patient.phone'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'address',
          text: vm.$i18n.t('patient.address'),
          sortable: true,
          align: "left"
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    patients() {
      const vm = this;

      return vm.$store.state.patientModule.patient;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.patientModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.patientModule.totalCount;
    }
  },

  watch: {
    planOperationDialog() {
      const vm = this;

      //We are accessing getAllOperationTypes in vuex store
      if(vm.planLoadOnce){
          vm.$store.dispatch('getOperationTypesByBranchId');
          vm.planLoadOnce = false;
      }

      }
    },


  methods: {
    getMethodName(){
      return "getPatients";
    },

    deleteMethodName(){
      return "deletePatient";
    },

    exportPatientsToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportPatient');
    },

    showModule(item){
        const vm = this;

        vm.planOperationDialog = true;
        item.fullName = item.firstName + ' ' + item.lastName;
        vm.planOperationAction = item;
    },

    getPatients(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
