<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              persistent>
      <v-card class="container fluid grid-list-md">
        <!-- <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
            </span>

            <v-icon @click="cancel"
                    class="close-wrap">
              close
            </v-icon>
          </div>
        </v-card-title> -->

        <v-card-text>
          <v-layout wrap>
            <grid-component :headers="headers"
                            :items="operatingRoomCalendar"
                            :title="title"
                            :show-detail="false"
                            :show-edit="false"
                            :show-delete="false"
                            :show-search="false"
                            :show-insert="false"
                            :hide-actions="true"
                            :methodName="getMethodName"
                            :loading="getLoading"
                            @edit="edit"
                            @newaction="addNewItem">
            </grid-component>
          </v-layout>
        </v-card-text>
      </v-card>
    </v-dialog>
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
      editedIndex: -1
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
          text: `${vm.$i18n.t('operatingroomscalendar.startDate')}`,
          sortable: true,
          align: 'left',
          isDateTime:true
        },
        {
          value: 'endDate',
          text: `${vm.$i18n.t('operatingroomscalendar.endDate')}`,
          sortable: true,
          align: 'left',
          isDateTime:true
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

  methods: {
    getMethodName(){
      return "getOperatingRoomsCalendar";
    },

    deleteMethodName(){
      return "deleteOperatingRoomCalendar";
    }
  }
};

</script>
