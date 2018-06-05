<template>
<div>
    <h1>Dashboard Page</h1>
    <hello-world v-for="room in rooms"
                  :key="room.id"
                  :room="room">
    </hello-world>
</div>
</template>

<script>

export default {
  computed: {
    rooms() {
      return this.$store.state.rooms;
    }
  },

  created() { //vuex store'una $store diyerek ulasabiliriz
    this.$store.dispatch('fetchRooms');
  }
}
</script>

<style>

</style>
