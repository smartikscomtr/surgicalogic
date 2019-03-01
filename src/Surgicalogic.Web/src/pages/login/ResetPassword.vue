<template>
  <v-content>
    <v-container fluid fill-height>
      <v-layout align-center justify-center>
        <v-flex xs12 sm4 md4>
          <v-card class="elevation-12">
            <v-toolbar dark color="primary">
              <v-toolbar-title>
                {{ $t('login.resetPasswordForm')}}
              </v-toolbar-title>

              <v-spacer></v-spacer>
            </v-toolbar>

            <v-card-text>
              <v-form>
                <v-text-field v-model="password" prepend-icon="lock" :append-icon="showPassword ? 'visibility_off' : 'visibility'" name="password" :label="$t('login.password')" id="password" :type="showPassword ? 'text' : 'password'" @click:append="showPassword = !showPassword">
                </v-text-field>

                <v-text-field v-model="confirmPassword" prepend-icon="lock" :append-icon="showConfirmPassword ? 'visibility_off' : 'visibility'" name="confirmPassword" :label="$t('login.confirmPassword')" id="confirmPassword" :type="showConfirmPassword ? 'text' : 'password'" @click:append="showConfirmPassword = !showConfirmPassword">
                </v-text-field>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>

              <v-btn class="btnSave btn--flat" @click="updatePassword()">
                Yenile
              </v-btn>
            </v-card-actions>

            <v-card-actions>
              <v-spacer></v-spacer>
            </v-card-actions>

            <snackbar-component :snackbar-visible="snackbarVisible" :savedMessage="savedMessage">
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
            password: null,
            confirmPassword: null,
            showPassword: false,
            showConfirmPassword: false,
            snackbarVisible: null,
            savedMessage: null
        };
    },

    methods: {
        updatePassword() {
            const vm = this;

            //We are accessing userLogin in vuex store
            vm.$store
                .dispatch('updatePassword', {
                    email: vm.$route.query.email,
                    password: vm.password,
                    confirmPassword: vm.confirmPassword,
                    code: vm.$route.query.code
                })
                .then(response => {
                  if (response.data.info.succeeded) {
                     vm.savedMessage = vm.$i18n.t('login.passwordUpdated');
                      setTimeout(() => {
                         vm.$router.push("/loginpage");
                    }, 2500);
                  } else {
                    switch (response.data.info.message) {
                        case 3:
                            vm.savedMessage = vm.$i18n.t('login.userNotFound');
                            break;
                        case 4:
                            vm.savedMessage = vm.$i18n.t('login.invalidCode');
                            break;
                        case 5:
                            vm.savedMessage = vm.$i18n.t(
                                'login.passwordsDoNotMatch'
                            );
                            break;
                        default:
                            break;
                      }
                    }
                    vm.snackbarVisible = true;

                    setTimeout(() => {
                        vm.snackbarVisible = false;
                    }, 2300);
                });
        }
    },

    created() {
        const vm = this;

        if (vm.auth.isAuthenticated()) {
            vm.$router.push('/dashboardpage');
        }
    }
};
</script>
