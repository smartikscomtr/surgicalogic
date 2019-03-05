<template>
  <div class="container fluid grid-list-md appointment-calendar-wrap" id="section-to-print" >
    <v-layout wrap edit-layout class="all-page-pad">
      <v-flex md3 sm3 xs12 class="doctor-detail">
        <img :src="doctorPictureUrl" height="150px" />

        <h4>  {{ doctorName }} </h4>
        <h4>  {{ doctorTitle }} </h4>


        <h5> {{ doctorBranchNames }}</h5>
      </v-flex>
      <v-flex md9 sm9 xs12 block-container>
        <v-flex xs12 sm12 md12>
          <v-menu ref="menu" :close-on-content-click="false" v-model="menu" :nudge-right="40" :return-value.sync="date" lazy transition="scale-transition" offset-y full-width min-width="290px">
            <v-text-field append-icon="keyboard_arrow_down" readonly slot="activator" v-model="dateFormatted" :label="$t('appointmentcalendar.appointmentDate')">
            </v-text-field>

            <v-date-picker v-model="date"
                            no-title
                            locale="tr-TR" @input="$refs.menu.save(date)" :min="getMinDate()" @change="destroyPicker()" :max="getMaxDate()">
            </v-date-picker>
          </v-menu>
        </v-flex>
        <v-flex xs12 sm12 md12>
          <input type="hidden" id="time-1">
        </v-flex>
      </v-flex>
      <v-dialog v-model="showModal" slot="activator" persistent>
        <v-card class="container fluid grid-list-md">
          <v-card-title>
            <div class="headline-wrap flex xs12 sm12 md12">
              <span class="text">
                {{ formTitle }}
              </span>

              <v-icon @click="cancel" class="close-wrap">
                close
              </v-icon>
            </div>
          </v-card-title>

        <v-form ref="form" v-model="valid" lazy-validation>
          <v-card-text>
            <v-layout wrap edit-layout>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="identityNumber" mask="###########" :rules="required" :label="$t('appointmentcalendar.identityNumber')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="phone"
                              mask="phone"
                              :rules="required"
                              :label="$t('appointmentcalendar.phone')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="firstName"
                              :rules="required"
                              :label="$t('appointmentcalendar.firstName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="lastName"
                              :rules="required"
                              :label="$t('appointmentcalendar.lastName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm12 md12>
                <v-text-field v-model="address" :label="$t('appointmentcalendar.address')">
                </v-text-field>
              </v-flex>
            </v-layout>
          </v-card-text>
          <div class="v-card__text">
            <div class="margin-bottom-none">
              <span class="available-message">{{ availableAppointmentsMessage }}</span>
              <v-btn class="btnSave orangeButton" @click="saveAppointment()">
              {{ $t('menu.save') }}
              </v-btn>
            </div>
          </div>
        </v-form>
        </v-card>
      </v-dialog>

      <snackbar-component :snackbar-visible="snackbarVisible" :savedMessage="savedMessage">
      </snackbar-component>
    </v-layout>
  </div>
</template>

<script src="js/appointment-picker.min.js"></script>

