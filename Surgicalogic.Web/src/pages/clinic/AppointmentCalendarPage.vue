<template>
  <div class="container fluid grid-list-md">
    <v-layout wrap edit-layout class="all-page-pad">
      <v-flex md3 sm3 xs12 class="doctor-detail">
        <img :src="doctorPictureUrl" height="150px" />

        <h4> {{ doctorName }} </h4>

        <h5> {{ doctorBranchNames }}</h5>
      </v-flex>
      <v-flex md9 sm9 xs12 block-container>
        <v-flex xs12 sm12 md12>
          <v-menu ref="menu" :close-on-content-click="false" v-model="menu" :nudge-right="40" :return-value.sync="date" lazy transition="scale-transition" offset-y full-width min-width="290px">
            <v-text-field readonly slot="activator" v-model="dateFormatted" :label="$t('operation.operationDate')">
            </v-text-field>

            <v-date-picker v-model="date" no-title @input="$refs.menu.save(date)" :min="getMinDate()" @change="destroyPicker()" :max="getMaxDate()">
            </v-date-picker>
          </v-menu>
        </v-flex>
        <v-flex xs12 sm12 md12>
          <input type="hidden" id="time-1">
        </v-flex>
      </v-flex>
      <v-dialog v-model="showModal"
                persistent>
        <v-card class="container fluid grid-list-md">
          <v-card-title>
            <div class="headline-wrap flex xs12 sm12 md12">
              <span class="text">
                {{ formTitle }}
              </span>

              <span>{{ availableAppointmentsMessage }}</span>

              <v-icon @click="cancel" class="close-wrap">
                close
              </v-icon>
            </div>
          </v-card-title>

          <v-card-text>
            <v-layout wrap>
              <v-flex xs12 sm6 md6>
                <v-text-field v-model="identityNumber"
                              mask="###########"
                              :label="$t('appointmentcalendar.identityNumber')"
                              ma>
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="firstName"
                              :label="$t('appointmentcalendar.firstName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="lastName"
                              :label="$t('appointmentcalendar.lastName')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md6>
                <v-text-field v-model="phone"
                              mask="phone"
                              :label="$t('appointmentcalendar.phone')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm12 md12>
                <v-text-field v-model="address"
                              :label="$t('appointmentcalendar.address')">
                </v-text-field>
              </v-flex>

            </v-layout>
          </v-card-text>
          <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right margin-bottom-none class="btn-wrap">
            <v-btn class="btnSave orange" @click="saveAppointment()">
              Kaydet
            </v-btn>
          </v-flex>
        </v-card>
      </v-dialog>
    </v-layout>
        <snackbar-component :snackbar-visible="snackbarVisible"
                            :savedMessage="savedMessage">
        </snackbar-component>

      </div>
    </v-container>
  </div>
</template>

<script src="js/appointment-picker.min.js"></script>

