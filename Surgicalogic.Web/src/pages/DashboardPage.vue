<template>
  <div>
    <v-container grid-list-md text-xs-center>
      <v-layout row wrap>

        <v-date-picker v-model="date"
                      width="340"
                      class="mt-1">
        </v-date-picker>

        <operation-divider-component v-for="room in rooms"
                                    :key="room.id"
                                    :room="room">
        </operation-divider-component>
      </v-layout>
    </v-container>
  </div>
</template>

<script>

export default {
  data() {
    return {
      date: '2018-03-02'
    }
  },

  computed: {
    rooms() {
      const vm = this;

      return vm.$store.state.rooms;
    }
  },

  created() { //vuex store'una $store diyerek ulasabiliriz
    const vm = this;

    vm.$store.dispatch('fetchRooms');
  }
}
</script>

<style>

</style>
