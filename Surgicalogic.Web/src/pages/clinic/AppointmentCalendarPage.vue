<template>
  <div>
    <v-container class="container-wrap layout row wrap">
      <div xs12 sm4 md3 class="doctor-detail">
        <h4> {{ doctorName }} </h4>

        <h5> {{ doctorBranchNames }}</h5>

        <img :src="doctorPictureUrl" height="150px"/>
      </div>

      <v-flex xs12 sm12 md9 block-container>
        <div>
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
            <v-text-field readonly
                          slot="activator"
                          v-model="dateFormatted"
                          :label="$t('operation.operationDate')">
            </v-text-field>

            <v-date-picker v-model="date"
                           no-title
                           @input="$refs.menu.save(date)"
                           :min="getMinDate()"
                           @change="destroyPicker()"
                           :max="getMaxDate()">
            </v-date-picker>
          </v-menu>
        </div>
      </v-flex>
      <v-flex xs12 sm12 md9 block-container>
        <div>
          <input type="hidden" id="time-1">
        </div>
      </v-flex>


  <v-layout row justify-center>
    <v-dialog v-model="dialog" persistent max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">User Profile</span>
          <span>{{ availableAppointmentsMessage }}</span>
        </v-card-title>
        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md4>
                <v-text-field label="Legal first name" required></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field label="Legal middle name" hint="example of helper text only on focus"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field
                  label="Legal last name"
                  hint="example of persistent helper text"
                  persistent-hint
                  required
                ></v-text-field>
              </v-flex>
              <v-flex xs12>
                <v-text-field label="Email" required></v-text-field>
              </v-flex>
              <v-flex xs12>
                <v-text-field label="Password" type="password" required></v-text-field>
              </v-flex>
              <v-flex xs12 sm6>
                <v-select
                  :items="['0-17', '18-29', '30-54', '54+']"
                  label="Age"
                  required
                ></v-select>
              </v-flex>
              <v-flex xs12 sm6>
                <v-autocomplete
                  :items="['Skiing', 'Ice hockey', 'Soccer', 'Basketball', 'Hockey', 'Reading', 'Writing', 'Coding', 'Basejump']"
                  label="Interests"
                  multiple
                  chips
                ></v-autocomplete>
              </v-flex>
            </v-layout>
          </v-container>
          <small>*indicates required field</small>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" flat @click.native="dialog = false">Close</v-btn>
          <v-btn color="blue darken-1" flat @click.native="dialog = false">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
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
      availableAppointmentsMessage: ""
    };
  },

  watch: {
    date(val) {
      this.dateFormatted = this.formatDate(this.date)
    }
  },

  methods: {
    getMaxDate() {
      const toTwoDigits = num => num < 10 ? '0' + num : num;
      let selectDay = new Date();

      selectDay.setDate(selectDay.getDate() + 15);

      let year = selectDay.getFullYear();
      let month = toTwoDigits(selectDay.getMonth() + 1);
      let day = toTwoDigits(selectDay.getDate());

      return `${year}-${month}-${day}`;
    },

    getMinDate() {
      const toTwoDigits = num => num < 10 ? '0' + num : num;
      let selectDay = new Date();

      selectDay.setDate(selectDay.getDate() + 1);

      let year = selectDay.getFullYear();
      let month = toTwoDigits(selectDay.getMonth() + 1);
      let day = toTwoDigits(selectDay.getDate());

      return `${year}-${month}-${day}`;
    },

    formatDate(date) {
      if (!date || date.indexOf('.') > -1)
        return null;

      const [year, month, day] = date.split('-');

      return `${day}.${month}.${year}`;
    },

    getDate() {
      const toTwoDigits = num => num < 10 ? '0' + num : num;
      let today = new Date();

      today.setDate(today.getDate() + 1);

      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let day = toTwoDigits(today.getDate());

      return `${year}-${month}-${day}`;
    },

    destroyPicker(){
      const vm = this;

      vm.picker.destroy()
    }
  },

  created () {
    const vm = this;

    vm.$store.dispatch('getPersonnelById', vm.$route.query.doctorId).then(response => {
      vm.doctorName = response.data.fullName,
      vm.doctorBranchNames = response.data.branchNames,
      vm.doctorPictureUrl = response.data.pictureUrl
    })
  },

  mounted() {
    const vm = this;

    vm.$watch('date', (newValue, oldValue) => {
      if (newValue !== oldValue) {
        const AppointmentPicker = require('appointment-picker');

        vm.$store.dispatch('getAppointmentCalendarByDate', {
          selectedDate: vm.date,
          doctorId: vm.$route.query.doctorId
        }).then(response => {
          vm.interval = response.data.interval;
          vm.startTime = response.data.startTime;
          vm.endTime = response.data.endTime;
          vm.disabled = response.data.disabled;
          vm.personPerPeriod = response.data.personPerPeriod;
          vm.selectedTimes = response.data.selectedTimes;

        require(['appointment-picker'], function(AppointmentPicker) {
          vm.picker = new AppointmentPicker(
            document.getElementById('time-1'),
            {
              interval: vm.interval,//Randevu aralık dakikaları
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
            });

            vm.picker.open();
        });

        document.body.addEventListener('change.appo.picker', function(e) {
          vm.selectedTime = e.time.h + ":" + (e.time.m == 0 ?  "0" + e.time.m :e.time.m);
          var availablePerson = vm.personPerPeriod;

          for (let index = 0; index < vm.selectedTimes.length; index++) {
              if(vm.selectedTimes[index] == vm.selectedTime) {
                availablePerson--;
              }
          }

          vm.availableAppointmentsMessage = vm._i18n.locale == "tr" ?
            /* TR */ vm.selectedTime + " tarihine " + availablePerson + " randevu daha alabilirsiniz." :
            /* EN */ "You can schedule " + availablePerson + " appointment(s) at " + vm.selectedTime;

          vm.dialog = true;
        }, false);
      });
    }
  });

  vm.date = vm.getDate();
  }
}

document.on
</script>

<style>
.container-wrap {
    min-height: 904px;
    padding: 20px 50px;
}

.doctor-detail,
.profession {
    margin-bottom: 30px;
    border-radius: 5px;
    background-color: #ff7107;
    color: #fff;
    padding: 10px;
}
.doctor-detail h4 {
    font-size: 24px;
}
.doctor-detail h5 {
    font-size: 16px;
    font-weight: 400;
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
        padding-left: 40px;
    }
}
@media (max-width: 960px) {
    .block-container {
        margin-top: 20px;
    }
}
</style>