<script>
export default {
    data() {
        const vm = this;

    return {
      menu: false,
      date: null,
      dateFormatted: null,
      interval:null,
      startTime:null,
      endTime:null,
      disabled:[],
      picker: null,
      doctorName: null,
      doctorPictureUrl: null,
      doctorBranchNames: null,
      selectedTime: null,
      dialog: false,
      personPerPeriod:0,
      selectedTimes:[],
      availableAppointmentsMessage: "",
      identityNumber: null,
      firstName: null,
      lastName: null,
      phone: null,
      address: null,
      maxDateDayCount:null,
      snackbarVisible: null,
      savedMessage: null
    };
  },

    watch: {
        date(val) {
            const vm = this;

            vm.dateFormatted = vm.formatDate(vm.date);
        }
    },

    computed: {
        formTitle() {
            const vm = this;

            return vm.$i18n.t('appointmentcalendar.patientInformation');
        }
    },

    methods: {
        cancel() {
            const vm = this;

            vm.showModal = false;
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

        destroyPicker() {
            const vm = this;

      vm.dialog = false;
      vm.picker.setTime('');
      vm.picker.destroy();
    },

        saveAppointment() {
            const vm = this;

      vm.$store.dispatch('insertAppointmentCalendar', {
        identityNumber: vm.identityNumber,
        firstName: vm.firstName,
        lastName: vm.lastName,
        phone: vm.phone,
        address: vm.address,
        appointmentDate: vm.date,
        appointmentTime : vm.selectedTime,
        personnelId: vm.$route.query.doctorId
      }).then(response => {
        if (response.data.info.succeeded) {
            vm.savedMessage = this.$i18n.t('appointmentcalendar.appointmentSavedSuccessfully')
        }
        else {
            vm.savedMessage = this.$i18n.t('appointmentcalendar.anErrorOccurred')
        }

        vm.snackbarVisible = true;

        setTimeout(() => {
          vm.snackbarVisible = false;
        }, 3000)

        vm.dialog = false;
        vm.getAppointmentCalendar();
        vm.picker.setTime('');
        vm.identityNumber= null,
        vm.firstName= null,
        vm.lastName= null,
        vm.phone= null,
        vm.address= null
      });
    },

        getAppointmentCalendar() {
            const vm = this;

            if (vm.picker) {
                vm.destroyPicker();
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
                    vm.personPerPeriod = response.data.personPerPeriod;
                    vm.selectedTimes = response.data.selectedTimes;

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
                                title: 'Randevu Saatleri' //Başlık
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
                (vm.doctorName = response.data.fullName),
                    (vm.doctorBranchNames = response.data.branchNames),
                    (vm.doctorPictureUrl = response.data.pictureUrl);
            });

      vm.$store.dispatch('getAppointmentDays').then(response => {
        vm.maxDateDayCount = response.data
      });

      document.body.addEventListener('change.appo.picker', function(e) {
        vm.selectedTime = e.time.h + ":" + (e.time.m == 0 ? "0" + e.time.m :e.time.m);

                var availablePerson = vm.personPerPeriod;

        for (let index = 0; index < vm.selectedTimes.length; index++) {
          if(vm.selectedTimes[index] == vm.selectedTime) {
            availablePerson--;
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

<style>
.doctor-detail,
.profession {
    margin-bottom: 30px;
    border-radius: 5px;
    background-color: #ff7107;
    color: #fff;
    padding: 10px;
}
.doctor-detail {
    flex-direction: column;
    align-items: center;
    display: flex;
    padding: 20px !important;
}
.doctor-detail h4 {
    font-size: 24px;
}
.doctor-detail h5 {
    font-size: 16px;
    font-weight: 400;
    text-align: center;
}
.appointment-input-wrap {
    background-color: white;
    padding: 10px 20px;
    width: 100%;
    margin-bottom: 10px;
    border: solid 1px #e8e5e5;
    border-radius: 5px;
}
.appo-picker {
    position: absolute;
    display: none;
    background-color: white;
    max-width: 240px;
    padding: 10px;
    border: 1px solid #ccc;
    z-index: 9999;
    /* Large variation */
}
.appo-picker.is-open {
    display: flex;
    -ms-flex-direction: column;
    -webkit-flex-direction: column;
    flex-direction: column;
}
.appo-picker.is-position-static {
    display: flex;
    flex-direction: column;
    position: static;
}
.appo-picker.is-large {
    max-width: 100%;
    padding: 20px;
    border: solid 1px #e8e5e5;
    border-radius: 5px;
}
.appo-picker-title {
    font-size: 20px;
    padding-bottom: 10px;
}
.appo-picker-list {
    list-style-type: none;
    display: flex;
    flex-wrap: wrap;
    padding: 0;
    margin: 0 -5px;
}
.appo-picker-list li input {
    background-color: #f5f5f5;
    border-radius: 5px;
    padding: 10px 20px;
}
.appo-picker-list li {
    margin: 5px;
}
.appo-picker-list-item input[type='button'].is-selected {
    background-color: #29a79b;
    color: #fff;
}
.appo-picker-list-item input:hover,
.appo-picker-list-item input:active,
.appo-picker-list-item input:focus {
    color: #fff;
    background-color: #ff7107;
    outline: none;
}
.appo-picker-list-item input:disabled {
    background-color: #d3d3d3;
    opacity: 0.3;
    color: #666;
    cursor: auto;
}

.doctor-detail img {
    border-radius: 50%;
    width: 100px;
    height: 100px;
    margin-bottom: 20px;
}
.v-tabs__bar {
    background-color: #ff7107 !important;
    border-top-left-radius: 5px;
    border-top-right-radius: 5px;
}
.theme--light .v-tabs__bar .v-tabs__div {
    color: rgb(250, 250, 250);
}
@media (min-width: 960px) {
    .block-container {
        padding-left: 40px !important;
    }
}
@media (max-width: 960px) {
    .block-container {
        margin-top: 20px;
    }
}
</style>
