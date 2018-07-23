<template>
  <div>
    <grid-component :headers="headers"
                    :items="operatingRooms"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    :methodName="getMethodName"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    @edit="edit"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

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
import {gridMixin} from './../../mixins/gridMixin'

export default {
  mixins: [gridMixin],
  data() {
    const vm = this;

    return {
      title: vm.$i18n.t('operatingrooms.operatingRooms'),
      search: '',
      detailDialog: false,
      editDialog: false,
      deleteDialog: false,
      detailAction: {},
      editAction: { operatingRoomEquipments : [] },
      deleteValue: {},
      editedIndex: -1,
      totalRowCount:0,
      editLoadOnce: true,
      equipmentName: null
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
          value: 'width',
          text: vm.$i18n.t('operatingrooms.width'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'height',
          text: vm.$i18n.t('operatingrooms.height'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'length',
          text: vm.$i18n.t('operatingrooms.length'),
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

    operatingRooms() {
      const vm = this;

      return vm.$store.state.operatingRoomModule.operatingRooms;
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
        vm.editLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getOperatingRooms";
    },

    deleteMethodName(){
      return "deleteOperatingRoom";
    }
  }
};

</script>
