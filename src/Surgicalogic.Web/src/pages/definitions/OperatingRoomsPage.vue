<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="operatingRooms"
                    :title="title"
                    :show-calendar="true"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="true"
                    :loading="getLoading"
                    :methodName="getMethodName"
                    :totalCount="getTotalCount"
                    @calendar="calendar"
                    @detail="detail"
                    @edit="edit"
                    @exportToExcel="exportOperatingRoomToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem"
                    ref="gridComponent">
    </grid-component>

    <!-- <operating-rooms-calendar-component :calendar-action="calendarAction"
                                       :calendar-visible="calendarDialog"
                                       @cancel="cancel">
    </operating-rooms-calendar-component> -->

    <operating-rooms-detail-component :detail-action="detailAction"
                                       :detail-visible="detailDialog"
                                       @cancel="cancel">
    </operating-rooms-detail-component>

    <operating-rooms-edit-component :edit-action="editAction"
                                    :edit-visible="editDialog"
                                    :edit-index="editedIndex"
                                    @cancel="cancel">
    </operating-rooms-edit-component>

    <delete-component :delete-value="deleteValue"
                      :delete-visible="deleteDialog"
                      :deleteMethode="deleteMethodName"
                      @cancel="cancel">
    </delete-component>

        <!-- Uygunluk Takvimi -->
<div id="calendarElement">
        <grid-component :headers="availabilityHeaders"
                    :items="operatingRoomCalendar"
                    :title="availabilityTitle"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="false"
                    :show-insert="true"
                    :hide-actions="true"
                    :closable="true"
                    :methodName="getAvailableMethodName"
                    :loading="getLoading"
                    @edit="editAvailability"
                    @exportToExcel="exportOperatingRoomCalendarToExcel"
                    @newaction="addNewCalendarItem"
                    @deleteitem="deleteCalendarItem"
                    @closeGrid="showAvailability = false"
                    v-if="showAvailability"
                    ref="gridAvailabilityComponent">
    </grid-component>

     <operating-rooms-calendar-edit-component :edit-available-action="editAvailableAction"
                        :edit-available-visible="editAvailableDialog"
                        :edit-available-index="editedAvailableIndex"
                        :edit-operating-room-id="editOperatingRoomId"
                        @refresh="getOperatingRooms"
                        @cancel="cancel">
    </operating-rooms-calendar-edit-component>

    <delete-component :delete-value="deleteAvailabilityValue"
                      :delete-visible="deleteAvailabilityDialog"
                      :deleteMethode="deleteAvailabilityMethodName"
                      @cancel="cancel">
    </delete-component>
</div>
  </div>
</template>

<script>

import { gridMixin } from './../../mixins/gridMixin';
var VueScrollTo = require('vue-scrollto');

export default {
  mixins: [
    gridMixin
  ],

  data() {
    const vm = this;

    return {
      search: '',
      calendarDialog:false,
      detailDialog: false,
      editDialog: false,
      deleteDialog: false,
      deleteAvailabilityDialog: false,
      calendarAction:{},
      detailAction: {},
      editAction: {
        operatingRoomEquipments : [],
        operatingRoomOperationTypes:[]
      },
      deleteValue: {},
      deleteAvailabilityValue: {},
      editedIndex: -1,
      totalRowCount:0,
      editLoadOnce: true,
      equipmentName: null,
      deletePath: 'deleteOperatingRoom',
      deleteAvailabilityPath: 'deleteOperatingRoomCalendar',
      showAvailability: false,
      editAvailableDialog: false,
      editAvailableAction: {},
      editedAvailableIndex: -1,
      editOperatingRoomId:null
    }
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('operatingrooms.operatingRooms');
    },

    availabilityTitle() {
      const vm = this;

      return vm.$i18n.t('operatingrooms.operatingRoomsCalendar');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'name',
          text: vm.$i18n.t('operatingrooms.operatingRoom'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'location',
          text: vm.$i18n.t('operatingrooms.location'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'unavailableDates',
          text: vm.$i18n.t('operatingrooms.unavailableDates'),
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

    availabilityHeaders() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'operatingRoomName',
          text: vm.$i18n.t('operatingrooms.operatingRoom'),
          sortable: false,
          align: 'left'
        },
        {
          value: 'startDate',
          text: vm.$i18n.t('operatingroomscalendar.startDate'),
          sortable: true,
          align: 'left',
          isDate:true
        },
        {
          value: 'endDate',
          text: vm.$i18n.t('operatingroomscalendar.endDate'),
          sortable: true,
          align: 'left',
          isDate:true
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    operatingRooms() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.operatingRooms;
    },

    operatingRoomCalendar() {
      const vm = this;

      return vm.$store.state.operatingRoomCalendarModule.operatingRoomCalendar;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.equipmentModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getNonPortableEquipments in vuex store
     if(vm.editLoadOnce){
        vm.$store.dispatch('getNonPortableEquipments');
        vm.$store.dispatch('getAllOperationTypes');
        vm.editLoadOnce = false;
     }
    },

    deleteAvailabilityDialog(newVal, oldVal){
      const vm = this;

      if (newVal == false && oldVal == true)
      {
        vm.getOperatingRooms();
      }
    }
  },

  methods: {
    calendar(payload) {
      const vm = this;

      vm.showAvailability = true;
      // vm.calendarDialog = true;
      // vm.calendarAction = Object.assign({}, payload);
      vm.editOperatingRoomId = payload.id;
      vm.$store.dispatch('getOperatingRoomsCalendar', {operatingRoomId: payload.id }).then(() => {
        window.scrollTo(0,1000);
      });

      //vm.$router.push('/operatingroomcalendarpage?roomId=' + payload.id);
    },

    getMethodName(){
      return "getOperatingRooms";
    },


    getAvailableMethodName() {
      return "getOperatingRoomsCalendar";
    },

    deleteMethodName(){
      return "deleteOperatingRoom";
    },

    deleteAvailabilityMethodName(){
      return "deleteOperatingRoomCalendar";
    },

    exportOperatingRoomToExcel() {
      const vm = this;

      vm.$store.dispatch('exportOperatingRoomToExcel', { langId: vm.$cookie.get("currentLanguage") });
    },

    exportOperatingRoomCalendarToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOperatingRoomCalendar', {id: vm.editOperatingRoomId, langId: vm.$cookie.get("currentLanguage")});
    },

    getOperatingRooms(){
      const vm = this;

      var child = vm.$refs.gridComponent;
      child.executeGridOperations(true);
    }
  }
};

</script>
