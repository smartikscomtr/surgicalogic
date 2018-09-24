<template>
  <div>
    <v-progress-circular v-model="loading"
                         :size="150"
                         :width="15"
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

<style>
.v-progress-circular{
    margin: 172px 0 0px -124px;
    color: #80cbc4;
    background-color: #fafafa;
    position: absolute;
    left: 50%;
    right: 50%;
    border-radius: 50%;
}
</style>
