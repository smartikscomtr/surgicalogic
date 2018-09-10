<template>
  <v-content>
    <v-container fluid fill-height>
      <v-layout align-center justify-center>
        <v-flex xs12 sm4 md4>
          <v-card class="elevation-12">
            <v-toolbar dark color="primary">
              <v-toolbar-title>
                {{ $t('login.loginForm') }}
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

                <v-text-field v-model="password"
                              prepend-icon="lock"
                              name="password"
                              :label="$t('login.password')"
                              id="password"
                              type="password"
                              >
                </v-text-field>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>

              <router-link :to="{ name: 'ForgotPassword' }" class="forgot-password">
                {{ $t('login.forgotPassword') }}
              </router-link>

              <v-btn class="btnSave"
                     @click="login()">
                {{ $t('login.login')}}
              </v-btn>
            </v-card-actions>

            <v-card-actions>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
  </v-content>
</template>

<script>

export default {
  data() {
  return {};
  },

  methods: {
  login() {
  const vm = this;

      //We are accessing userLogin in vuex store
      vm.$store.dispatch("userLogin", {
        email: vm.email,
        password: vm.password
      });
    },

  forgotPassword() {
  const vm = this;

      //We are accessing userLogout in vuex store
      vm.$router.push("/forgotpassword");
    }
  },

  created(){
    const vm = this;

     if (vm.auth.isAuthenticated()) {
      vm.$router.push("DashboardPage");
     }
  }
  };

</script>

<style>
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
.forgot-password {
  text-decoration: none;
  flex: 10;
  text-align: left;
  color: #ff7107 !important;
  font-size: 15px;
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
  background-image: url(../../images/doctor.jpg);
  background-size: 100% auto;
  background-repeat: no-repeat;
}
</style>
