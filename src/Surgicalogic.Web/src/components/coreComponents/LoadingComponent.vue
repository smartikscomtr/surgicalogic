<template>
  <div>
    <v-progress-circular v-show="loading"
                         :size="40"
                         indeterminate>
    </v-progress-circular>
  </div>
</template>

<script>
export default {
  props: {
    loading: {
      type: Boolean,
      required: false
    }
  }
}
</script>
