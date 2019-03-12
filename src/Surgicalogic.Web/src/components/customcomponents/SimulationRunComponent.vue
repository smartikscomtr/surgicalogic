<template>
  <div>
    <div class="v-card__title">

      <v-flex xs12 sm6 md3>
        <v-btn class="orangeButton"
              @click="runSimulation">
          <span lang="tr">{{ $t('simulation.simulate') }}</span>
        </v-btn>
      </v-flex>

      <v-flex xs12 sm6 md4>
      </v-flex>
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
                      :hide-export="true"
                      :methodName="getMethodName"
                      :custom-parameters="customParameters"
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

  props: {
    date: {
      type: String,
      required: false
    }
  },

  data() {
    const vm = this;

    return {
      menu: false,
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
      simulation: false,
      customParameters: {}
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
          value:'overTime',
          text: vm.$i18n.t('simulation.overtime'),
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

      return vm.$store.state.simulationModule.operationList;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.simulationModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.simulationModule.operationListTotalCount;
    }
  },

  methods: {
    getMethodName(){
      const vm = this;

      vm.customParameters.selectDate = vm.date;

      return "runSimulation";
    },

    deleteMethodName(){
      return "";
    },

    runSimulation() {
      const vm = this;

      vm.$store.dispatch('runSimulation', {
        selectDate: vm.date
      });
      vm.simulation = true;
    }
  },

  created () {
    const vm = this;

    vm.$store.dispatch('getAllOperatingRooms');
  }
};

</script>
