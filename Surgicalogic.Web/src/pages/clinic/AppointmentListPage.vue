<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="appointments"
                    :title="title"
                    :show-detail="false"
                    :show-edit="false"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="false"
                    :hide-export="true"
                    :methodName="getMethodName"
                    :custom-parameters="customParameters"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @deleteitem="deleteItem">
    </grid-component>

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
      title: vm.$i18n.t('appointment.futureAppointments'),
      search: 'Ara',
      deleteDialog: false,
      deleteValue: {},
      totalRowCount:0,
      deletePath: 'deleteAppointment',
      customParameters: {}
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'appointmentDate',
          text: vm.$i18n.t('appointment.appointmentDate'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'fullName',
          text: vm.$i18n.t('appointment.fullName'),
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

    appointments() {
      const vm = this;

      return vm.$store.state.appointmentCalendarModule.appointments;
    },


    getTotalCount() {
     const vm = this;

      return  vm.$store.state.appointmentCalendarModule.totalCount;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.appointmentCalendarModule.loading;
    }
  },

  methods: {
    getMethodName(){
     return "getFutureAppointments";
    },

    deleteMethodName(){
      // return "deleteAppointments";
    }
  },

  created() {
     const vm = this;

      vm.customParameters.doctorId = vm.$route.query.doctorId;
  }
};

</script>
