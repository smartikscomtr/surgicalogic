<template>
  <div>
    <v-dialog
      v-model="showModal"
      slot="activator"
      persistent
    >
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
              {{ formTitle }}
            </span>

            <v-icon
              @click="cancel"
              class="close-wrap"
            >
              close
            </v-icon>
          </div>
        </v-card-title>

        <v-form
          ref="form"
          v-model="valid"
          lazy-validation
        >
          <v-card-text>
            <v-layout
              wrap
              edit-layout
            >
              <v-flex
                xs6
                sm6
                md6
              >
                <v-menu
                  ref="menu1"
                  :close-on-content-click="false"
                  v-model="menu1"
                  :nudge-right="40"
                  :return-value.sync="startDate"
                  lazy
                  transition="scale-transition"
                  offset-y
                  full-width
                  min-width="290px"
                >
                  <v-text-field
                    readonly
                    slot="activator"
                    clearable
                    :rules="required"
                    v-model="startDateFormatted"
                    :label="$t('operatingroomscalendar.startDate')"
                  >
                  </v-text-field>

                  <v-date-picker
                    v-model="startDate"
                    no-title
                    locale="tr-TR"
                    @input="$refs.menu1.save(startDate);"
                    :min="getMinDate()"
                  >
                  </v-date-picker>
                </v-menu>
              </v-flex>

              <v-flex
                xs6
                sm6
                md6
              >
                <v-menu
                  ref="menu2"
                  :close-on-content-click="false"
                  v-model="menu2"
                  :nudge-right="40"
                  :return-value.sync="endDate"
                  lazy
                  transition="scale-transition"
                  offset-y
                  full-width
                  min-width="290px"
                >
                  <v-text-field
                    readonly
                    slot="activator"
                    clearable
                    :rules="requiredEnd"
                    v-model="endDateFormatted"
                    :label="$t('operatingroomscalendar.endDate')"
                  >
                  </v-text-field>

                  <v-date-picker
                    v-model="endDate"
                    no-title
                    locale="tr-TR"
                    @input="$refs.menu2.save(endDate);"
                    :min="getMinDate()"
                  >
                  </v-date-picker>
                </v-menu>
              </v-flex>
            </v-layout>
          </v-card-text>

          <v-card-actions class="justify-end flex">
            <v-btn
              class="orangeButton"
              @click.native="save"
            >
              {{ $t('menu.save') }}
            </v-btn>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  props: {
    editAvailableVisible: {
      type: Boolean,
      required: false
    },

    editAvailableAction: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    },

    editAvailableIndex: {
      type: Number,
      required: false
    },

    editOperatingRoomId: {
      type: Number,
      required: false
    }
  },

  data() {
    return {
      menu1: false,
      menu2: false,
      startDateFormatted: null,
      endDateFormatted: null,
      valid: true,
      required: [v => !!v || this.$i18n.t("common.required")],
      requiredEnd: [
        v => !!v || this.$i18n.t("common.required"),
        v =>
          this.endDate >= this.startDate ||
          this.$i18n.t("common.endDateMustBeBiggerThanStart")
      ]
    };
  },

  computed: {

    formTitle() {
      const vm = this;

      return vm.editAvailableIndex === -1
        ? vm.$t("operatingrooms.addOperatingRoomInformation")
        : vm.$t("operatingrooms.editOperatingRoomInformation");
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editAvailableVisible;
      },

      set(value) {
        const vm = this;

        //When the cancel button is clicked, the event is sent to the operating rooms edit component
        if (!value) {
          vm.$emit("cancel");
        }
      }
    },

    startDate: {
      get() {
        const vm = this;

        if (vm.editAvailableAction.startDate) {
          vm.$store.commit("saveStartDate", vm.editAvailableAction.startDate);
        }

        vm.startDateFormatted = vm.formatDate(
          vm.$store.state.operatingRoomCalendarModule.startDate
        );
        return vm.$store.state.operatingRoomCalendarModule.startDate;
      },

      set(newValue) {
        const vm = this;

        if (newValue) {
          vm.editAvailableAction.startDate = newValue;
          vm.$store.commit("saveStartDate", vm.editAvailableAction.startDate);
        }
      }
    },

    endDate: {
      get() {
        const vm = this;

        if (vm.editAvailableAction.endDate) {
          vm.$store.commit("saveEndDate", vm.editAvailableAction.endDate);
        }

        vm.endDateFormatted = vm.formatDate(
          vm.$store.state.operatingRoomCalendarModule.endDate
        );
        return vm.$store.state.operatingRoomCalendarModule.endDate;
      },

      set(newValue) {
        const vm = this;

        if (newValue) {
          vm.editAvailableAction.endDate = newValue;
          vm.$store.commit("saveEndDate", vm.editAvailableAction.endDate);
        }
      }
    }
  },

  watch: {
    startDate(val) {
      const vm = this;

      vm.startDateFormatted = vm.formatDate(vm.editAvailableAction.startDate);
    },

    endDate(val) {
      const vm = this;

      vm.endDateFormatted = vm.formatDate(vm.editAvailableAction.endDate);
    }
  },

  methods: {
    cancel() {
      const vm = this;

      vm.clear();
      vm.showModal = false;
    },

    clear() {
      const vm = this;

      vm.$store.commit("saveStartDate",null);
      vm.$store.commit("saveEndDate",null);
      vm.editAvailableAction.startDate = null;
      vm.editAvailableAction.endDate = null;
      vm.endDateFormatted = null;
      vm.startDateFormatted = null;

      vm.$refs.form.reset();
    },

    save() {
      const vm = this;

      if (!vm.$refs.form.validate()) {
        return;
      }

      //Edit operating room
      if (vm.editAvailableIndex > -1) {
        //We are accessing updateOperatingRoom in vuex store
        vm.$store.dispatch("updateOperatingRoomCalendar", {
          id: vm.editAvailableAction.id,
          operatingRoomId: vm.editOperatingRoomId,
          startDate: vm.startDate,
          endDate: vm.endDate
        }).then(() => {
          vm.$emit('refresh');
        });
      }
      //Add operating room
      else {
        //We are accessing insertOperatingRoom in vuex store
        vm.$store.dispatch("insertOperatingRoomCalendar", {
          operatingRoomId: vm.editOperatingRoomId,
          startDate: vm.startDate,
          endDate: vm.endDate
        }).then(() => {
          vm.$emit('refresh');
        });
      }

      vm.showModal = false;
      vm.clear();
    },

    getMinDate() {
      const toTwoDigits = num => (num < 10 ? "0" + num : num);
      let today = new Date();

      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let day = toTwoDigits(today.getDate());

      return `${year}-${month}-${day}`;
    },

    formatDate(date) {
      if (!date || date.indexOf(".") > -1) return null;

      const [year, month, day] = date.split("-");

      return `${day}.${month}.${year}`;
    }
  }

  // mounted () {
  //   const vm = this;

  //   vm.$watch('startDate', (newValue) => {

  //       vm.startDateFormatted = vm.formatDate(newValue)
  //   });

  //   vm.$watch('endDate', (newValue) => {
  //       vm.endDateFormatted = vm.formatDate(newValue)
  //   });
  // }
};
</script>
