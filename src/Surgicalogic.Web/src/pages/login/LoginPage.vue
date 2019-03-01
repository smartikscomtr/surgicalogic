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
                              :append-icon="showPassword ? 'visibility_off' : 'visibility'"
                              name="password"
                              :label="$t('login.password')"
                              id="password"
                              :type="showPassword ? 'text' : 'password'"
                              @click:append="showPassword = !showPassword"
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
    password:null,
    showPassword: false,
    snackbarVisible: null,
    savedMessage: this.$i18n.t('login.loginError')
    };
  },

  methods: {
  login() {
  const vm = this;

      //We are accessing userLogin in vuex store
      vm.$store.dispatch("userLogin", {
        email: vm.email,
        password: vm.password
      }).then(() => {
        if (vm.$store.state.loginModule.loginError) {
          vm.snackbarVisible = true;
        }

        setTimeout(() => {
          vm.snackbarVisible = false;
        }, 2300)
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
      // vm.$router.push("dashboardpage");
     }
    }
  };

</script>
