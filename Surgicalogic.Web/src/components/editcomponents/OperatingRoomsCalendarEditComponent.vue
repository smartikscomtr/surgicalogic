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
          <v-layout wrap>
            <v-flex xs6 sm6 md6>
 <v-text-field v-model="editAction['startDate']"
                            :label="$t('operatingroomscalendar.startDate')">
              </v-text-field>
            </v-flex>

             <v-flex xs6 sm6 md6>
 <v-text-field v-model="editAction['endDate']"
                            :label="$t('operatingroomscalendar.endDate')">
              </v-text-field>
            </v-flex>

            <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right>
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
    return {};
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
          startDate: vm.editAction.startDate,
          endDate: vm.editAction.endDate,
        });

      }
      //Add operating room
      else {
        //We are accessing insertOperatingRoom in vuex store
        vm.$store.dispatch('insertOperatingRoomCalendar', {
          operatingRoomId: vm.$route.query.roomId,
          startDate: vm.editAction.startDate,
          endDate: vm.editAction.endDate,
        });
      }

      vm.showModal = false;
    }
  }
}

</script>
