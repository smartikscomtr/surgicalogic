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

            <v-icon @click="cancel"
                    class="close-wrap">
              close
            </v-icon>
          </div>
        </v-card-title>

        <v-card-text>
          <v-layout wrap edit-layout>
            <v-flex xs6 sm6 md6>
              <v-menu ref="menu1"
                      :close-on-content-click="false"
                      v-model="menu1"
                      :nudge-right="40"
                      :return-value.sync="startDate"
                      lazy
                      transition="scale-transition"
                      offset-y
                      full-width
                      min-width="290px">
                <v-text-field readonly
                              slot="activator"
                              v-model="startDateFormatted"
                              :label="$t('operatingroomscalendar.startDate')">
                </v-text-field>

                <v-date-picker v-model="startDate"
                                no-title
                                @input="$refs.menu1.save(startDate);"
                                :min="getMinDate()">
                </v-date-picker>
              </v-menu>
            </v-flex>

             <v-flex xs6 sm6 md6>
              <v-menu ref="menu2"
                      :close-on-content-click="false"
                      v-model="menu2"
                      :nudge-right="40"
                      :return-value.sync="endDate"
                      lazy
                      transition="scale-transition"
                      offset-y
                      full-width
                      min-width="290px">
                <v-text-field readonly
                              slot="activator"
                              v-model="endDateFormatted"
                              :label="$t('operatingroomscalendar.endDate')">
                </v-text-field>

                <v-date-picker v-model="endDate"
                                no-title
                                @input="$refs.menu2.save(endDate);"
                                :min="getMinDate()">
                </v-date-picker>
              </v-menu>
            </v-flex>

            <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right margin-bottom-none>
              <v-btn class="btnSave orange"
                      @click.native="save">
                Kaydet
              </v-btn>
            </v-flex>
          </v-layout>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>

export default {
  props: {
    editVisible: {
      type: Boolean,
      required: false
    },

    editAction: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    },

    editIndex: {
      type: Number,
      required: false
    }
  },

  data() {
    return {
      menu1: false,
      menu2: false,
      startDateFormatted: null,
      endDateFormatted: null
    };
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1 ? vm.$t('operatingrooms.addOperatingRoomInformation') : vm.$t('operatingrooms.editOperatingRoomInformation');
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the operating rooms edit component
        if (!value) {
          vm.$emit('cancel');
        }
      }
    },

    startDate: {
      get(){
        const vm = this;

        if (vm.editAction.startDate)
        {
          vm.$store.commit('saveStartDate', vm.editAction.startDate);
        }

        return vm.$store.state.operatingRoomCalendarModule.startDate;
      },

      set(newValue){
        const vm = this;

        if (newValue)
        {
          vm.editAction.startDate = newValue;
          vm.$store.commit('saveStartDate',  vm.editAction.startDate);
        }
      }
    },

     endDate: {
      get(){
        const vm = this;

        if (vm.editAction.endDate)
        {
          vm.$store.commit('saveEndDate',  vm.editAction.endDate);
        }

        return vm.$store.state.operatingRoomCalendarModule.endDate;
      },

      set(newValue){
        const vm = this;

        if (newValue) {
          vm.editAction.endDate = newValue;
          vm.$store.commit('saveEndDate', vm.editAction.endDate);
        }
      }
     }
  },

  watch:{
    startDate (val) {
      this.startDateFormatted = this.formatDate(this.startDate)
    },

    endDate (val) {
      this.endDateFormatted = this.formatDate(this.endDate)
    }
  },

  methods: {
    cancel() {
      const vm = this;

      vm.showModal = false;
    },

    save() {
      const vm = this;

      //Edit operating room
      if (vm.editIndex > -1) {
        //We are accessing updateOperatingRoom in vuex store
        vm.$store.dispatch('updateOperatingRoomCalendar', {
          id: vm.editAction.id,
          operatingRoomId: vm.$route.query.roomId,
          startDate: vm.startDate,
          endDate: vm.endDate,
        });

      }
      //Add operating room
      else {
        //We are accessing insertOperatingRoom in vuex store
        vm.$store.dispatch('insertOperatingRoomCalendar', {
          operatingRoomId: vm.$route.query.roomId,
          startDate: vm.startDate,
          endDate: vm.endDate,
        });
      }

      vm.showModal = false;
    },

    getMinDate() {
      const toTwoDigits = num => num < 10 ? '0' + num : num;
      let today = new Date();

      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let day = toTwoDigits(today.getDate());

      return `${year}-${month}-${day}`;
    },

     formatDate (date) {
      if (!date || date.indexOf('.')> -1) return null

      const [year, month, day] = date.split('-');

      return `${day}.${month}.${year}`
    }
  }
}

</script>
