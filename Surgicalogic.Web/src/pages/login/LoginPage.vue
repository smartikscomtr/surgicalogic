<template>
  <v-content>
    <v-container fluid fill-height>
      <v-layout align-center justify-center>
        <v-flex xs12 sm8 md4>
          <v-card class="elevation-12">
            <v-toolbar dark color="primary">
              <v-toolbar-title>
                Login form
              </v-toolbar-title>

              <v-spacer></v-spacer>
            </v-toolbar>

            <v-card-text>
              <v-form>
                <v-text-field v-model="email"
                              prepend-icon="person"
                              name="login"
                              label="Login"
                              type="text">
                </v-text-field>

                <v-text-field v-model="password"
                              prepend-icon="lock"
                              name="password"
                              label="Password"
                              id="password"
                              type="password">
                </v-text-field>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>

              <v-btn class="btnSave btn--flat"
                     @click="login()">
                Login
              </v-btn>
            </v-card-actions>

            <v-card-actions>
              <v-spacer></v-spacer>

              <v-btn class="btnSave btn--flat"
                     @click="logout()">
                Logout
              </v-btn>
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

    logout() {
      const vm = this;

      //We are accessing userLogout in vuex store
      vm.$store.dispatch("userLogout");
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
.btnSave {
    padding: 0;
    margin: 0;
    min-width: 140px;
    background-color: #ff7107 !important;
    height: 40px;
    font-size: 15px;
}
.btnSave .btn__content {
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

</style>
