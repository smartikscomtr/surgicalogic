<template>
  <v-content>
    <v-container fluid fill-height>
      <v-layout align-center justify-center>
        <v-flex xs12 sm4 md4>
          <v-card class="elevation-12">
            <v-toolbar dark color="primary">
              <v-toolbar-title>
                {{ $t('login.forgotPasswordForm') }}
              </v-toolbar-title>

              <v-spacer></v-spacer>
            </v-toolbar>

            <v-card-text>
              <v-form>
                <v-text-field v-model="email"
                              prepend-icon="person"
                              name="login"
                              :label="$t('login.eposta')"
                              type="text">
                </v-text-field>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>

              <v-btn class="btnSave"
                     @click="resetPassword()">
                {{ $t('login.resetPassword') }}
              </v-btn>
            </v-card-actions>

            <v-card-actions>
              <v-spacer></v-spacer>
            </v-card-actions>

            <snackbar-component :snackbar-visible="snackbarVisible"
                                :savedMessage="savedMessage">
            </snackbar-component>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
  </v-content>
</template>

<script>

export default {
  data() {
    return {
      email:null,
      snackbarVisible: null,
      savedMessage: this.$i18n.t('login.forgotPasswordMailInfo')
    };
  },

  methods: {
    resetPassword() {
      const vm = this;

      //When the reset password button is clicked, the event is sent to the grid component
        vm.$store.dispatch('resetPassword', {
          email: vm.email
        }).then(() => {
          if (vm.email.indexOf('@') > 0) {
             vm.snackbarVisible = true;

            setTimeout(() => {
              vm.snackbarVisible = false;
            }, 2300)
          }
        })
    }
  }
};

</script>

<style>
.close-wrap{
  color: #ff7107 !important;
}
.primary {
  background-color: #009688 !important;
  border-color: #009688 !important;
}
.primary--text {
  color: #009688 !important;
}
.primary--text input,
.primary--text textarea {
  caret-color: #009688 !important;
}
.primary--after::after {
  background: #009688 !important;
}
.v-btn__content {
  color: #fff;
}
.primary {
  background-color: #009688 !important;
  border-color: #009688 !important;
}
.primary--text {
  color: #009688 !important;
}
.primary--text input,
.primary--text textarea {
  caret-color: #009688 !important;
}
.primary--after::after {
  background: #009688 !important;
}
.btnSave {
  padding: 0;
  margin: 0;
  min-width: 155px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
}
.btnSave .btn__content {
    color: #fff;
}
.btnReset {
  padding: 0;
  margin: 0;
  min-width: 155px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
}
.btnReset .btn__content {
  color: #fff;
}
.btn__content {
  will-change: box-shadow;
  -webkit-box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
      0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
  box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
      0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
}
.card__actions {
  padding: 0 40px 40px;
}
.toolbar {
  box-shadow: none !important;
}
.card__text {
    padding: 16px 45px;
}
.container.fill-height {
  background-image: url(../../images/login.jpg);
  background-size: 100% auto;
  background-repeat: no-repeat;
}
</style>
