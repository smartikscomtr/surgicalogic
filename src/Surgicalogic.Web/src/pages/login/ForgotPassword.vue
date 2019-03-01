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
