<template>
    <i class="loading" />
</template>

<script>

export default {

}
</script>

<style>
.loading {
    border: 5px solid #f3f3f3;
    border-top: 5px solid #292B2C;
    border-radius: 50%;
    width: 40px;
    height: 40px;
    left: 50%;
    animation: spin 1s linear infinite;
    display: inline-block;
    position: relative;
    margin: 30px 0 30px -20px;
}

/* Keyframes sürekli döndürmek için kullanılır */
@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
</style>
