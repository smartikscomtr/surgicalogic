<template>
  <div>
    <div class="v-card__title">
      <v-flex xs12 sm6 md3>
        <v-menu ref="menu"
                :close-on-content-click="false"
                v-model="menu"
                :nudge-right="40"
                :return-value.sync="date"
                lazy
                transition="scale-transition"
                offset-y
                full-width
                min-width="290px">
          <v-text-field append-icon="keyboard_arrow_down"
                        readonly
                        slot="activator"
                        clearable
                        v-model="dateFormatted"
                        :label="$t('operation.operationDate')">
          </v-text-field>

          <v-date-picker v-model="date"
                          no-title
                          @input="$refs.menu.save(date)">
          </v-date-picker>
        </v-menu>
      </v-flex>

      <v-flex xs12 sm6 md3>
        <v-btn class="orange"
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
      menu: false,
      dateFormatted: null,
      date: null,
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

  watch: {
    date(val) {
      const vm = this;

      vm.dateFormatted = vm.formatDate(vm.date);
    }
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

      return `${"runSimulation"}, ${vm.date}`;
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
    },

    getDate() {
      const toTwoDigits = num => (num < 10 ? "0" + num : num);
      let tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);

      let year = tomorrow.getFullYear();
      let month = toTwoDigits(tomorrow.getMonth() + 1);
      let day = toTwoDigits(tomorrow.getDate());

      return `${year}-${month}-${day}`;
    },

    formatDate(date) {
      if (!date || date.indexOf(".") > -1) return null;

      const [year, month, day] = date.split("-");

      return `${day}.${month}.${year}`;
    }
  },

  created () {
    const vm = this;

    vm.date = vm.getDate();
    vm.$store.dispatch('getAllOperatingRooms');
  }
};

</script>