<script>
export default {
    data() {
        const vm = this;

        return {
            menu: false,
            required: [v => !!v || vm.$i18n.t("common.required")],
            valid: true,
            date: null,
            dateFormatted: null,
            interval: null,
            startTime: null,
            endTime: null,
            disabled: [],
            picker: null,
            doctorName: null,
            doctorTitle: null,
            doctorPictureUrl: null,
            doctorBranchNames: null,
            selectedTime: null,
            showModal: false,
            selectedTimes: [],
            availableAppointmentsMessage: '',
            identityNumber: null,
            firstName: null,
            lastName: null,
            phone: null,
            address: null,
            maxDateDayCount: null,
            snackbarVisible: null,
            savedMessage: null,
            dateChanged: false
        };
    },

    watch: {
        date(val) {
            const vm = this;

            vm.dateChanged = true;
            vm.dateFormatted = vm.formatDate(vm.date);
        }
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.$i18n.t('appointmentcalendar.patientInformation');
        },

        pickerTitle(){
          const vm = this;

          return vm.$i18n.t('appointmentcalendar.appointmentTimes');
        }
    },

    methods: {
        cancel() {
          const vm = this;

          vm.clear();
          vm.showModal = false;
          vm.picker.setTime('');
        },

        getMaxDate() {
            const vm = this;

            const toTwoDigits = num => (num < 10 ? '0' + num : num);
            let selectDay = new Date();

            selectDay.setDate(selectDay.getDate() + vm.maxDateDayCount);

            let year = selectDay.getFullYear();
            let month = toTwoDigits(selectDay.getMonth() + 1);
            let day = toTwoDigits(selectDay.getDate());

            return `${year}-${month}-${day}`;
        },

        getMinDate() {
            const toTwoDigits = num => (num < 10 ? '0' + num : num);
            let selectDay = new Date();

            selectDay.setDate(selectDay.getDate() + 1);

            let year = selectDay.getFullYear();
            let month = toTwoDigits(selectDay.getMonth() + 1);
            let day = toTwoDigits(selectDay.getDate());

            return `${year}-${month}-${day}`;
        },

        formatDate(date) {
            if (!date || date.indexOf('.') > -1) return null;

            const [year, month, day] = date.split('-');

            return `${day}.${month}.${year}`;
        },

        getDate() {
            const toTwoDigits = num => (num < 10 ? '0' + num : num);
            let today = new Date();

            today.setDate(today.getDate() + 1);

            let year = today.getFullYear();
            let month = toTwoDigits(today.getMonth() + 1);
            let day = toTwoDigits(today.getDate());

            return `${year}-${month}-${day}`;
        },

        destroyPicker(manual) {
            const vm = this;

            if (manual || vm.dateChanged)
            {
              vm.showModal = false;
              vm.picker.setTime('');
              vm.picker.destroy();
              vm.dateChanged = false;
            }
        },

        clear () {
            this.$refs.form.reset()
        },

        saveAppointment() {
            const vm = this;

            if (!vm.$refs.form.validate()) {
              return;
            }

            vm.$store
                .dispatch('insertAppointmentCalendar', {
                    identityNumber: vm.identityNumber,
                    firstName: vm.firstName,
                    lastName: vm.lastName,
                    phone: vm.phone,
                    address: vm.address,
                    appointmentDate: vm.date,
                    appointmentTime: vm.selectedTime,
                    personnelId: vm.$route.query.doctorId
                })
                .then(response => {
                    if (response.data.info.succeeded) {
                        vm.savedMessage = this.$i18n.t(
                            'appointmentcalendar.appointmentSavedSuccessfully'
                        );
                    } else {
                        vm.savedMessage = this.$i18n.t(
                            'appointmentcalendar.anErrorOccurred'
                        );
                    }

                    vm.snackbarVisible = true;

                    setTimeout(() => {
                        vm.snackbarVisible = false;
                    }, 3000);

                    vm.showModal = false;
                    vm.getAppointmentCalendar();
                    vm.picker.setTime('');
                    (vm.identityNumber = null),
                        (vm.firstName = null),
                        (vm.lastName = null),
                        (vm.phone = null),
                        (vm.address = null);
                });
        },

        getAppointmentCalendar() {
            const vm = this;

            if (vm.picker) {
                vm.destroyPicker(true);
            }

            const AppointmentPicker = require('appointment-picker');

            vm.$store
                .dispatch('getAppointmentCalendarByDate', {
                    selectedDate: vm.date,
                    doctorId: vm.$route.query.doctorId
                })
                .then(response => {
                    vm.interval = response.data.interval;
                    vm.startTime = response.data.startTime;
                    vm.endTime = response.data.endTime;
                    vm.disabled = response.data.disabled;
                    vm.selectedTimes = response.data.selectedTimes;
                    vm.assignedSchedulesAsDate = response.data.assignedSchedulesAsDate;

                    require(['appointment-picker'], function(
                        AppointmentPicker
                    ) {
                        vm.picker = new AppointmentPicker(
                            document.getElementById('time-1'),
                            {
                                interval: vm.interval, //Randevu aralık dakikaları
                                minTime: vm.startTime, //Min seçilebilir saat
                                maxTime: vm.endTime, //Max seçilebilir saat
                                mode: '24h', //24 saat ya da 12 saat kullanılıp kullanılmayacağı
                                startTime: vm.startTime, //Gürntülenen saat başlangıcı
                                endTime: vm.endTime, //Görüntülenen saat bitişi
                                disabled: vm.disabled, //Disabled
                                large: true, //Görünümün büyüklük kontrolü
                                static: true, // Statik pop-up durumu (Her zaman açık)
                                leadingZero: true, // Whether to zero pad hour (i.e. 07:15)
                                allowReset: true, // Whether a time can be resetted once entered
                                title: "" //Başlık
                            }
                        );

                        vm.picker.open();
                    });
                });
        }
    },

    created() {
        const vm = this;

        vm.$store
            .dispatch('getPersonnelById', vm.$route.query.doctorId)
            .then(response => {
              (vm.doctorTitle = response.data.personnelTitle),
                (vm.doctorName = response.data.fullName),
                    (vm.doctorBranchNames = response.data.branchNames),
                    (vm.doctorPictureUrl = response.data.pictureUrl);
            });

          vm.$store.dispatch('getAppointmentDays').then(response => {
                vm.maxDateDayCount = response.data
          });

        document.body.addEventListener(
            'change.appo.picker',
            function(e) {
                vm.selectedTime =
                    (e.time.h <= 9 ? '0' + e.time.h : e.time.h) +
                    ':' +
                    (e.time.m == 0 ? '0' + e.time.m : e.time.m);

                var availablePerson = 0;

                for (let index = 0; index < vm.assignedSchedulesAsDate.length; index++) {
                    if (vm.assignedSchedulesAsDate[index] == vm.selectedTime) {
                        availablePerson++;
                    }
                }

                vm.availableAppointmentsMessage =
                    vm.selectedTime +
                    ' saatine ' +
                    availablePerson +
                    ' randevu alabilirsiniz.';
                vm.showModal = true;
            },
            false
        );
    },

    mounted() {
        const vm = this;

        vm.$watch('date', (newValue, oldValue) => {
            if (newValue !== oldValue) {
                vm.getAppointmentCalendar();
            }
        });

        vm.date = vm.getDate();
    }
};
</script>
