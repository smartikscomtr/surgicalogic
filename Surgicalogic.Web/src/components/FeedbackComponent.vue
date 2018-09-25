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

        <v-card-text>
          <v-layout wrap edit-layout>
              <v-flex xs12 sm12 md12>
                <v-text-field v-model="email"
                              :label="$t('feedbacks.email')">
                </v-text-field>
              </v-flex>

            <v-flex xs12 sm12 md12>
              <v-textarea v-model="body"
                          rows="3"
                          :label="$t('feedbacks.body')">
              </v-textarea>
            </v-flex>
              <v-flex xs12 sm12 md12 text-lg-right text-md-right text-sm-right text-xs-right margin-bottom-none>
                <v-btn class="btnSave orange"
                      @click.native="save">
                  Kaydet
                </v-btn>
            </v-flex>
          </v-layout>
        </v-card-text>
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
      savedMessage: this.$i18n.t('feedbacks.feedbackSaved')
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

      vm.$emit("showModal", false);
    },

    save() {
      const vm = this;

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

<style>
.grid-card.card {
  box-shadow: inherit;
  height: 100vh !important;
}
.grid-card .page-title h2 {
  padding-left: 24px;
  padding-top: 20px;
}
.datatable__actions {
  background-color: #f8f8f8 !important;
  padding-top: 10px;
}
.table__overflow {
  margin-top: 36px;
}
table.v-table thead tr {
  height: 70px;
}
table.v-table thead th {
  font-weight: bold;
  font-size: initial;
}
table.v-table thead td {
  font-weight: 400;
  font-size: 75%;
}
table.v-table tbody td:last-child {
  text-align: right;
}
tr:nth-child(even) {
  background-color: #f2f2f2;
}
.headline-wrap {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
}
.v-card {
  background-color: #fff;
  position: relative;
  margin-top: 0;
  margin-bottom: 0;
  box-shadow: none;
}
.grid-card.v-card {
  padding: 0 20px;
}
.v-card__title {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-direction: row;
}
.v-card__title.search-wrap {
  padding: 15px 0;
}
.v-card__title .flex {
  padding: 0 10px;
}
.v-card__text .flex {
  padding: 0 15px !important;
}
.text {
  flex: 1;
  font-size: 18px;
  color: #000;
  margin: 0;
}
.orange {
  padding: 0;
  margin: 0;
  min-width: 140px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
}
.export-wrap {
  padding: 0;
  margin: 0;
  min-width: 140px;
  background-color: #ff7107 !important;
  height: 40px;
  font-size: 15px;
  margin-right: 1%;
}
.orange .v-btn__content {
  color: #fff;
}
.close-wrap{
  color: #ff7107 !important;
}
.headline-wrap .btn--active .btn__content:before,
.headline-wrap .btn:focus .btn__content:before,
.headline-wrap .btn:hover .btn__content:before {
  background-color: transparent;
}
.headline-wrap span.text {
  /* margin: 0 10px; */
}
.headline-wrap .backBtn i {
  color: #000 !important;
}
.headline-wrap .v-btn__content {
  will-change: box-shadow;
  -webkit-box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
      0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
  box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2),
      0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
}
.label {
  font-size: 12px;
  line-height: 1.33;
  text-align: left;
  color: #464646;
  margin-bottom: 5px;
}
.value {
  font-size: 16px;
  line-height: 1.5;
  text-align: left;
  color: #000;
}
.v-dialog.v-dialog--active,
.v-dialog:not(.v-dialog--fullscreen) {
  max-width: 800px;
  width:98%;
}
a {
  color: #009688;
}
.primary {
  background-color: #009688 !important;
  border-color: #009688 !important;
}
.primary--text {
  color: #ff7107 !important;
}
.primary--text input,
.primary--text textarea {
  caret-color: #009688 !important;
}
.primary--after::after {
  background: #009688 !important;
}
.input-group-checkbox {
  display: flex;
  align-items: center;
}
.readonly-wrap {
  flex-direction: column;
}
.input-group.readonly-wrap {
  margin-bottom: 30px;
  background-color: #f7f7f7;
  padding: 10px;
}
.v-dialog:not(.dialog--fullscreen) {
  max-height: inherit;
}
.v-dialog:not(.v-dialog--fullscreen) .v-card__text {
  max-height: 50vh;
  overflow-y: auto;
}
.input-group__selections > div {
  display: inline-block !important;
  width: 100%;
  white-space: nowrap;
  overflow: hidden !important;
  text-overflow: ellipsis;
}
table.v-table tbody td:first-child,
table.v-table tbody td:not(:first-child),
table.v-table tbody th:first-child,
table.v-table tbody th:not(:first-child),
table.v-table thead td:first-child,
table.v-table thead td:not(:first-child),
table.v-table thead th:first-child,
table.v-table thead th:not(:first-child) {
  padding: 0px 15px;
  white-space: nowrap;
}
.navigation i,
.navigation .v-list__tile__title {
  color: #fff !important;
}
.application .theme--light.v-list .v-list__group__header:hover,
.application .theme--light.v-list .v-list__tile--highlighted,
.application .theme--light.v-list .v-list__tile--link:hover,
.theme--light .v-list .v-list__group__header:hover,
.theme--light .v-list .v-list__tile--highlighted,
.theme--light .v-list .v-list__tile--link:hover {
  background: rgba(0, 0, 0, 0.1);
}
.v-navigation-drawer .v-list {
  background: #333;
}
.v-list__group.v-list__group--active {
  background: #262626;
}
.v-card__title .v-btn__content {
  color: #fff;
}

.v-card__title .v-input__slot {
  width: 80%;
}
.v-input__slot {
  margin-bottom: 0;
}
.v-toolbar__content .v-input {
  position: relative;
}
.v-toolbar__content .v-input .v-input__prepend-outer {
  position: absolute;
  right: 10px;
  top: 0;
}
.v-toolbar__content .v-toolbar__title button {
  margin-left: -11px;
}
.v-toolbar__content .v-toolbar__title {
  overflow: visible;
}
.v-select.v-select--chips .v-select__selections {
  min-height: 32px;
}

input[type="date"]::-webkit-inner-spin-button {
  display: none;
  -webkit-appearance: none;
}
.v-icon.close-wrap{
    background-color:#ff7107;
    color: #fff !important;
    border-radius: 3px;
    padding: 5px;
    font-size: 18px;
    position: relative;
    top: -20px;
    right: -30px;
}
.edit-layout .flex{
  margin-bottom:10px;
}
.margin-bottom-none{
  margin-bottom: 0 !important;
}
.v-chip__content {
  background-color: #ee8b41;
  color: #fff !important;
}
</style>

