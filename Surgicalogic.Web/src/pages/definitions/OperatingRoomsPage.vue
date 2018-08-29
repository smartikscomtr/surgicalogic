<template>
  <div>
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
                    @deleteitem="deleteItem">
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
      title: vm.$i18n.t('operatingrooms.operatingRooms'),
      search: '',
      calendarDialog:false,
      detailDialog: false,
      editDialog: false,
      deleteDialog: false,
      calendarAction:{},
      detailAction: {},
      editAction: {
        operatingRoomEquipments : [],
        operatingRoomOperationTypes:[]
      },
      deleteValue: {},
      editedIndex: -1,
      totalRowCount:0,
      editLoadOnce: true,
      equipmentName: null,
      deletePath: 'deleteOperatingRoom'
    }
  },

  computed: {
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
          value: 'isAvailable',
          text: vm.$i18n.t('operatingrooms.isAvailable'),
          sortable: true,
          isCheck: true,
          align: 'left'
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
    }
  },

  methods: {
    calendar(payload) {
      const vm = this;

      // vm.calendarDialog = true;
      // vm.calendarAction = Object.assign({}, payload);

      vm.$router.push('/operatingroomcalendarpage?roomId=' + payload.id);
    },

    getMethodName(){
      return "getOperatingRooms";
    },

    deleteMethodName(){
      return "deleteOperatingRoom";
    },

    exportOperatingRoomToExcel() {
      const vm = this;

      vm.$store.dispatch('exportOperatingRoomToExcel');

      setTimeout(() => {
        const link = document.createElement('a');

        link.href = vm.$store.state.operatingRoomModule.excelUrl;
        document.body.appendChild(link);
        link.click();
      }, 2000);
    }
  }
};

</script>
