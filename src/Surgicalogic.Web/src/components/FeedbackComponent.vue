<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              persistent>
      <v-card class="container fluid grid-list-md">
        <v-card-title>
          <div class="headline-wrap flex xs12 sm12 md12">
            <span class="text">
              {{ formTitle }}
            </span>

            <v-icon @click="cancel"
                    class="close-wrap">
              close
            </v-icon>
          </div>
        </v-card-title>

        <v-form ref="form" v-model="valid" lazy-validation>
        <v-card-text>
          <v-layout wrap edit-layout>
              <v-flex xs12 sm12 md12>
                <v-text-field v-model="email"
                              :label="$t('feedbacks.email')">
                </v-text-field>
              </v-flex>

            <v-flex xs12 sm12 md12>
              <v-textarea v-model="body"
                          :rules="required"
                          rows="3"
                          :label="$t('feedbacks.body')">
              </v-textarea>
            </v-flex>
              <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right margin-bottom-none>
                <v-btn class="btnSave orangeButton"
                      @click.native="save">
              {{ $t('menu.save') }}
                </v-btn>
            </v-flex>
          </v-layout>
        </v-card-text>
        </v-form>
      </v-card>
    </v-dialog>

    <snackbar-component :snackbar-visible="snackbarVisible"
                        :savedMessage="savedMessage">
    </snackbar-component>
  </div>
</template>

<script>

export default {
  data() {
    return {
      snackbarVisible: null,
      showModal: true,
      body: null,
      email: null,
      savedMessage: this.$i18n.t('feedbacks.feedbackSaved'),
      valid: true,
      required: [
        v => !!v || this.$i18n.t('common.required'),
        v => (v && v.length >= 10) || this.$i18n.t('common.moreThanTenCharacters')
      ]
    }
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.$i18n.t('feedbacks.addFeedbackInformation');
    }
  },

  methods: {
    cancel() {
      const vm = this;

      vm.clear();
      vm.showModal = false;
    },

    clear () {
        this.$refs.form.reset()
    },

    save() {
      const vm = this;

      if (!vm.$refs.form.validate()) {
        return;
      }

      vm.snackbarVisible = false;

      //We are accessing insertFeedback in vuex store
      vm.$store.dispatch('insertFeedback', {
        email: vm.email,
        body: vm.body
      }).then(() => {
        vm.snackbarVisible = true;

        setTimeout(() => {
          vm.snackbarVisible = false;
        }, 2300)
      })

      vm.showModal = false
      // vm.$emit("showModal", false);
    }
  },

  mounted () {
    const vm = this;

    vm.$watch('showModal', (newValue, oldValue) => {
      if (newValue === false) {

      setTimeout(() => {
        vm.$emit('showModal', newValue);
      }, 2300);
      }
    });
  }
}
</script>

