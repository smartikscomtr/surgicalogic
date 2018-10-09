<template>
  <div>
    <v-container class="container-wrap layout row wrap">
        <div xs12 sm4 md3 class="doctor-detail">

          <h4> {{ doctorName }} </h4>

          <h5>Acil Tıp</h5>
        </div>

      <v-flex xs12 sm12 md9 block-container>
        <div>
          <input id="time-1">
        </div>
      </v-flex>
    </v-container>
  </div>
</template>

<script src="js/appointment-picker.min.js"></script>

<script>

export default {
  data() {
    const vm = this;

    return {
      date: null,
      interval:null,
      startTime:null,
      endTime:null,
      disabled:[],
      doctorName: null
    };
  },

  methods: {
    getDate () {
      const toTwoDigits = num => num < 10 ? '0' + num : num;
      let today = new Date();
      today.setDate(today.getDate() + 1);
      let year = today.getFullYear();
      let month = toTwoDigits(today.getMonth() + 1);
      let day = toTwoDigits(today.getDate());
      return `${year}-${month}-${day}`;
    }
  },

  created () {
    const vm = this;

    vm.$store.dispatch('getPersonnelById', vm.$route.query.doctorId).then(response => {
      vm.doctorName = response.data.fullName
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
            vm.startTime = response.data.startTime;;
            vm.endTime = response.data.endTime;
            vm.disabled = response.data.disabled;

      require(['appointment-picker'], function(AppointmentPicker) {
      var picker = new AppointmentPicker(
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
        }
      );

      picker.open();

        });
      });
    }
});

    vm.date = vm.getDate();

  }
}
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
