<template>
  <div>
    <h1>Hello World</h1>
    {{room.oda}}
  </div>
</template>

<script>

export default {
  props: {
    room: {
      type: [],
      required: false
    }
  },

  name: 'Hello',
  data () {
    return {}
  }
}
</script>