<template>
  <div>
    <div class="container fluid grid-list-md">
      <div class="grid-card">
        <v-btn class="orange"
               @click="runSimulation">
          <span lang="tr">{{ $t('simulation.simulate') }}</span>
        </v-btn>
      </div>
    </div>

    <div v-if="simulation">
      <grid-component :headers="headers"
                      :title="title"
                      :items="simulations"
                      :show-detail="false"
                      :show-edit="false"
                      :show-delete="false"
                      :show-search="false"
                      :show-insert="false"
                      :hide-actions="false"
                      :methodName="getMethodName"
                      :loading="getLoading"
                      :totalCount="getTotalCount"
                      @detail="detail"
                      @edit="edit"
                      @newaction="addNewItem"
                      @deleteitem="deleteItem">
      </grid-component>
    </div>
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
      simulation: false
    };
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('simulation.simulation');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value:'operatingRoomName',
          sortBy:'OperatingRoom.Name',
          text: vm.$i18n.t('plan.operationRoomName'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'usage',
          sortBy: 'usage',
          text: vm.$i18n.t('simulation.usage'),
          sortable: true,
          align: 'left'
        },
        {
          value:'overTime',
          text: vm.$i18n.t('simulation.overtime'),
          sortable: true,
          align: 'left'
        },
        {
          value:'waitingTime',
          text: vm.$i18n.t('simulation.waitingTime'),
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

    simulations() {
      const vm = this;

      return vm.$store.state.simulationModule.tomorrowList;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.simulationModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.simulationModule.tomorrowListTotalCount;
    }
  },

  methods: {
    getMethodName(){
      return "runSimulation";
    },

    deleteMethodName(){
      return "";
    },

    runSimulation() {
      const vm = this;

      vm.simulation = true;
    }
  },

  created () {
    const vm = this;

    vm.$store.dispatch('getAllOperatingRooms');
  }
};

</script>
