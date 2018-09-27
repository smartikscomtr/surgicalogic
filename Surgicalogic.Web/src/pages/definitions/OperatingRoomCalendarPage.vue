
<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="operatingRoomCalendar"
                    :title="title"
                    :show-detail="false"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="false"
                    :show-insert="true"
                    :hide-actions="true"
                    :methodName="getMethodName"
                    :custom-parameters="customParameters"
                    :loading="getLoading"
                    @edit="edit"
                    @exportToExcel="exportOperatingRoomCalendarToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <operating-rooms-calendar-edit-component :edit-action="editAction"
                        :edit-visible="editDialog"
                        :edit-index="editedIndex"
                        @cancel="cancel">
    </operating-rooms-calendar-edit-component>

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

  props: {
    calendarVisible: {
      type: Boolean,
      required: false
    },

    calendarAction: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    }
  },

  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('operatingrooms.operatingRoomsCalendar'),
      editDialog: false,
      deleteDialog: false,
      editAction: {},
      deleteValue: {},
      editedIndex: -1,
      editLoadOnce: true,
      customParameters: {}
    };
  },

  computed: {
    showModal: {
      get() {
        const vm = this;

        return vm.calendarVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the operating rooms detail component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'startDate',
          text: "Başlangıç Tarihi", //vm.$i18n.t('operatingrooms.operatingRoom'),
          sortable: true,
          align: 'left',
          isDate:true
        },
        {
          value: 'endDate',
          text: 'Bitiş Tarihi', //vm.$i18n.t('operatingrooms.location'),
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

    operatingRoomCalendar() {
      const vm = this;

      return vm.$store.state.operatingRoomCalendarModule.operatingRoomCalendar;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.operatingRoomCalendarModule.loading;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getNonPortableEquipments in vuex store
     if(vm.editLoadOnce){
        // vm.$store.dispatch('getNonPortableEquipments');
        // vm.$store.dispatch('getAllOperationTypes');
        vm.editLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getOperatingRoomsCalendar";
    },

    deleteMethodName(){
      return "deleteOperatingRoomCalendar";
    },

    exportOperatingRoomCalendarToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportOperatingRoomCalendar', {id: vm.$route.query.roomId});
    }
  },

  created() {
     const vm = this;

      vm.customParameters.operatingRoomId = vm.$route.query.roomId;
  }
};

</script>
