<template>
  <div>
    <v-dialog v-model="showModal"
              slot="activator"
              max-width="500px">
      <v-card>
        <v-card-title>
          <div class="headline-wrap">
            <a class="backBtn"
                  flat
                   @click="cancel">
              <v-icon>arrow_back</v-icon>
            </a>

            <span class="text">
              {{ formTitle }}
            </span>

            <v-btn class="btnSave"
                  flat
                  @click.native="save">
              Save
            </v-btn>
          </div>
        </v-card-title>

        <v-card-text >
           <v-layout wrap>
              <v-flex xs12 sm6 md12>
                <v-text-field v-model="editAction['name']"
                              :label="$t('branchs.branch')">
                </v-text-field>
              </v-flex>

              <v-flex xs12 sm6 md12>
                <v-text-field v-model="editAction['description']"
                              :label="$t('common.description')">
                </v-text-field>
              </v-flex>
            </v-layout>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  props: {
    editVisible: {
      type: Boolean,
      required: false
    },

    editAction: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    },

    deleteValue: {
      type: Object,
      required: false
    },

    editIndex: {
      type: Number,
      required: false
    }
  },

  data() {
    return {};
  },

  computed: {
    formTitle() {
      const vm = this;

      return vm.editIndex === -1
        ? vm.$i18n.t("branchs.addBranchsInformation")
        : vm.$i18n.t("branchs.editBranchsInformation");
    },

    showModal: {
      get() {
        const vm = this;

        return vm.editVisible;
      },
      set(value) {
        const vm = this;

        if (!value) {
          vm.$emit("cancel");
        }
      }
    }
  },

  methods: {
    cancel() {
      const vm = this;

      vm.showModal = false;
    },

    save() {
      const vm = this;

      if (vm.editIndex > -1) {
        vm.$store.dispatch("updateBranch", {
          id: vm.editAction.id,
          name: vm.editAction.name,
          description: vm.editAction.description
        });
      } else {
        vm.$store.dispatch("insertBranch", {
          name: vm.editAction.name,
          description: vm.editAction.description
        });
      }

      vm.showModal = false;
    }
  },

  created() {
    const vm = this;

    vm.$watch("deleteValue", (newValue, oldValue) => {
      if (newValue !== oldValue) {
        confirm(vm.$i18n.t("common.areYouSureWantToDelete"));
        vm.editVisible = false;
        //Silme İşlemi
      }
    });
  }
};
</script>

<style>
.headline-wrap {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
}
.card {
  border-radius: 8px;
  background-color: #fff;
  position: relative;
  margin-top: 0;
  margin-bottom: 0;
  padding: 20px 50px;
}
.card__title {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-direction: row;
}
.text {
  flex: 1;
  font-size: 18px;
  color: #000;
  margin: 0;
}
.btnSave {
  padding: 0;
  margin: 0;
  min-width: 140px;
  background-color: #ff7107 !important;
  height: 40px;
  border-radius: 6px;
  font-size: 15px;
}
.btnSave .btn__content{ color: #fff;}
.headline-wrap .btn--active .btn__content:before,
.headline-wrap .btn:focus .btn__content:before,
.headline-wrap .btn:hover .btn__content:before {
  background-color: transparent;
}
.headline-wrap .backBtn {
  position: absolute;
  left: 20px;
}
.headline-wrap .backBtn i {
  color: #ff7107 !important;
}
.label {
  font-size: 12px;
  line-height: 1.33;
  text-align: left;
  color: #464646;
}
.value {
  font-size: 16px;
  line-height: 1.5;
  text-align: left;
  color: #000;
}
</style>
