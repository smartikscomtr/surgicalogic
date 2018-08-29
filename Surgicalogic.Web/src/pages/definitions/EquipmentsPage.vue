<template>
  <div>
    <grid-component :headers="headers"
                    :items="equipments"
                    :title="title"
                    :show-detail="true"
                    :show-edit="true"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="true"
                    :methodName="getMethodName"
                    :loading="getLoading"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    @edit="edit"
                    @exportToExcel="exportEquipmentsToExcel"
                    @newaction="addNewItem"
                    @deleteitem="deleteItem">
    </grid-component>

    <equipments-detail-component :detail-action="detailAction"
                                 :detail-visible="detailDialog"
                                 @cancel="cancel">
    </equipments-detail-component>

    <equipments-edit-component :edit-action="editAction"
                               :edit-visible="editDialog"
                               :edit-index="editedIndex"
                               @cancel="cancel">
    </equipments-edit-component>

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
      title: vm.$i18n.t('equipments.equipments'),
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
      deletePath: 'deleteEquipment'
    };
  },

  computed: {
    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'code',
          text: vm.$i18n.t('equipments.equipmentCode'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'name',
          text: vm.$i18n.t('equipments.name'),
          sortable: true,
          align: 'left'
        },
        {
          value:'equipmentTypeName',
          sortable:'EquipmentType.Name',
          text: vm.$i18n.t('equipmenttypes.equipmentType'),
          sortable: true,
          align: 'left'
        },
        {
          isCheck: true,
          value: 'isPortable',
          text: vm.$i18n.t('equipments.portable'),
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

    equipments() {
      const vm = this;

      return vm.$store.state.equipmentModule.equipments;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.equipmentModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.equipmentModule.totalCount;
    }
  },

  watch: {
   editDialog(){
     const vm = this;

    //We are accessing getAllEquipmentTypes in vuex store
     if(vm.editLoadOnce){
        vm.$store.dispatch('getAllEquipmentTypes');
        vm.$store.dispatch('getAllOperatingRooms');
        vm.editLoadOnce = false;
     }
    }
  },

  methods: {
    getMethodName(){
      return "getEquipments";
    },

    deleteMethodName(){
      return "deleteEquipment";
    },

    exportEquipmentsToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportEquipments');

      setTimeout(() => {
        const link = document.createElement('a');

        link.href = vm.$store.state.equipmentModule.excelUrl;
        document.body.appendChild(link);
        link.click();
      }, 2000);
    }
  }
};

</script>
