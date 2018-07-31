<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator">
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
              {{ formTitle }}
            </span>

            <!-- // <v-icon @click="cancel"
            //         class="close-wrap">
            //   close
            // </v-icon> -->
          </div>
        </v-card-title>

        <v-card-text>
            <v-layout wrap>



               <v-data-table
                :headers="headers"
                :items="operatingRoomCalendar"
                hide-actions
                class="elevation-1"
              >
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.startdate }}</td>
                  <td>{{ props.item.enddate }}</td>
                </template>
              </v-data-table>



            </v-layout>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

export default {
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
      title: vm.$i18n.t('operatingrooms.operatingRooms'),
      deleteValue: {}

    };
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.$i18n.t('operatingrooms.operatingRoomsCalendar');
    },

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
          value: 'startdate',
          text: "Başlangıç Tarihi", //vm.$i18n.t('operatingrooms.operatingRoom'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'enddate',
          text: 'Bitiş Tarihi', //vm.$i18n.t('operatingrooms.location'),
          sortable: true,
          align: 'left'
        }
      ];
    },

    operatingRoomCalendar() {
      const vm = this;

      return vm.$store.state.operatingRoomCalendarModule.operatingRoomCalendar;
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
